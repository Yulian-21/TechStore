using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TechStore.Business.Exceptions;
using TechStore.Controllers.Models.Orders;
using TechStore.Domain.Models.Orders;
using TechStore.Domain.Services;
using TechStore.Domain.Types;

namespace TechStore.Controllers.Controllers;

[ApiController]
[Route("orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMapper _mapper;
        
    private readonly IOrdersService _ordersService;
    private readonly IOrderItemsService _orderItemsService;
    private readonly IOrderReviewsService _orderReviewsService;

    public OrdersController(
        IMapper mapper,
        IOrdersService ordersService,
        IOrderItemsService orderItemsService,
        IOrderReviewsService orderReviewsService)
    {
        ArgumentNullException.ThrowIfNull(mapper);
        _mapper = mapper;
            
        ArgumentNullException.ThrowIfNull(ordersService);
        _ordersService = ordersService;
            
        ArgumentNullException.ThrowIfNull(orderItemsService);
        _orderItemsService = orderItemsService;
            
        ArgumentNullException.ThrowIfNull(orderReviewsService);
        _orderReviewsService = orderReviewsService;
    }

    [HttpGet("{orderId}")]
    public ActionResult<ApiOrder> GetOrderById(int orderId)
    {
        try
        {
            var model = _ordersService.GetOrderById(orderId);
            return Ok(_mapper.Map<ApiOrder>(model));
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpGet("all")]
    [Authorize(Roles = UserRole.Admin)]
    public ActionResult<IEnumerable<ApiOrder>> GetAllOrders()
    {
        try
        {
            var models = _ordersService.GetAllOrders();
            return Ok(_mapper.Map<IEnumerable<ApiOrder>>(models));
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpGet("clients/{clientId}")]
    public ActionResult<List<ApiOrder>> GetClientOrders(int clientId)
    {
        try
        {
            var models = _ordersService.GetClientOrders(clientId);
            return models.Select(x => _mapper.Map<ApiOrder>(x)).ToList();
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpPost("create")]
    public ActionResult<ApiOrder> CreateOrder()
    {
        try
        {
            var identity = HttpContext.User.Identity;
            if (identity == null) return BadRequest("Unknown current user.");
            
            var model = _ordersService.CreateOrder(identity.Name);
            return _mapper.Map<ApiOrder>(model);
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpPut("{orderId}/completed")]
    [Authorize(Roles = UserRole.Admin)]
    public ActionResult<ApiOrder> UpdateOrderAsCompleted(int orderId)
    {
        try
        {
            var model = _ordersService.UpdateOrderAsCompleted(orderId);
            return _mapper.Map<ApiOrder>(model);
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpDelete("{orderId}")]
    public ActionResult DeleteOrder(int orderId)
    {
        try
        {
            _ordersService.DeleteOrder(orderId);
            return NoContent();
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpPost("{orderId}/items")]
    public ActionResult<ApiOrderItem> CreateOrderItem(int orderId, ApiOrderItemCreateRequest request)
    {
        try
        {
            var model = _orderItemsService.CreateOrderItem(new OrderItem
            {
                OrderId = orderId,

                ProductId = request.ProductId,
                Qty = request.Qty,

                Comment = request.Comment
            });

            return _mapper.Map<ApiOrderItem>(model);
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
        catch (InvalidModelException exception)
        {
            return BadRequest(exception.Errors);
        }
    }
        
    [HttpPut("{orderId}/items/{itemId}")]
    public ActionResult<ApiOrderItem> UpdateOrderItem(int orderId, int itemId, ApiOrderItemUpdateRequest request)
    {
        try
        {
            var model = _orderItemsService.UpdateOrderItem(new OrderItem
            {
                Id = itemId,
                OrderId = orderId,

                ProductId = request.ProductId,
                Qty = request.Qty,

                Comment = request.Comment
            });

            return _mapper.Map<ApiOrderItem>(model);
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
        catch (InvalidModelException exception)
        {
            return BadRequest(exception.Errors);
        }
    }

    [HttpDelete("{orderId}/items/{itemId}")]
    public ActionResult DeleteOrderItem(int orderId, int itemId)
    {
        try
        {
            _orderItemsService.DeleteOrderItem(orderId, itemId);
            return NoContent();
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpPost("{orderId}/review")]
    [Authorize(Roles = UserRole.Client)]
    public ActionResult<ApiOrderReview> CreateOrderReview(int orderId, ApiOrderReview review)
    {
        try
        {
            var model = _mapper.Map<OrderReview>(review);
            model.OrderId = orderId;
                
            model = _orderReviewsService.CreateOrderReview(model);
            return _mapper.Map<ApiOrderReview>(model);
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
        catch (InvalidModelException exception)
        {
            return BadRequest(exception.Errors);
        }
    }
        
    [HttpDelete("{orderId}/review")]
    [Authorize(Roles = UserRole.Client)]
    public IActionResult DeleteOrderReview(int orderId)
    {
        try
        {
            _orderReviewsService.DeleteOrderReview(orderId);
            return NoContent();
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }
}