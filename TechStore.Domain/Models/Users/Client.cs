using TechStore.Domain.Models.Orders;

namespace TechStore.Domain.Models.Users;

public class Client : UserDTO
{
    public IEnumerable<Order> Orders { get; set; }
    
    public IEnumerable<ShippingAddress> Addresses { get; set; }
}
