using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductWithBrandsAndTypes : BaseSpecifications<Product,int>
    {

        public ProductWithBrandsAndTypes(int id): base(P=>P.Id == id)
        {
            ApplyInclude();
        }

        public ProductWithBrandsAndTypes(int? brandid, int? typeid) : base(
            P=>        
            (!brandid.HasValue || P.BrandId==brandid) &&
            (!typeid.HasValue || P.TypeId == typeid)           
        )
        {
            ApplyInclude();
        }

        private void ApplyInclude()
        {
            AddInclude(P => P.productBrand);
            AddInclude(P => P.productType);
        }




    }
}
