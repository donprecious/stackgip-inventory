using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;
using StackgipInventory.Dto.CustomerOrder;
using StackgipInventory.Entities;
using StackgipInventory.Enums;
using StackgipInventory.Repository;

namespace StackgipInventory.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class CustomerOrderController : Controller
    {
        private ICustomerOrderRepository _customerOrderRepository;
        private IProductInventoryRepository _productInventoryRepository;
        private IOrderLogRepository _orderLogRepository;
        private readonly IMapper _mapper;
        public CustomerOrderController(ICustomerOrderRepository customerOrderRepository, IMapper mapper,
            IProductInventoryRepository productInventoryRepository, IOrderLogRepository orderLogRepository)
        {
            _customerOrderRepository = customerOrderRepository;
            _productInventoryRepository = productInventoryRepository;
            _orderLogRepository = orderLogRepository;
            _mapper = mapper;
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrder(int userId)
        {
            try
            {
                var customerOrders = await _customerOrderRepository.Get(userId);
                if (customerOrders == null)
                    return NotFound();

                var mapped = _mapper.Map<IEnumerable<GetCustomerOrderDto>>(customerOrders);
                return Ok(mapped);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var customerOrders =  _customerOrderRepository.GetAll();
                var mapped = _mapper.Map<IEnumerable<GetCustomerOrderDto>>(customerOrders);
                return Ok(mapped);
            }
            catch (Exception)
            {

                throw;
            } 
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateCustomerOrderDto createCustomerOrderDto)
        {
            var productInventory = await _productInventoryRepository.Get(createCustomerOrderDto.ProductId);

            if (productInventory == null)
                return NotFound();

            if(productInventory.AvailableUnit < createCustomerOrderDto.Unit)
            {
                var response = new { message = "out of stock", status = "failed", code = "OUT_OF_STOCK" };
                return Ok(response);
            }

            var customerOrder = _mapper.Map<CustomerOrder>(createCustomerOrderDto);

            await _customerOrderRepository.Create(customerOrder);
            var saveCustomerOrderChanges = await _customerOrderRepository.Save();
            if (!saveCustomerOrderChanges)
                return StatusCode(500, "An error occured while handling your request.");

            var saveOrderLogChanges = await CreateOrderLog(customerOrder.CustomerId, Operation.ORDER_RECEIVED.ToString());
            if (!saveOrderLogChanges)
                return StatusCode(500, "An error occured while handling your request.");

            productInventory.AvailableUnit = productInventory.AvailableUnit - createCustomerOrderDto.Unit;

            await UpdateRemoteService(productInventory.ProductId, productInventory.AvailableUnit);

            var saveProductInventoryChanges = await _productInventoryRepository.Save();
            if (!saveProductInventoryChanges)
                return StatusCode(500, "An error occured while handling your request.");



            var customerOrderCreated = _mapper.Map<GetCustomerOrderDto>(customerOrder);
            return Ok(customerOrderCreated);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<bool> UpdateRemoteService(int productId, decimal availableUnit) 
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
        public async Task<IActionResult> DeleteCustomerOrder(int id)
        {
            var customerOrder = await _customerOrderRepository.GetById(id);
            if (customerOrder == null)
                return NotFound();

            await _customerOrderRepository.SoftDelete(customerOrder);
            var saveChanges = await _customerOrderRepository.Save();
            if (!saveChanges)
                return StatusCode(500, "An error occured while handling your request.");
     
            var saveOrderLogChanges = await CreateOrderLog(customerOrder.CustomerId, Operation.ORDER_CANCELLED.ToString());
            if (!saveOrderLogChanges)
                return StatusCode(500, "An error occured while handling your request.");

            return NoContent();
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
