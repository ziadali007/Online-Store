using AutoMapper;
using Domain.Contracts;
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
        public async Task<IEnumerable<ProductResultDto>> GetAllProductsAsync(int? brandid, int? typeid, string? sort)
        {
            var Spec = new ProductWithBrandsAndTypes(brandid,typeid,sort);
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(Spec);

            var result=mapper.Map<IEnumerable<ProductResultDto>>(products);

            return result;
        }

        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var product = await unitOfWork.GetRepository<Product,int>().GetAsync(id);

            if (product == null) return null;

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
