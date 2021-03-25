using GetItemCQRS.Models;
using GetItemCQRS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetItemCQRS.Commands
{
    public interface IItemCommands
    {
        void SaveItem(Item item);
    }
   
    public class ItemCommands : IItemCommands
    {
        private readonly IItemCommandsRepository _repository;
        public ItemCommands(IItemCommandsRepository repository)
        {
            _repository = repository;
        }
        public void SaveItem(Item item)
        {
            _repository.SaveItemData(item);
        }
    }
}
