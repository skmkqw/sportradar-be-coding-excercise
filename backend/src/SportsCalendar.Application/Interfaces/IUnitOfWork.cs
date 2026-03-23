using System.Data;

namespace SportsCalendar.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDbConnection Connection { get; }
    
    IDbTransaction? Transaction { get; }
    
    void Begin();
    
    void Commit();
    
    void Rollback();
}