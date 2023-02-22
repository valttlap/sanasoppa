namespace Sanasoppa.API.Interfaces
{
    public interface IUnitOfWork
    {
       IGameRepository GameRepository {get;}
       IPlayerRepository PlayerRepository { get;}
       Task<bool> Complete();
       bool HasChanges();
    }

}