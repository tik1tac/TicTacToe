using Newtonsoft.Json;

using Tic_Tac_Toe.IRepository;
using Tic_Tac_Toe.Model;

namespace Tic_Tac_Toe.Repository;

public class RepositoryGame : IRepositoryGame
{
    public RepositoryGame() => Directory.CreateDirectory("./games");
    public async Task<Game> GetGameByIdAsync(Guid id) => await Task.FromResult(JsonConvert.DeserializeObject<Game>(File.ReadAllText(Path.Combine("./games", id + ".json"))));

    public async Task SaveAsync(Game game) => await File.WriteAllTextAsync(Path.Combine("./games", game.Id + ".json"), JsonConvert.SerializeObject(game));
}
