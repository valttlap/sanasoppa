namespace Sanasoppa.API.Interfaces
{
    public interface IUnitOfWork
    {
       IGameRepository GameRepository {get;}
       IPlayerRepository PlayerRepository { get;}
       IRoundRepository RoundRepository { get; }
       Task<bool> Complete();
       bool HasChanges();
    }

}