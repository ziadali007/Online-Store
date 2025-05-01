using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string id);

        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket,TimeSpan? timeToLive=null);

        Task<bool> DeleteBasketAsync(string id);
    }
}
