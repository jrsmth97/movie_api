using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using FluentValidation.Results;
using Newtonsoft.Json;
using movie_api.Models;
using movie_api.Validations;
using movie_api.Repositories;
using movie_api.Attributes;

namespace movie_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IConfiguration _configuration;

        public OrdersController(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            ILogger<OrdersController> logger,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("/api/orders")]
        public async Task<IActionResult> GetOrders()
        {
            IList<Orders> orders = await _orderRepository.GetListAsync();
            _logger.LogInformation("[GET: /api/orders] All Orders data accessed");
            return Ok(orders);
        }

        [Authorize]
        [HttpGet("/api/orders/{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            Orders order = await _orderRepository.GetAsync(id);
            if (order == null) {
                _logger.LogInformation("[GET: /api/orders/{id}] Order data not found ( id : " + id + ")");
                return NotFound("Order not found");
            }

            _logger.LogInformation("[GET: /api/orders/{id}] Order data accessed ( id : " + id + ")");
            return Ok(order);
        }

        [Authorize]
        [HttpPost("/api/orders")]
        public async Task<IActionResult> AddOrder(Orders order)
        {
            OrdersValidation Obj = new OrdersValidation();
            ValidationResult Result = Obj.Validate(order);

            if (Result.IsValid == false) {
                return BadRequest(Result);
            }

            Users user = await _userRepository.GetAsync(order.user_id);
            if (user == null)
                return BadRequest("User id not exists");


            order.user = user;
            _logger.LogInformation($"[POST: /api/orders] Ordercreation requested => '{JsonConvert.SerializeObject(order)}'");
            Orders createOrder = await _orderRepository.CreateAsync(order);

            return Ok(createOrder);
        }

        [Authorize(Roles = "1")]
        [HttpDelete("/api/orders/delete/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            Orders order = await _orderRepository.DeleteAsync(id);
           
            if (order == null) 
            {
                _logger.LogInformation("[DELETE: /api/orders/delete/{id}] not exist order ( id : " + id + ")");
                return NotFound("Order not found");
            }
            
            _logger.LogInformation("[DELETE: /api/orders/delete/{id}] Deleting Order ( id : "+id+")");
            return Ok(order);

        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/orders/delete/{id}")]
        public async Task<IActionResult> SoftDeleteOrder(int id)
        {
            Orders order = await _orderRepository.SoftDeleteAsync(id);
           
            if (order == null) 
            {
                _logger.LogInformation("[PATCH: /api/orders/delete/{id}] not exist Order ( id : " + id + ")");
                return NotFound("Order not found");
            }
            
            _logger.LogInformation("[PATCH: /api/orders/delete/{id}] Deleting Order ( id : "+id+")");
            return Ok(order);
        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/orders/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Orders order)
        {
            Orders updateOrder = await _orderRepository.UpdateAsync(id, order);
             
            if (updateOrder == null) 
            {
                _logger.LogInformation("[PATCH: /api/orders/{id}] not exist Order ( id : " + id + ")");
                return NotFound("Order not found");
            }
            
            _logger.LogInformation("[PATCH: /api/orders/{id}] Updating Order ( id : "+ id +")");
            return Ok(updateOrder);
        }
    }
}