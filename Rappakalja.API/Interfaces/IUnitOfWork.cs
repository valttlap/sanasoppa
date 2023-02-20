namespace Rappakalja.API.Interfaces
{
    public interface IUnitOfWork
    {
       IGameRepository GameRepository {get;}
       Task<bool> Complete();
       bool HasChanges();
    }

}