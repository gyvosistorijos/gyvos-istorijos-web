using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Hintme.Models
{
    public class CachingRepository : IHintRepository
    {
        private readonly IHintRepository _repository;
        private readonly MemoryCache _cache;
        private readonly TimeSpan _cacheTime;

        public CachingRepository(IHintRepository repository, TimeSpan cacheTime)
        {
            _repository = repository;
            _cache = new MemoryCache(new MemoryCacheOptions());
            _cacheTime = cacheTime;
        }

        public IEnumerable<Hint> GetHints()
        {
            var result = _cache.Get("Hints") as IEnumerable<Hint>;
            if (result != null)
            {
                return result;
            }

            result = _repository.GetHints();

            _cache.Set("Hints", result, DateTimeOffset.Now.Add(_cacheTime));

            return result;
        }

        public void SaveHint(Hint hint)
        {
            _repository.SaveHint(hint);

            var result = _cache.Get("Hints") as IEnumerable<Hint>;

            var list = result.ToList();
            list.Add(hint);

            _cache.Set("Hints", list, DateTimeOffset.Now.Add(_cacheTime));
        }
    }
}