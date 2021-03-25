using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetItemCQRS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace GetItemCQRS.Repositories
{
    public interface IItemQueriesRepository
    {
        string GetNameByID(string guid);
    }
    public class ItemQueriesRepository : IItemQueriesRepository
    {
        private readonly IMemoryCache _memoryCache;
        public ItemQueriesRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public string GetNameByID(string guid)
        {
            string name = "";
            if(!_memoryCache.TryGetValue(guid, out name))
                throw new KeyNotFoundException();

            return name;
        }
    }
}
