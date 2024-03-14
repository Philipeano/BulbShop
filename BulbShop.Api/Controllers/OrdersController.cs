using BulbShop.Api.Models;
using BulbShop.Common.DTOs.Order;
using BulbShop.Common.DTOs.Product;
using BulbShop.Common.Enums;
using BulbShop.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace BulbShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public OrdersController(ILogger<OrdersController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }


        [HttpGet]  // https://localhost:7057/api/Orders
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderSummaryDTO>))]
        public IActionResult Get()
        {
            return Ok(_unitOfWork.OrderRepository.GetAllOrders());
        }


        [HttpGet("{id}")]  // https://localhost:7057/api/Orders/some-guid-value-for-id
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderWithItemsDTO))]
        public IActionResult Get(Guid id)
        {
            var matchedOrder = _unitOfWork.OrderRepository.GetOrder(id);
            if (matchedOrder == null)
            {
                return NotFound(new object());
            }
            return Ok(matchedOrder);
        }


        [HttpPost]  // https://localhost:7057/api/Orders
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderSummaryDTO))]
        public IActionResult Add([FromBody] AddOrderDTO order)
        {
            if (order == null || !ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse("Request has missing or invalid data.", HttpStatusCode.BadRequest));
            }

            if (order.CustomerId == Guid.Empty || _unitOfWork.CustomerRepository.GetCustomer(order.CustomerId) == null)
            {
                return BadRequest(new ErrorResponse("No customer exists with the supplied CustomerId. ", HttpStatusCode.BadRequest));
            }

            if (!order.Items.Any())
            {
                return BadRequest(new ErrorResponse("At least one product must be added to create an order.", HttpStatusCode.BadRequest));
            }

            var addedOrder = _unitOfWork.OrderRepository.AddOrder(order);
            _unitOfWork.Commit();

            if (addedOrder == null)
            {
                return BadRequest(new ErrorResponse("Unable to complete the save operation due to invalid data.", HttpStatusCode.BadRequest));
            }

            return Created($"{Request.Path}/{addedOrder.Id}", addedOrder);
        }
    }
}






/*
 * 
------------------------------------------------------------
ORDER ENDPOINTS
------------------------------------------------------------
GET 	api/orders

GET 	api/orders/{id}

POST 	api/orders

PATCH 	api/orders/{id}/update?newStatus={newStatus}

DELETE 	api/orders/{id}


------------------------------------------------------------
ORDER ITEM ENDPOINTS
------------------------------------------------------------

GET 	api/orders/{id}/items

POST 	api/orders/{id}/items		 	          // Payload should specify productId and quantity

PUT 	api/orders/{id}/items/update?productId={productId}     // Payload should specify new quantity

DELETE 	api/orders/{id}/items?productId={productId}

 */