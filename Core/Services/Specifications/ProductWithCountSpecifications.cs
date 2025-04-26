using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductWithCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithCountSpecifications(ProductSpecificationsParameters parameters) : base(
            P =>
            (!parameters.BrandId.HasValue || P.BrandId == parameters.BrandId) &&
            (!parameters.TypeId.HasValue || P.TypeId == parameters.TypeId)
        )
        {
           
        }
    }
}
