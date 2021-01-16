using Hw_14_try.Helpers;
using Hw_14_try.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hw_14_try.BusinessLogic
{
    public class BusinessLogic
    {
        private readonly Timer threeSecondsTimer;
        private readonly Timer sevenSecondsTimer;
        private readonly Logger _logger = Logger.Instance;
        private readonly Random _rnd = new Random();
        private ListAsync<int> _list = new ListAsync<int>();
        private readonly ThreadSaveLogic _threadSaveLogic = ThreadSaveLogic.Instance;

        public async Task Run()
        {
            _ = new Timer(
                new TimerCallback(async o =>
                {
                    await _threadSaveLogic.ThreadSaveAction(async () =>
                    {
                        await _logger.LoginfoAsync("Trigged 3 second timer");
                        await _list.AddAsync(_rnd.Next(100, 500));
                    });
                }), null, 0, 3000);

            _ = new Timer(
                new TimerCallback(async o =>
                {
                    await _threadSaveLogic.ThreadSaveAction(async () =>
                    {
                        await _logger.LoginfoAsync("Trigged 7 second timer");
                        var distinct = _list.Distinct();
                        _list = new ListAsync<int>();
                        await _list.AddRangeAsync(distinct.ToArray());
                    });
                }), null, 0, 7000);

            for (int i = 0; i < 1; i++)
            {
                await Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    _list?.AddAsync(i);
                });
            }

            var remove = _list.RemoveAsync(0);
            await _logger.LoginfoAsync($"RemoveAsync result: {remove}");

            var select = _list.Select(x => true);
            await _logger.LoginfoAsync($"Select result: {select}");

            var where = _list.Where(x => x < 2);
            foreach (var item in where)
            {
                await _logger.LoginfoAsync($"Where result: {item}");

            }

            var orderBy = _list.OrderBy(x => x < 2);
            foreach (var item in orderBy)
            {
                await _logger.LoginfoAsync($"OrderBy result: {item}");
            }

            var groupBy = _list.OrderBy(x => x < 2);
            foreach (var item in groupBy)
            {
                await _logger.LoginfoAsync($"GroupBy result: {item}");
            }

            var take = _list.Take(3);
            foreach (var item in take)
            {
                await _logger.LoginfoAsync($"Take result: {item}");
            }

            var first = _list.FirstOrDefault();
            await _logger.LoginfoAsync($"FirstOrDefault result: {first}");
        }
    }
}
