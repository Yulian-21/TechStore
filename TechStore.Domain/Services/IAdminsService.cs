using TechStore.Domain.Models.Users;

namespace TechStore.Domain.Services;

public interface IAdminsService
{
    Admin RegisterAdmin(Admin admin);

    Admin GetAdminByEmail(string email);

    IEnumerable<Admin> GetAll();

    void DeleteById(int id);
}