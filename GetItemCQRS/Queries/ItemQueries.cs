using GetItemCQRS.Models;
using GetItemCQRS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetItemCQRS.Queries
{
    public interface IItemQueries
    {
        Item FindByID(string guid);
    }
    public class ItemQueries : IItemQueries
    {
        private readonly IItemQueriesRepository _repository;
        public ItemQueries(IItemQueriesRepository repository)
        {
            _repository = repository;
        }
        public Item FindByID(string guid)
        {
            var name = _repository.GetNameByID(guid);

            return new Item()
            {
                Id = guid,
                Name = name
            };
        }
    }
}
