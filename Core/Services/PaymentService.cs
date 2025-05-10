using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Domain.OrderModels;
using Microsoft.Extensions.Configuration;
using Services.Abstractions;
using Shared;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProduct = Domain.Models.Product;

namespace Services
{
    public class PaymentService(IBasketRepository basketRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IConfiguration configuration) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            var basket = await basketRepository.GetBasketAsync(basketId);

            if(basket is null) throw new BasketNotFoundException(basketId);

            foreach(var item in basket.Items)
            {
                var product= await unitOfWork.GetRepository<OrderProduct, int>().GetAsync(item.Id);

                if (product is null) throw new ProductNotFoundException(item.Id);

                item.Price = product.Price;

            }

            if(!basket.DeliveryMethodId.HasValue) throw new Exception("Invalid Delivery method Id");

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetAsync(basket.DeliveryMethodId.Value);

            if (deliveryMethod is null) throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);

            basket.ShippingPrice = deliveryMethod.Cost;

            var amount=(long) (basket.Items.Sum(item=>item.Price * item.Quantity) + basket.ShippingPrice)*100;

            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];

            var service = new PaymentIntentService();

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var createOptions= new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>()
                    {
                        "card"
                    }
                };

                var PaymentIntent = await service.CreateAsync(createOptions);
                basket.ClientSecret = PaymentIntent.ClientSecret;
                basket.PaymentIntentId = PaymentIntent.Id;
            }
            else
            {

                var updateOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = amount,
                };
                await service.UpdateAsync(basket.PaymentIntentId, updateOptions);
            }

            await basketRepository.UpdateBasketAsync(basket);

            var result = mapper.Map<BasketDto>(basket);

            return result;


        }
    }
}
