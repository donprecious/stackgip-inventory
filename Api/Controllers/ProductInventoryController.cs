using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StackgipEcommerce.Shared;
using StackgipInventory.Dto;
using StackgipInventory.Dto.ProductInventory;
using StackgipInventory.Entities;
using StackgipInventory.Repository;
using StackgipInventory.Shared;

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
        public async Task<ResponseDto> GetProductInventory(int productId)
        {
            var productInventory = await _productInventoryRepository.Get(productId);

            if (productInventory == null)
                return Responses.NotFound();

            var mapped = _mapper.Map<GetProductInventoryDto>(productInventory);
            return Responses.Ok(mapped);
        }
        [HttpGet]
        public ResponseDto GetProductInventories()
        {
            var productInventories = _productInventoryRepository.GetAll();
            var mappedList = _mapper.Map<List<GetProductInventoryDto>>(productInventories);
            return Responses.Ok(mappedList);
        }
        [HttpPost]
        public async Task<ResponseDto> CreateProductInventory([FromBody] 
            CreateProductInventoryDto createProductInventoryDto)
        {
            var productInventory = _mapper.Map<ProductInventory>(createProductInventoryDto);
            await _productInventoryRepository.Create(productInventory);
            var saveChanges = await _productInventoryRepository.Save();
            if (!saveChanges)
            {
                return Responses.Error();
            }
            var productInventoryCreated = _mapper.Map<GetProductInventoryDto>(productInventory);
            return Responses.Ok(productInventoryCreated);
        }
        [HttpPut]
        public async Task<ResponseDto> UpdateProductInventory(int productId, 
            [FromBody] UpdateProductInventoryDto updateProductInventoryDto)
        {
            var productInventory = await _productInventoryRepository.Get(productId);
            if (productInventory == null)
            {
                return Responses.NotFound();
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
                return Responses.Error();
            }
            return Responses.Ok(productInventory,ResponseStatus.Success,"Resource updated.");
        }
        [HttpDelete("{productId}")]
        public async Task<ResponseDto> DeleteProductInventory(int productId)
        {
            var productInventory = await _productInventoryRepository.Get(productId);
            if (productInventory == null)
            {
                return Responses.NotFound();
            }
            await _productInventoryRepository.SoftDelete(productInventory);
            var saveChanges = await _productInventoryRepository.Save();
            if (!saveChanges)
            {
                return Responses.Error();
            }
            return Responses.Delete("Resources deleted.");
        }
    }
}
