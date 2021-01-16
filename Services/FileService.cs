using Hw_14_try.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Hw_14_try.Services
{
    public class FileService
    {
        private static readonly Lazy<FileService> _instance = new Lazy<FileService>(() => new FileService());

        public static FileService Instance => _instance.Value;
        private readonly ThreadSaveLogic _threadSaveLogic = ThreadSaveLogic.Instance;

        private FileService()

        {

        }

        public Task WriteToFileAsync(string path, string msg)
        {
            return _threadSaveLogic.ThreadSaveAction(async () => await File.AppendAllTextAsync(path, msg));

        }

        public async Task<IReadOnlyList<string>> ReadLinesAsync(string path, int count)
        {
            var result = await _threadSaveLogic.ThreadSaveAction(async () =>
            {
                using (var sr = new StreamReader(path, Encoding.Default))
                {
                    var list = new List<string>();
                    for (int i = 0; i < count; i++)
                    {
                        var Line = await sr.ReadLineAsync();
                        list.Add(Line);
                    }
                    return list;
                }
            });

            return await result;
        }
    }
}
