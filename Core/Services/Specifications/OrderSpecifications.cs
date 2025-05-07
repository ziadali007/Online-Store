using Domain.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class OrderSpecifications : BaseSpecifications<Order,Guid>
    {
        public OrderSpecifications(Guid id) : base(x => x.Id == id)
        {
            AddInclude(x => x.OrderItems);
            AddInclude(x => x.DeliveryMethod);
        }

        public OrderSpecifications(string userEmail) : base(x => x.UserEmail==userEmail)
        {
            AddInclude(x => x.OrderItems);
            AddInclude(x => x.DeliveryMethod);
            AddOrderBy(x => x.OrderDate);
        }
    }
}
