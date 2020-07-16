using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;
using StackgipEcommerce.Shared;
using StackgipInventory.Dto;
using StackgipInventory.Dto.CustomerOrder;
using StackgipInventory.Entities;
using StackgipInventory.Enums;
using StackgipInventory.Repository;
using StackgipInventory.Shared;
using NLog.Web;
using Microsoft.Extensions.Logging;
using StackgipInventory.Data;

namespace StackgipInventory.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class CustomerOrderController : Controller
    {
        private ICustomerOrderRepository _customerOrderRepository;
        private IProductInventoryRepository _productInventoryRepository;
        private IOrderLogRepository _orderLogRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerOrderController> _logger;
        public CustomerOrderController(ICustomerOrderRepository customerOrderRepository, IMapper mapper,
            IProductInventoryRepository productInventoryRepository, IOrderLogRepository orderLogRepository,
            ILogger<CustomerOrderController> logger)
        {
            _customerOrderRepository = customerOrderRepository;
            _productInventoryRepository = productInventoryRepository;
            _orderLogRepository = orderLogRepository;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet("user/{userId}")]
        public async Task<ResponseDto> GetOrder(int userId)
        {
            var customerOrders = await _customerOrderRepository.Get(userId);
            if (customerOrders == null)
                return Responses.NotFound();

            var mapped = _mapper.Map<IEnumerable<GetCustomerOrderDto>>(customerOrders);
            return Responses.Ok(mapped);
        }
        [HttpGet]
        public async Task<ResponseDto> GetOrders()
        {
            var customerOrders =  _customerOrderRepository.GetAll();
            var mapped = _mapper.Map<IEnumerable<GetCustomerOrderDto>>(customerOrders);
            return Responses.Ok(mapped);
        }
        [HttpPost]
        public async Task<ResponseDto> CreateOrder([FromBody] CreateCustomerOrderDto createCustomerOrderDto)
        {
            var productInventory = await _productInventoryRepository.Get(createCustomerOrderDto.ProductId);

            if (productInventory == null)
                return Responses.NotFound();

            if(productInventory.AvailableUnit < createCustomerOrderDto.Unit)
            {
                return Responses.Ok(null, ResponseStatus.Fail, "out of stock");
            }

            var customerOrder = _mapper.Map<CustomerOrder>(createCustomerOrderDto);

            await _customerOrderRepository.Create(customerOrder);
            var saveCustomerOrderChanges = await _customerOrderRepository.Save();
            if (!saveCustomerOrderChanges)
                return Responses.Error(); 

            var saveOrderLogChanges = await CreateOrderLog(customerOrder.CustomerId, Operation.ORDER_RECEIVED.ToString());
            if (!saveOrderLogChanges)
                return Responses.Error();

            productInventory.AvailableUnit = productInventory.AvailableUnit - createCustomerOrderDto.Unit;

            BackgroundJob.Enqueue(() => UpdateRemoteService(productInventory.ProductId, productInventory.AvailableUnit));
            //await UpdateRemoteService(productInventory.ProductId, productInventory.AvailableUnit);

            var saveProductInventoryChanges = await _productInventoryRepository.Save();
            if (!saveProductInventoryChanges)
                return Responses.Error();

            var customerOrderCreated = _mapper.Map<GetCustomerOrderDto>(customerOrder);
            return Responses.Ok(customerOrderCreated);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [AutomaticRetry(Attempts = 3)]
        [LogFailure]
        public static async Task<bool> UpdateRemoteService(int productId, decimal availableUnit) 
        {
            var message = new { unit = availableUnit};
            var success = false;
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44309/api/v1/products/" + productId.ToString(), content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var deserializeResponse = JsonConvert.DeserializeObject(apiResponse);
                    if (true)
                    {

                    }
                }
            }
            return success;
        }
        [HttpDelete("{id}")]
        public async Task<ResponseDto> DeleteCustomerOrder(int id)
        {
            var customerOrder = await _customerOrderRepository.GetById(id);
            if (customerOrder == null)
                return Responses.NotFound();

            await _customerOrderRepository.SoftDelete(customerOrder);
            var saveChanges = await _customerOrderRepository.Save();
            if (!saveChanges)
                return Responses.Error();
     
            var saveOrderLogChanges = await CreateOrderLog(customerOrder.CustomerId, Operation.ORDER_CANCELLED.ToString());
            if (!saveOrderLogChanges)
                return Responses.Error();

            return Responses.Delete("Resources deleted.");
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<bool> CreateOrderLog(int customerId, string operation)
        {
            var orderLog = new OrderLog()
            {
                CustomerOrderId = customerId,
                Operation = operation
            };
            await _orderLogRepository.Create(orderLog);
            var saveOrderLogChanges = await _orderLogRepository.Save();
            return saveOrderLogChanges;
        }
    }
}
