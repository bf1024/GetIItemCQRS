using GetItemCQRS.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetItemCQRS.Repositories
{
    public interface IItemCommandsRepository
    {
        void SaveItemData(Item item);
    }

    public class ItemCommandsRepository : IItemCommandsRepository
    {
        private readonly IMemoryCache _memoryCache;
        public ItemCommandsRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public void SaveItemData(Item item)
        {
            _memoryCache.Set(item.Id, item.Name);  
        }
    }
}
