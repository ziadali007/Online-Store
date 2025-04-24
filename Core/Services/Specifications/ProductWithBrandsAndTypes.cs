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

        public ProductWithBrandsAndTypes(int? brandid, int? typeid, string? sort) : base(
            P=>        
            (!brandid.HasValue || P.BrandId==brandid) &&
            (!typeid.HasValue || P.TypeId == typeid)           
        )
        {
            ApplyInclude();
            ApplySorting(sort);
        }

        private void ApplyInclude()
        {
            AddInclude(P => P.productBrand);
            AddInclude(P => P.productType);
        }

        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "namedesc":
                        AddOrderByDescending(P => P.Name);
                        break;
                    case "priceasc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }

        }

    }
}
