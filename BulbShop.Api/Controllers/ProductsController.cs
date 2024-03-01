using BulbShop.Common.DTOs.Product;
using BulbShop.Data;
using Microsoft.AspNetCore.Mvc;

namespace BulbShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(ILogger<ProductsController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public ActionResult<IEnumerable<BaseProductModel>> Get()
        {
            return Ok(_unitOfWork.ProductRepository.GetAllProducts());
        }


        [HttpGet("/{id}")]
        public ActionResult<BaseProductModel> Get(Guid id)
        {
            var matchedProduct = _unitOfWork.ProductRepository.GetProduct(id); 
            if(matchedProduct == null)
            {
                return NotFound();
            }
            return Ok(matchedProduct);
        }
    }
}