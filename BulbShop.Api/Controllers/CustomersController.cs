using BulbShop.Api.Models;
using BulbShop.Common.DTOs.Customer;
using BulbShop.Common.Enums;
using BulbShop.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BulbShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // https://localhost:7057/api/Customers
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CustomersController(ILogger<CustomersController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }


        [HttpGet()]  // https://localhost:7057/api/Customers
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CustomerDto>))]
        public IActionResult Get()
        {
            return Ok(_unitOfWork.CustomerRepository.GetAllCustomers());
        }


        [HttpGet("{id}")] // https://localhost:7057/api/Customers/some-guid-value-for-id
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(object))]
        public IActionResult Get(Guid id)
        {
            var matchedCustomer = _unitOfWork.CustomerRepository.GetCustomer(id);
            if (matchedCustomer == null)
            {
                return NotFound(new object());
            }
            return Ok(matchedCustomer);
        }


        [HttpPost]  // https://localhost:7057/api/Customers
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CustomerDto))]
        public IActionResult Add([FromBody] AddCustomerDto customer)
        {
            if (customer == null || !ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse("Request has missing or invalid data.", HttpStatusCode.BadRequest));
            }

            // TODO: Validate all other fields (PhoneNumber and EmailAddress) to ensure that only valid customer information is saved.            

            var addedCustomer = _unitOfWork.CustomerRepository.AddCustomer(customer);
            _unitOfWork.Commit();

            if (addedCustomer == null)
            {
                return BadRequest(new ErrorResponse("Unable to complete the save operation due to invalid data.", HttpStatusCode.BadRequest));
            }

            return Created($"{Request.Path}/{addedCustomer.Id}", addedCustomer);

        }


        // Edit a customer definition
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDto))]
        public IActionResult Update([FromRoute] Guid id, [FromBody] CustomerDto customer)
        {
            // Check that a customer with the specified id exists
            var matchedCustomer = _unitOfWork.CustomerRepository.GetCustomer(id);
            if (matchedCustomer == null)
            {
                return NotFound(new ErrorResponse("Invalid operation! No customer exists with the specified Id.", HttpStatusCode.NotFound));
            }

            // Validate the fields of the incoming payload
            if (customer == null || !ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse("Request has missing or invalid data.", HttpStatusCode.BadRequest));
            }

            // TODO: Validate all other fields (PhoneNumber and EmailAddress) to ensure that only valid customer information is saved.            

            var updatedCustomer = _unitOfWork.CustomerRepository.UpdateCustomer(customer);
            _unitOfWork.Commit();

            if (updatedCustomer == null)
            {
                return BadRequest(new ErrorResponse("Unable to complete the save operation due to invalid data.", HttpStatusCode.BadRequest));
            }

            return Ok(updatedCustomer);
        }


        // TODO: Delete a customer definition

        [HttpDelete("{id}")]  // https://localhost:7057/api/Customers/some-guid-value-for-id
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        public IActionResult Delete(Guid id)
        {
            var matchedCustomer = _unitOfWork.CustomerRepository.GetCustomer(id);
            if (matchedCustomer == null)
            {
                return NotFound(new ErrorResponse("The specified id does not match any known customer.", HttpStatusCode.NotFound));
            }

            _unitOfWork.CustomerRepository.DeleteCustomer(id);
            _unitOfWork.Commit();
            return Ok();
        }
    }
}
