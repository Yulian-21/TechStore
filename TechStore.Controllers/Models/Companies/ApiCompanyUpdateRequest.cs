using System.ComponentModel.DataAnnotations;

namespace TechStore.Controllers.Models.Companies;

public class ApiCompanyUpdateRequest
{
    [Required]
    public string Name { get; set; }
}