using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBasketById(string id)
        {
            var basket= await serviceManager.BasketService.GetBasketAsync(id);

            return Ok(basket);
            
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBasket(BasketDto basketDto)
        {
            var basket = await serviceManager.BasketService.UpdateBasketAsync(basketDto);

            return Ok(basket);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            await serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }


    }
}
