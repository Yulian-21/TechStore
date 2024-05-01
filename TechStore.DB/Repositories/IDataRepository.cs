using System.Collections.Generic;

using TechStore.Domain.Models;

namespace TechStore.DB.Repositories;

public interface IDataRepository<T>
{
    IEnumerable<T> GetAll();
    
    T Get(int id);
    
    T Insert(T model);
    
    T Update(int id, T model);
    
    void Delete(int id);
}
