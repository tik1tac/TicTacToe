using Tic_Tac_Toe.Model;

namespace Tic_Tac_Toe.IRepository;

public interface IRepositoryGame
{
    Task<Game> GetGameByIdAsync(Guid id);
    Task SaveAsync(Game game);
}
