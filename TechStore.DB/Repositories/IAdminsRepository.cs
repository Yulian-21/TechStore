using TechStore.Domain.Models.Users;

namespace TechStore.DB.Repositories
{
    public interface IAdminsRepository: IDataRepository<Admin>
    {
        bool AdminExistsByEmail(string email);

        Admin GetAdminByEmail(string email);
    }
}
