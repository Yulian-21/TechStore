using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TechStore.Business.Exceptions;
using TechStore.Business.Services;
using TechStore.Controllers.Models.Orders;
using TechStore.Controllers.Models.Users;
using TechStore.Domain.Models.Users;
using TechStore.Domain.Services;
using TechStore.Domain.Types;

namespace TechStore.Controllers.Controllers
{
    [Route("clients")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IClientService _clientsService;

        public ClientsController(
            IMapper mapper,
            IClientService clientsService)
        {
            ArgumentNullException.ThrowIfNull(mapper);
            _mapper = mapper;
            
            ArgumentNullException.ThrowIfNull(clientsService);
            _clientsService = clientsService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult<object> Register(ApiClientRegisterRequest request)
        {
            try
            {
                var client = _clientsService.RegisterClient(new Client
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,

                    Email = request.Email,
                    Password = request.Password
                });

                return Ok(_mapper.Map<ApiClient>(client));
            }
            catch (InvalidModelException exception)
            {
                return BadRequest(exception.Errors);
            }
        }

        [HttpGet("id/{clientId}")]
        [Authorize(Roles = UserRole.Admin)]
        public ActionResult<ApiClient> GetById(int clientId)
        {
            try
            {
                var client = _clientsService.GetClient(clientId);
                return _mapper.Map<ApiClient>(client);
            }
            catch (UnknownModelException exception)
            {
                return NotFound(exception.Message);
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = UserRole.Admin)]
        public ActionResult<List<ApiClient>> GetAll()
        {
            var clientsList = new List<ApiClient>();
            try
            {
                var clients = _clientsService.GetAllClients();
                foreach (var client in clients)
                {
                    var currentClient = _mapper.Map<ApiClient>(client);
                    clientsList.Add(currentClient);
                }
                return clientsList;
            }
            catch (UnknownModelException exception)
            {
                return NotFound(exception.Message);
            }
        }

        [HttpDelete("delete")]
        [Authorize(Roles = UserRole.Client)]
        public ActionResult DeleteClient()
        {
            try
            {
                var identity = HttpContext.User.Identity;
                if (identity == null) return BadRequest("Unknown current user.");

                _clientsService.DeleteClient(identity.Name);
                return Ok("Client was deleted succesfully");
            }
            catch (UnknownModelException exception)
            {
                return NotFound(exception.Message);
            }
        }

        [HttpPost("mail/{clientEmail}")]
        public ActionResult<ApiClient> GetByEmail(string clientEmail)
        {
            try
            {
                var client = _clientsService.GetClientByEmail(clientEmail);
                return _mapper.Map<ApiClient>(client);
            }
            catch (UnknownModelException exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}
