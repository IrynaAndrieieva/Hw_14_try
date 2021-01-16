using Hw_14_try.Config;
using Hw_14_try.Enums;
using Hw_14_try.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Hw_14_try.Services
{
    public class Logger
    {
        private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());
        private int _logMessagesCount;
        private LoggerConfig _config;
        private readonly ThreadSaveLogic _threadSaveLogic = ThreadSaveLogic.Instance;

        public event Func<int, Task<string>> LogCountHandler;
        public static Logger Instance => _instance.Value;

        private Logger()
        {

        }

        public async Task<TResult> CalMethodlWithLog<TValue, TResult>(Expression<Func<TValue, TResult>> funcExp, TValue item)
        {
            var result = default(TResult);
            var info = GetMethodInfo(funcExp.Body);

            try
            {
                result = funcExp.Compile().Invoke(item);
                await LoginfoAsync($"Success call Method: {info.name} | {string.Join(" ", info.argsType)}");
            }
            catch (Exception ex)
            {
                await LogErrorAsync($"Failed Method: {info.name} with Error: {ex}");
            }
            return result;
        }

        public async Task<TResult> CallMethodWithLog<TResult>(Expression<Func<TResult>> funcExp)
        {
            var result = default(TResult);
            var info = GetMethodInfo(funcExp.Body);

            try
            {
                result = funcExp.Compile().Invoke();
                await LoginfoAsync($"Success call Method: {info.name}");
            }
            catch (Exception ex)
            {
                await LogErrorAsync($"Failed Method: {info.name} with Errror: {ex}");
            }
            return result;
        }

        public async Task CallMethodWithLog<TValue>(Expression<Action<TValue>> actionExp, TValue item)
        {
            var info = GetMethodInfo(actionExp.Body);

            try
            {
                actionExp.Compile().Invoke(item);
                await LoginfoAsync($"Success call Method: {GetMethodInfo(actionExp.Body)} | {string.Join(" ", info.argsType)}");
            }
            catch (Exception ex)
            {
                await LogErrorAsync($"Failed Method: {info.name} with Error: {ex}");
            }
        }

        public async Task CallMethodWithLog(Expression<Action> actionExp)
        {
            var info = GetMethodInfo(actionExp.Body);
            try
            {
                actionExp.Compile().Invoke();
                await LoginfoAsync($"Success call Method: {info.name}");
            }
            catch (Exception ex)
            {
                await LogErrorAsync($"Failed Method: {info.name} with Eqror: {ex}");
            }
        }

        public Task LogErrorAsync(string message, Exception ex = null)
        {
            return LogAsync(LogLevel.Error, ex != null ? $"{message}:{ex}" : message);
        }

        public Task LoginfoAsync(string message)
        {
            return LogAsync(LogLevel.Info, message);
        }

        private readonly TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

        public async Task LogAsync(LogLevel loglevel, string message)
        {
            await _threadSaveLogic.ThreadSaveAction(async () =>
            {
                var log = $"{DateTime.UtcNow}:{loglevel}:{message}";
                var logPath = $"{_config.DirectoryPath}{_config.FileName}{_config.FileExtention}";

                await FileService.Instance.WriteToFileAsync(logPath, $"{log}{Environment.NewLine}");
                Console.WriteLine(log);

                Interlocked.Increment(ref _logMessagesCount);

                if (_logMessagesCount % _config.LineSeparator == 0)
                {
                    var msglist = await OnLogCountHandler(_logMessagesCount);
                    if (msglist != null)
                    {
                        foreach (var item in msglist)
                        {
                            await FileService.Instance.WriteToFileAsync(logPath, item);
                            Console.WriteLine(item);
                        }
                    }
                }
            });
        }

        private (string name, IReadOnlyList<string> argsType) GetMethodInfo(Expression expression)
        {

            var methodCallexp = (MethodCallExpression)expression;

            var list = new List<string>(methodCallexp.Arguments.Count);

            foreach (var item in methodCallexp.Arguments)
            {
                list.Add($"ArgType:{item.GetType()} :: ArgName: {item}");
            }
            return (methodCallexp.Method.Name, list);
        }
        private async Task<IReadOnlyList<string>> OnLogCountHandler(int count)
        {
            var listDel = LogCountHandler?.GetInvocationList();
            var handlerTasks = new Task<string>[listDel.Length];
            for (int i = 0; i < listDel.Length; i++)
            {
                handlerTasks[i] = ((Func<int, Task<string>>)listDel[i])(count);
            }
            var results = await Task<string>.WhenAll(handlerTasks);
            var list = new List<string>();

            foreach (var item in results)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    list.Add($"{item}{Environment.NewLine}");
                }
            }

            return list;
        }
    }
}
