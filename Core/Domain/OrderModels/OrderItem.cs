using Domain.Models;

namespace Domain.OrderModels
{
    public class OrderItem : BaseEntity<Guid>
    {
        public OrderItem() { }
        public OrderItem(ProductInOrderItem productInOrderItem, int quantity, decimal price)
        {
            product = productInOrderItem;
            Quantity = quantity;
            Price = price;
        }

        public ProductInOrderItem product { get; set; }


        public int Quantity { get; set; }


        public decimal Price { get; set; }
    }
}