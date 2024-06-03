using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiGraph.src.application.services;
using Microsoft.Graph.Models;
using ApiGraph.src.domain;

namespace ApiGraph.src.adapters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharepointController : ControllerBase
    {

        private readonly ISharepointService _sharepointService;

        public SharepointController(ISharepointService sharepointService)
        {
            _sharepointService = sharepointService;
        }

        [HttpGet]
        [Route("get-all-items")]
        public Task<List<Dictionary<string, object>>?> ReadItems()
        {
            return _sharepointService.ReadItems();
        }

        [HttpPost]
        [Route("add-item")]
        public Task<ListItem?> CreateItem([FromBody] ToDoItem item)
        {
            return _sharepointService.CreateItem(item);
        }

        [HttpPut]
        [Route("update-item/{itemId}")]
        public async Task<IActionResult> UpdateItem(string itemId, [FromBody] ToDoItem item)
        {
            var updatedItem = await _sharepointService.UpdateItem(itemId, item);
            if (updatedItem == null)
            {
                return NotFound();
            }

            return Ok(updatedItem);
        }

        [HttpDelete]
        [Route("delete-item/{itemId}")]
        public async Task<IActionResult> DeleteItem(string itemId)
        {
            var isDeleted = await _sharepointService.DeleteItem(itemId);
            if (isDeleted)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("get-item/{itemId}")]
        public async Task<ListItem?> ReadItem(string itemId)
        {
            return await _sharepointService.ReadItem(itemId);
        }
    }
}
