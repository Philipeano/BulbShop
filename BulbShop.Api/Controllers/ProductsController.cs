using BulbShop.Api.Models;
using BulbShop.Common.DTOs.Product;
using BulbShop.Common.Enums;
using BulbShop.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BulbShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // https://localhost:7057/api/Products
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(ILogger<ProductsController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }


        [HttpGet()]  // https://localhost:7057/api/Products
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDto>))]
        public IActionResult Get()
        {
            return Ok(_unitOfWork.ProductRepository.GetAllProducts());
        }


        [HttpGet("{id}")] // https://localhost:7057/api/Products/some-guid-value-for-id
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(object))]
        public IActionResult Get(Guid id)
        {
            var matchedProduct = _unitOfWork.ProductRepository.GetProduct(id);
            if (matchedProduct == null)
            {
                return NotFound(new object());
            }
            return Ok(matchedProduct);
        }


        [HttpPost]  // https://localhost:7057/api/Products
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductDto))]
        public IActionResult Add([FromBody] AddProductDto product)
        {
            if (product == null || !ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse("Request has missing or invalid data.", HttpStatusCode.BadRequest));
            }

            if (!Enum.IsDefined(typeof(ProductCategory), product.Category))
            {
                return BadRequest(new ErrorResponse("The value supplied for Category is invalid.", HttpStatusCode.BadRequest));
            }

            if (!Enum.IsDefined(typeof(Manufacturer), product.Manufacturer))
            {
                return BadRequest(new ErrorResponse("The value supplied for Manufacturer is invalid.", HttpStatusCode.BadRequest));
            }

            // TODO: Validate all other fields (InitialQuantity and Price) to ensure that only valid product information is saved.            

            var addedProduct = _unitOfWork.ProductRepository.AddProduct(product);
            _unitOfWork.Commit();

            if (addedProduct == null)
            {
                return BadRequest(new ErrorResponse("Unable to complete the save operation due to invalid data.", HttpStatusCode.BadRequest));
            }

            return Created($"{Request.Path}/{addedProduct.Id}", addedProduct);

        }


        // Edit a product definition
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public IActionResult Update([FromRoute] Guid id, [FromBody] ProductDto product)
        {
            // Check that a product with the specified id exists
            var matchedProduct = _unitOfWork.ProductRepository.GetProduct(id);
            if (matchedProduct == null)
            {
                return NotFound(new ErrorResponse("Invalid operation! No product exists with the specified Id.", HttpStatusCode.NotFound));
            }

            // Validate the fields of the incoming payload
            if (product == null || !ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse("Request has missing or invalid data.", HttpStatusCode.BadRequest));
            }

            if (!Enum.IsDefined(typeof(ProductCategory), product.Category))
            {
                return BadRequest(new ErrorResponse("The value supplied for Category is invalid.", HttpStatusCode.BadRequest));
            }

            if (!Enum.IsDefined(typeof(Manufacturer), product.Manufacturer))
            {
                return BadRequest(new ErrorResponse("The value supplied for Manufacturer is invalid.", HttpStatusCode.BadRequest));
            }

            // TODO: Validate all other fields (QuantityInStock and Price) to ensure that only valid product information is saved.

            var updatedProduct = _unitOfWork.ProductRepository.UpdateProduct(product);
            _unitOfWork.Commit();

            if (updatedProduct == null)
            {
                return BadRequest(new ErrorResponse("Unable to complete the save operation due to invalid data.", HttpStatusCode.BadRequest));
            }

            return Ok(updatedProduct);
        }


        // TODO: Delete a product definition

        [HttpDelete("{id}")]  // https://localhost:7057/api/Products/some-guid-value-for-id
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        public IActionResult Delete(Guid id)
        {
            var matchedProduct = _unitOfWork.ProductRepository.GetProduct(id);
            if (matchedProduct == null)
            {
                return NotFound(new ErrorResponse("The specified id does not match any known product.", HttpStatusCode.NotFound));
            }

            _unitOfWork.ProductRepository.DeleteProduct(id);
            _unitOfWork.Commit();
            return Ok();
        }
   }
}