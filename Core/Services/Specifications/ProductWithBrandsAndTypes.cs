using Domain.Models;
using Shared;
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

        public ProductWithBrandsAndTypes(ProductSpecificationsParameters productSpecParams) : base(
            P=>        
            (string.IsNullOrEmpty(productSpecParams.Search) || P.Name.ToLower().Contains(productSpecParams.Search.ToLower()))&&
            (!productSpecParams.BrandId.HasValue || P.BrandId== productSpecParams.BrandId) &&
            (!productSpecParams.TypeId.HasValue || P.TypeId == productSpecParams.TypeId)           
        )
        {
            ApplyInclude();
            ApplySorting(productSpecParams.Sort);
            ApplyPagination(productSpecParams.PageIndex, productSpecParams.PageSize);
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
