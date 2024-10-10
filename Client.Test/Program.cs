
using Client.Start;

// Present the debug window
DebugWindow dbw = new DebugWindow();
dbw.Show();

// Then run the game
using (Game game = new Game())
{
    game.Run();
}
