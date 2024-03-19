using KeyValueStore.Models;
using KeyValueStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace KeyValueStore.Controllers;

[ApiController]
[Route("api/gateway-node")]
public class KeyValueController(JsonFileStoreService storeService) : ControllerBase
{
    [HttpGet("strong-get/{key}")]
    public ActionResult<KeyValueItem> StrongGet(string key)
    {
        var item = storeService.GetItem(key);
        if (item == null) return NotFound();
        return item;
    }
    
    [HttpGet("eventual-get/{key}")]
    public ActionResult<KeyValueItem> EventualGet(string key)
    {
        var item = storeService.GetItem(key);
        if (item == null) return NotFound();
        return item;
    }

    [HttpPost("compare-and-swap")]
    public IActionResult CompareAndSwap([FromBody] KeyValueItem item)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        storeService.AddItem(item);
        return CreatedAtAction(nameof(StrongGet), new { key = item.Key }, item);
    }
}