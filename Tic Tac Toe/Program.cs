using System.Text.Json.Serialization;

using Tic_Tac_Toe.IRepository;
using Tic_Tac_Toe.Model;
using Tic_Tac_Toe.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
var boardSize = 5;/*int.TryParse(Environment.GetEnvironmentVariable("TICTACTOE_BOARD_SIZE"), out var size) ? size : 3;*/
var winLength = 4;/*int.TryParse(Environment.GetEnvironmentVariable("TICTACTOE_WIN_LENGTH"), out var win) ? win : 3;*/
builder.Services.AddSingleton(new GameConfig { BoardSize = boardSize, WinLength = winLength });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IRepositoryGame, RepositoryGame>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok());
app.MapControllers();

app.Run();