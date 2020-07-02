using AutoMapper;
using StackgipInventory.Dto.CustomerOrder;
using StackgipInventory.Dto.OrderLog;
using StackgipInventory.Dto.ProductInventory;
using StackgipInventory.Entities;

namespace StackgipInventory.Mapper
{
    public class AutoMapping: Profile
    {
        public AutoMapping()
        {
            // create mapping here 
            CreateMap<CustomerOrder, GetCustomerOrderDto>();
            CreateMap<ProductInventory, GetProductInventoryDto>();

            CreateMap<CreateProductInventoryDto, ProductInventory>();
            CreateMap<CreateCustomerOrderDto, CustomerOrder>();
            CreateMap<CreateOrderLogDto, OrderLog>();
        }
    }
}
