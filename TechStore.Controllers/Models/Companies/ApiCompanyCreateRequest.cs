using System.ComponentModel.DataAnnotations;

namespace TechStore.Controllers.Models.Companies;

public class ApiCompanyCreateRequest
{
    [Required]
    public string Name { get; set; }
}