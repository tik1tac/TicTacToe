using Tic_Tac_Toe.Model;

namespace TicTacToe.Tests
{
    public class Tests
    {
        private readonly GameConfig gameConfig = new GameConfig { BoardSize = 5, WinLength = 4 };
        [Fact]
        public void IsDraw_ReturnsFalse_WhenBoardIsNotFull()
        {
            var game = new Game(gameConfig.BoardSize);
            game.Board[0, 0] = "X";

            var result = IsFilled(game);

            Assert.False(result);
        }

        [Fact]
        public void IsDraw_ReturnsTrue_WhenBoardIsFull()
        {
            var game = new Game(3);
            var moves = new[] { "X", "O" };
            int index = 0;

            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    game.Board[r, c] = moves[index % 2];
                    index++;
                }
            }

            var result = IsFilled(game);

            Assert.True(result);
        }

        [Fact]
        public void CheckWin_ReturnsTrue_WhenHorizontalWin()
        {
            var game = new Game(gameConfig.BoardSize);
            string player = "X";
            game.CurrentPlayer = player;

            for (int i = 0; i < 4; i++)
                game.Board[0, i] = player;

            var result = CheckWin(game);

            Assert.True(result);
        }

        [Fact]
        public void CheckWin_ReturnsTrue_WhenVerticalWin()
        {
            var game = new Game(gameConfig.BoardSize);
            string player = "O";
            game.CurrentPlayer = player;

            for (int i = 0; i < 4; i++)
                game.Board[i, 1] = player;

            var result = CheckWin(game);

            Assert.True(result);
        }

        [Fact]
        public void CheckWin_ReturnsTrue_WhenDiagonalWin()
        {
            var game = new Game(gameConfig.BoardSize);
            string player = "X";
            game.CurrentPlayer = player;

            for (int i = 0; i < 4; i++)
                game.Board[i, i] = player;

            var result = CheckWin(game);

            Assert.True(result);
        }

        [Fact]
        public void CheckWin_ReturnsTrue_WhenAntiDiagonalWin()
        {
            var game = new Game(gameConfig.BoardSize);
            string player = "O";
            game.CurrentPlayer = player;

            for (int i = 0; i < 4; i++)
                game.Board[i, 3 - i] = player;

            var result = CheckWin(game);

            Assert.True(result);
        }

        [Fact]
        public void CheckWin_ReturnsFalse_WhenNoWin()
        {
            var game = new Game(gameConfig.BoardSize);
            string player = "X"; 
            game.CurrentPlayer = player;

            game.Board[0, 0] = player;
            game.Board[0, 1] = "O";
            game.Board[0, 2] = player;

            var result = CheckWin(game);

            Assert.False(result);
        }
        private bool IsFilled(Game game) => (game.Board.Cast<string>().All(c => !string.IsNullOrEmpty(c)));

        private bool CheckWin(Game game)
        {
            var size = gameConfig.BoardSize;
            var winLen = gameConfig.WinLength;
            var player = game.CurrentPlayer;

            for (int bz = 0; bz < size; bz++)
            {
                for (int wl = 0; wl < size; wl++)
                {
                    if (CheckDir(bz, wl, 1, 0) || CheckDir(bz, wl, 0, 1) || CheckDir(bz, wl, 1, 1) || CheckDir(bz, wl, 1, -1))
                        return true;
                }
            }
            return false;

            bool CheckDir(int bz, int wl, int val1, int val2)
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
