using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TechStore.Business.Exceptions;
using TechStore.Controllers.Models.Companies;
using TechStore.Domain.Models.Products;
using TechStore.Domain.Services;
using TechStore.Domain.Types;

namespace TechStore.Controllers.Controllers;

[ApiController]
[Route("companies")]
[Authorize]
public class CompaniesController : ControllerBase
{
    private readonly ICompaniesService _companiesService;
    private readonly IMapper _mapper;
    
    public CompaniesController(
        ICompaniesService companiesService,
        IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(companiesService);
        _companiesService = companiesService;
        
        ArgumentNullException.ThrowIfNull(mapper);
        _mapper = mapper;
    }
    
    [HttpGet("all")]
    public ActionResult<ApiCompany> GetAll()
    {
        try
        {
            var companies = _companiesService.GetAllCompanies();
            return Ok(_mapper.Map<IEnumerable<ApiCompany>>(companies));
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpGet("{companyId}")]
    public ActionResult<ApiCompany> Get(int companyId)
    {
        try
        {
            var company = _companiesService.GetCompanyById(companyId);
            return Ok(_mapper.Map<ApiCompany>(company));
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles = UserRole.Admin)]
    public ActionResult<ApiCompany> Post(ApiCompanyCreateRequest request)
    {
        try
        {
            var company = new Company { Name = request.Name };
            company = _companiesService.CreateCompany(company);
            
            return Ok(_mapper.Map<ApiCompany>(company));
        }
        catch (InvalidModelException exception)
        {
            return BadRequest(exception.Errors);
        }
    }

    [HttpPut("{companyId}")]
    [Authorize(Roles = UserRole.Admin)]
    public ActionResult<ApiCompany> Put(int companyId, ApiCompanyUpdateRequest request)
    {
        try
        {
            var company = new Company
            {
                Id = companyId,
                Name = request.Name
            };
            
            company = _companiesService.UpdateCompany(company);
            return Ok(_mapper.Map<ApiCompany>(company));
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

    [HttpDelete("{companyId}")]
    [Authorize(Roles = UserRole.Admin)]
    public IActionResult Delete(int companyId)
    {
        try
        {
            _companiesService.DeleteCompany(companyId);
            return NoContent();
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }
}