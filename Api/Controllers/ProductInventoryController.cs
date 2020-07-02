using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StackgipInventory.Dto.ProductInventory;
using StackgipInventory.Entities;
using StackgipInventory.Repository;

namespace StackgipInventory.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductInventoryController : Controller
    {
        private IProductInventoryRepository _productInventoryRepository;
        private readonly IMapper _mapper;
        public ProductInventoryController(IProductInventoryRepository productInventoryRepository, IMapper mapper)
        {
            _productInventoryRepository = productInventoryRepository;
            _mapper = mapper;
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductInventory(int productId)
        {
            try
            {
                var productInventory = await _productInventoryRepository.Get(productId);

                if (productInventory == null)
                    return NotFound();

                var mapped = _mapper.Map<GetProductInventoryDto>(productInventory);
                return Ok(mapped);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        [HttpGet]
        public IActionResult GetProductInventories()
        {
            var productInventories = _productInventoryRepository.GetAll();
            var mappedList = _mapper.Map<List<GetProductInventoryDto>>(productInventories);
            return Ok(mappedList);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProductInventory([FromBody] 
            CreateProductInventoryDto createProductInventoryDto)
        {
            var productInventory = _mapper.Map<ProductInventory>(createProductInventoryDto);
            await _productInventoryRepository.Create(productInventory);
            var saveChanges = await _productInventoryRepository.Save();
            if (!saveChanges)
            {
                return StatusCode(500, "An error occured while handling your request.");
            }
            var productInventoryCreated = _mapper.Map<GetProductInventoryDto>(productInventory);
            return Ok(productInventoryCreated);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProductInventory(int productId, 
            [FromBody] UpdateProductInventoryDto updateProductInventoryDto)
        {
            var productInventory = await _productInventoryRepository.Get(productId);
            if (productInventory == null)
            {
                return NotFound();
            }
            if (updateProductInventoryDto.AvailableUnit != 0)
            {
                productInventory.AvailableUnit = updateProductInventoryDto.AvailableUnit;
            }
            if (updateProductInventoryDto.Min != 0)
            {
                productInventory.Min = updateProductInventoryDto.Min;
            }
            if (updateProductInventoryDto.Max != 0)
            {
                productInventory.Max = updateProductInventoryDto.Max;
            }
            productInventory.UpdatedOn = DateTime.UtcNow;

            var saveChanges = await _productInventoryRepository.Save();
            if (!saveChanges)
            {
                return StatusCode(500, "An error occured while handling your request.");
            }
            return NoContent();
        }
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProductInventory(int productId)
        {
            var productInventory = await _productInventoryRepository.Get(productId);
            if (productInventory == null)
            {
                return NotFound();
            }
            await _productInventoryRepository.SoftDelete(productInventory);
            var saveChanges = await _productInventoryRepository.Save();
            if (!saveChanges)
            {
                return StatusCode(500, "An error occured while handling your request.");
            }
            return NoContent();
        }
    }
}
