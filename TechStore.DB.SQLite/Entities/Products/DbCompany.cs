using System.Collections.Generic;

namespace TechStore.DB.SQLite.Entities.Products;

public class DbCompany
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public IList<DbProduct> Products { get; set; }
}
