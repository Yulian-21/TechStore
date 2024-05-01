namespace TechStore.Domain.Types;

public class UserRole
{
    public const string Client = "Client";
    public const string Admin = "Admin";
    public static readonly string[] Roles = new[] { Client, Admin };
}
