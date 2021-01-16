using Hw_14_try.Helpers;
using Hw_14_try.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Hw_14_try.BusinessLogic
{
    public class ListAsync<T>
    {
        private readonly List<T> _source = new List<T>();
        private readonly Random _rnd = new Random();
        private readonly Logger _logger = Logger.Instance;
        private readonly ThreadSaveLogic _threadSaveLogic = ThreadSaveLogic.Instance;
        private int _randMilliseconds => _rnd.Next(100, 500);
        public async Task AddAsync(T item)
        {
            await _threadSaveLogic.ThreadSaveAction(async () =>
            {
                await Task.Delay(_randMilliseconds);
                await _logger.CallMethodWithLog(item => _source.Add(item), item);
            });
        }

        public async Task AddRangeAsync(ICollection<T> collections)
        {
            await _threadSaveLogic.ThreadSaveAction(async () =>
            {
                await Task.Delay(_randMilliseconds);
                await _logger.CallMethodWithLog(collections => _source.AddRange(collections), collections);
            });
        }

        public async Task<bool> RemoveAsync(T item)
        {
            var result = await _threadSaveLogic.ThreadSaveAction(async () =>
            {
                await Task.Delay(_randMilliseconds);
                return await _logger.CalMethodlWithLog(item => _source.Remove(item), item);
            });
            return await result;
        }

        public IReadOnlyList<T> Where(Func<T, bool> expression) => _logger.CalMethodlWithLog(expression => _source.Where(expression).ToList(), expression).GetAwaiter().GetResult();
        public IReadOnlyList<bool> Select(Func<T, bool> expression) => _logger.CallMethodWithLog(expression => _source.Select(expression).ToList(), expression).GetAwaiter().GetResult();
        public IReadOnlyList<T> OrderBy(Func<T, bool> expression) => _logger.CalMethodlWithLog(expression => _source.OrderBy(expression).ToList(), expression).GetAwaiter().GetResult();
        public IReadOnlyList<T> OrderByDistinct(Func<T, bool> expression) => _logger.CalMethodlWithLog(expression => _source.OrderByDescending(expression).ToList(), expression).GetAwaiter().GetResult();
        public IReadOnlyList<IGrouping<bool, T>> GroupBy(Func<T, bool> expression) => _logger.CallMethodWithLog(expression => _source.GroupBy(expression).ToList(), expression).GetAwaiter().GetResult();
        public IReadOnlyList<T> Take(int count) => _logger.CalMethodlWithLog(count => _source.Take(count).ToList(), count).GetAwaiter().GetResult();
        public IReadOnlyList<T> Skip(int count) => _logger.CallMethodWithLog(count => _source.Skip(count).ToList(), count).GetAwaiter().GetResult();
        public IReadOnlyList<T> Distinct() => _logger.CallMethodWithLog(() => _source.Distinct().ToList()).GetAwaiter().GetResult();
        public IReadOnlyList<T> DistinctEnum() => _logger.CallMethodWithLog(() => _source.Distinct().ToList()).GetAwaiter().GetResult();
        public IReadOnlyList<T> Expect(IEnumerable<T> someEnuberable) => _logger.CalMethodlWithLog(someEnuberable => _source.Except(someEnuberable).ToList(), someEnuberable).GetAwaiter().GetResult();
        public IReadOnlyList<T> Intersect(IEnumerable<T> someEnuberable) => _logger.CalMethodlWithLog(someEnuberable => _source.Intersect(someEnuberable).ToList(), someEnuberable).GetAwaiter().GetResult();
        public IReadOnlyList<T> Concat(IEnumerable<T> someEnuberable) => _logger.CallMethodWithLog(soneEnuberable => _source.Concat(someEnuberable).ToList(), someEnuberable).GetAwaiter().GetResult();
        public void Reverse() => _logger.CallMethodWithLog(() => _source.Reverse()).GetAwaiter().GetResult();
        public bool All(Func<T, bool> expression) => _logger.CalMethodlWithLog(expression => _source.All(expression), expression).GetAwaiter().GetResult();
        public bool Any(Func<T, bool> expression) => _logger.CallMethodWithLog(expression => _source.Any(expression), expression).GetAwaiter().GetResult();
        public int Count(Func<T, bool> expression) => _logger.CallMethodWithLog(expression => _source.Count(expression), expression).GetAwaiter().GetResult();
        public int Count() => _logger.CallMethodWithLog(() => _source.Count()).GetAwaiter().GetResult();
        public T First() => _logger.CallMethodWithLog(() => _source.First()).GetAwaiter().GetResult();
        public T FirstOrDefault() => _logger.CallMethodWithLog(() => _source.FirstOrDefault()).GetAwaiter().GetResult();
        public T Single(Func<T, bool> expression) => _logger.CallMethodWithLog(expression => _source.Single(expression), expression).GetAwaiter().GetResult();
        public T SingleOrDefault(Func<T, bool> expression) => _logger.CallMethodWithLog(expression => _source.SingleOrDefault(expression), expression).GetAwaiter().GetResult();
        public T ElementAt(int index) => _logger.CallMethodWithLog(index => _source.ElementAt(index), index).GetAwaiter().GetResult();
        public T ElementAtOrDefault(int index) => _logger.CalMethodlWithLog(index => _source.ElementAtOrDefault(index), index).GetAwaiter().GetResult();
        public T Last() => _logger.CallMethodWithLog(() => _source.Last()).GetAwaiter().GetResult();
        public T LastOrDefault() => _logger.CallMethodWithLog(() => _source.LastOrDefault()).GetAwaiter().GetResult();

        public IReadOnlyList<T> OrderByThenBy(Func<T, bool> orderBy, Func<T, bool> thenBy)
            => _logger.CallMethodWithLog(() => _source.OrderBy(orderBy).ThenBy(thenBy).ToList()).GetAwaiter().GetResult();

        public IReadOnlyList<T> OrderByThenByDescending(Func<T, bool> orderBy, Func<T, bool> thenBy)
            => _logger.CallMethodWithLog(() => _source.OrderBy(orderBy).ThenByDescending(thenBy).ToList()).GetAwaiter().GetResult();

        public IReadOnlyList<TV> Join<TU, TK, TV>(IEnumerable<TU> inner, Func<T, TK> outerKeySelector, Func<TU, TK> innerKeySelector, Func<T, TU, TV> resultSelector)
            => _logger.CallMethodWithLog(() => _source.Join(inner, outerKeySelector, innerKeySelector, resultSelector).ToList()).GetAwaiter().GetResult();

        public IEnumerator GetEnumerator()
        {
            foreach (var item in _source)
            {
                yield return item;
            }
        }
    }
}
