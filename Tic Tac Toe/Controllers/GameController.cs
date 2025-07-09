using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Tic_Tac_Toe.IRepository;
using Tic_Tac_Toe.Model;

namespace Tic_Tac_Toe.Controllers
{
    [Route("game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IRepositoryGame rep;
        private readonly GameConfig gameConfig;
        public GameController(IRepositoryGame _rep, GameConfig _gameconfig) => (rep, gameConfig) = (_rep, _gameconfig);

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame(Guid id)
        {
            try
            {
                var game = await rep.GetGameByIdAsync(id);

                if (game == null)
                    return NotFound("Game Not Found");
                return Ok(game);
            }
            catch (Exception)
            {
                return BadRequest("Fail!");
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGame()
        {
            try
            {
                var game = new Game(gameConfig.BoardSize);
                await rep.SaveAsync(game);
                return Ok(game);
            }
            catch (Exception)
            {
                return BadRequest("Fail!");
            }
        }

        [HttpPost("move/{id}")]
        public async Task<IActionResult> Move(Guid id, [FromBody] Move move)
        {
            var semaphore = new SemaphoreSlim(1); //смысла конечно особого нет
            await semaphore.WaitAsync();
            try
            {
                var game = await rep.GetGameByIdAsync(id);//Получаем игру

                if (game is null || game.StatusGame != Share.Status.InProgress) //Проверяем есть ли игра и не закончена ли она
                    return BadRequest("Invalid Game");

                if (!string.IsNullOrEmpty(game.Board[move.Row, move.Column]))//Проверяем есть ли символ в данной ячейке
                    return Ok("Клетка уже занята");

                game.MoveCount++;

                if (game.MoveCount % 3 == 0 && new Random().Next(100) < 10) //Условие поставленное в задании
                    game.Board[move.Row, move.Column] = "*";
                else
                    game.Board[move.Row, move.Column] = game.CurrentPlayer; // Проставляем конкретный символ в ячейку

                if (await CheckWin(game))//Проверяем победу
                    game.StatusGame = Share.Status.Finished;
                else if (await IsFilled(game)) //Проверяем можно ли продолжать игру
                    game.StatusGame = Share.Status.Finished;
                else game.CurrentPlayer = game.CurrentPlayer is "X" ? "O" : "X";

                await rep.SaveAsync(game);//Сохраняем игру

                Response.Headers["ETag"] = game.Id.ToString();
                return Ok(game);
            }
            catch (Exception)
            {
                return BadRequest("Fail!");
            }
            finally
            {
                semaphore.Release();
            }

        }

        private async Task<bool> IsFilled(Game game) => await Task.FromResult(game.Board.Cast<string>().All(c => !string.IsNullOrEmpty(c)));

        private async Task<bool> CheckWin(Game game)
        {
            var size = gameConfig.BoardSize;
            var winLen = gameConfig.WinLength;
            var player = game.CurrentPlayer;

            for (int bz = 0; bz < size; bz++)
            {
                for (int wl = 0; wl < size; wl++)
                {
                    if (await CheckDir(bz, wl, 1, 0) || await CheckDir(bz, wl, 0, 1) || await CheckDir(bz, wl, 1, 1) || await CheckDir(bz, wl, 1, -1))
                        return true;
                }
            }
            return false;

            async Task<bool> CheckDir(int bz, int wl, int val1, int val2)
            {

                for (int i = 0; i < winLen; i++)
                {
                    int nr = bz + val1 * i, nc = wl + val2 * i;
                    if (nr < 0 || nr >= size || nc < 0 || nc >= size || game.Board[nr, nc] != player)
                        return false;
                }
                return true;
            }
        }
    }
}
