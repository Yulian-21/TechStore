using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TechStore.Business.Exceptions;
using TechStore.Business.Services;
using TechStore.Controllers.Models.Users;
using TechStore.Domain.Models.Users;
using TechStore.Domain.Services;
using TechStore.Domain.Types;

namespace TechStore.Controllers.Controllers
{
    [Route("admins")]
    [ApiController]
    [Authorize]
    public class AdminsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAdminsService _adminsService;

        public AdminsController(
            IMapper mapper,
            IAdminsService adminsService)
        {
            ArgumentNullException.ThrowIfNull(mapper);
            _mapper = mapper;
            
            ArgumentNullException.ThrowIfNull(adminsService);
            _adminsService = adminsService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult<object> Register(ApiAdminRegisterRequest request)
        {
            try
            {
                var admin = _adminsService.RegisterAdmin(new Admin
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,

                    Email = request.Email,
                    Password = request.Password,

                    UserName = request.UserName
                });

                return Ok(_mapper.Map<ApiAdmin>(admin));
            }
            catch (InvalidModelException exception)
            {
                return BadRequest(exception.Errors);
            }
        }

        [HttpPost("{adminMail}")]
        public ActionResult<ApiAdmin> Get(string adminMail)
        {
            try
            {
                var admin = _adminsService.GetAdminByEmail(adminMail);
                return _mapper.Map<ApiAdmin>(admin);
            }
            catch (UnknownModelException exception)
            {
                return NotFound(exception.Message);
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = UserRole.Admin)]
        public ActionResult<IEnumerable<ApiAdmin>> GetAll()
        {
            var allAdmins = new List<ApiAdmin>();
            try
            {
                var admins = _adminsService.GetAll();
                foreach (var admin in admins)
                {
                    allAdmins.Add(_mapper.Map<ApiAdmin>(admin));
                }
                return allAdmins;
            }
            catch (UnknownModelException exception)
            {
                return NotFound(exception.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public ActionResult<ApiAdmin> DeleteById(int id)
        {
            try
            {
                _adminsService.DeleteById(id);
                return Ok("Admin was deleted succesfully");
            }
            catch (UnknownModelException exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}
