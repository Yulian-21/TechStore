using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TechStore.Business.Exceptions;
using TechStore.Controllers.Models.Users;
using TechStore.Domain.Services.Authentication;

namespace TechStore.Controllers.Controllers
{
    [Route("authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        
        public AuthenticationController(
            IAuthenticationService authenticationService,
            IMapper mapper)
        {
            ArgumentNullException.ThrowIfNull(authenticationService);
            _authenticationService = authenticationService;
            
            ArgumentNullException.ThrowIfNull(mapper);
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("login/client")]
        public ActionResult<ApiAuthenticationResult> ClientLogin(ApiAuthenticationRequest request)
        {
            try
            {
                var result = _authenticationService.ClientLogin(request.Username, request.Password);
                return Ok(_mapper.Map<ApiAuthenticationResult>(result));
            }
            catch (AuthenticationFailedException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login/admin")]
        public ActionResult<ApiAuthenticationResult> AdminLogin(ApiAuthenticationRequest request)
        {
            try
            {
                var result = _authenticationService.AdminLogin(request.Username, request.Password);
                return Ok(_mapper.Map<ApiAuthenticationResult>(result));
            }
            catch (AuthenticationFailedException exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
