using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using FluentValidation.Results;
using Newtonsoft.Json;
using movie_api.Models;
using movie_api.Validations;
using movie_api.Repositories;
using movie_api.Attributes;
using movie_api.Utils;

namespace movie_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMovieScheduleRepository _movieScheduleRepository;
        private readonly ILogger<OrderItemsController> _logger;
        private readonly IConfiguration _configuration;
        
        public OrderItemsController(
            IOrderItemRepository orderItemRepository,
            IOrderRepository orderRepository,
            IMovieScheduleRepository movieScheduleRepository,
            ILogger<OrderItemsController> logger,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _movieScheduleRepository = movieScheduleRepository;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("/api/order-items")]
        public async Task<IActionResult> GetOrderItems()
        {
            IList<OrderItems> orderItems = await _orderItemRepository.GetListAsync();
            _logger.LogInformation("[GET: /api/order-items] All OrderItems data accessed");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.GET_OK,
                orderItems
            ));
        }

        [Authorize]
        [HttpGet("/api/order-items/{id}")]
        public async Task<IActionResult> GetOrderItem(int id)
        {
            OrderItems orderItem = await _orderItemRepository.GetAsync(id);
            if (orderItem == null) {
                _logger.LogInformation("[GET: /api/order-items/{id}] OrderItem data not found ( id : " + id + ")");
                return NotFound(ResponseBuilder.FailedResponse(
                    ResponseBuilder.NOT_FOUND,
                    new string[] { "Order item not found" },
                    StatusCodes.Status404NotFound
                ));
            }

            _logger.LogInformation("[GET: /api/order-items/{id}] OrderItem data accessed ( id : " + id + ")");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.GET_OK,
                orderItem
            ));
        }

        [Authorize]
        [HttpPost("/api/order-items")]
        public async Task<IActionResult> AddOrderItem(OrderItems orderItem)
        {
            OrderItemsValidation Obj = new OrderItemsValidation();
            ValidationResult Result = Obj.Validate(orderItem);

            if (Result.IsValid == false) {
                return BadRequest(ResponseBuilder.FailedResponse(
                    ResponseBuilder.CREATE_FAILED,
                    Result,
                    StatusCodes.Status400BadRequest
                ));
            }
            
            Orders order = await _orderRepository.GetAsync(orderItem.order_id);
            if (order == null)
                return BadRequest(ResponseBuilder.FailedResponse(
                    ResponseBuilder.CREATE_FAILED,
                    new string[] { "Order id not exists" },
                    StatusCodes.Status400BadRequest
                ));

            MovieSchedules movieSchedule = await _movieScheduleRepository.GetAsync(orderItem.movie_schedule_id);
            if (movieSchedule == null)
                return BadRequest(ResponseBuilder.FailedResponse(
                    ResponseBuilder.CREATE_FAILED,
                    new string[] { "Movie schedule id not exists" },
                    StatusCodes.Status400BadRequest
                ));
            
            orderItem.order = order;
            orderItem.movieSchedule = movieSchedule;
            
            _logger.LogInformation($"[POST: /api/order-items] OrderItemcreation requested => '{JsonConvert.SerializeObject(orderItem)}'");
            OrderItems createOrderItem = await _orderItemRepository.CreateAsync(orderItem);

            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.CREATE_OK,
                createOrderItem
            ));
        }

        [Authorize(Roles = "1")]
        [HttpDelete("/api/order-items/delete/{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            OrderItems orderItem = await _orderItemRepository.DeleteAsync(id);
           
            if (orderItem == null) 
            {
                _logger.LogInformation("[DELETE: /api/order-items/delete/{id}] not exist orderItem ( id : " + id + ")");
                return NotFound(ResponseBuilder.FailedResponse(
                    ResponseBuilder.NOT_FOUND,
                    new string[] { "Order item not found" },
                    StatusCodes.Status404NotFound
                ));
            }
            
            _logger.LogInformation("[DELETE: /api/order-items/delete/{id}] Deleting OrderItem ( id : "+id+")");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.DELETE_OK,
                orderItem
            ));

        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/order-items/delete/{id}")]
        public async Task<IActionResult> SoftDeleteOrderItem(int id)
        {
            OrderItems orderItem = await _orderItemRepository.SoftDeleteAsync(id);
           
            if (orderItem == null) 
            {
                _logger.LogInformation("[PATCH: /api/order-items/delete/{id}] not exist OrderItem ( id : " + id + ")");
                return NotFound(ResponseBuilder.FailedResponse(
                    ResponseBuilder.NOT_FOUND,
                    new string[] { "Order item not found" },
                    StatusCodes.Status404NotFound
                ));
            }
            
            _logger.LogInformation("[PATCH: /api/order-items/delete/{id}] Deleting OrderItem ( id : "+id+")");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.DELETE_OK,
                orderItem
            ));
        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/order-items/{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, OrderItems orderItem)
        {
            OrderItems updateOrderItem = await _orderItemRepository.UpdateAsync(id, orderItem);
             
            if (updateOrderItem == null) 
            {
                _logger.LogInformation("[PATCH: /api/order-items/{id}] not exist OrderItem ( id : " + id + ")");
                return NotFound(ResponseBuilder.FailedResponse(
                    ResponseBuilder.NOT_FOUND,
                    new string[] { "Order item not found" },
                    StatusCodes.Status404NotFound
                ));
            }
            
            _logger.LogInformation("[PATCH: /api/order-items/{id}] Updating OrderItem ( id : "+ id +")");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.UPDATE_OK,
                updateOrderItem
            ));
        }
    }
}