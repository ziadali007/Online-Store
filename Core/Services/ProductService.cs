using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork,IMapper mapper) : IProductService
    {
        public async Task<PaginationResponse<ProductResultDto>> GetAllProductsAsync(ProductSpecificationsParameters productSpecParams)
        {
            var Spec = new ProductWithBrandsAndTypes(productSpecParams);
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(Spec);

            var SpecCount= new ProductWithCountSpecifications(productSpecParams);

            var count=await unitOfWork.GetRepository<Product, int>().CountAsync(SpecCount);

            var result=mapper.Map<IEnumerable<ProductResultDto>>(products);

            return new PaginationResponse<ProductResultDto>(productSpecParams.PageIndex,productSpecParams.PageSize,count,result);
        }

        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var product = await unitOfWork.GetRepository<Product,int>().GetAsync(id);

            if (product == null) throw new ProductNotFoundException(id);

            var result=mapper.Map<ProductResultDto>(product);

            return result;

        }
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var Brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();

            var result=mapper.Map<IEnumerable<BrandResultDto>>(Brands);

            return result;
        }

       
        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();

            var result=mapper.Map<IEnumerable<TypeResultDto>>(types);

            return result;
        }

       
    }
}
