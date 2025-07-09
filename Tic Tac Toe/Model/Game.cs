using Tic_Tac_Toe.Share;

namespace Tic_Tac_Toe.Model;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string[,]? Board {  get; set; }
    public Status StatusGame { get; set; } = Status.InProgress;
    public int MoveCount { get; set; } = 0;
    public string CurrentPlayer { get; set; } = "X";

    public Game(int size)
    {
        Board = new string[size, size];
    }
}
