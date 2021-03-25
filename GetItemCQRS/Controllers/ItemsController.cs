using GetItemCQRS.Commands;
using GetItemCQRS.Models;
using GetItemCQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GetItemCQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemCommands _itemCommands;
        private readonly IItemQueries _itemQueries;
        public ItemsController(IItemCommands itemCommands, IItemQueries itemQueries)
        {
            _itemCommands = itemCommands;
            _itemQueries = itemQueries;
        }
        // GET: api/<ItemsController>
        [HttpGet("GetItem/{guid}")]
        public ActionResult Get(string guid)
        {
            Item item = new Item();
            try { item = _itemQueries.FindByID(guid); }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/<ItemsController>
        [HttpPost("AddItem/{guid}")]
        public void Post(string guid, [FromBody] string name)
        {
            _itemCommands.SaveItem(new Item() { Id = guid, Name = name });
        }
    }
}
