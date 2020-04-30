namespace BlazorSignalRApp.Server.Hubs
{
    public class Game
    {
        public string ID { get; set; }
        public Player P1 { get; set; }
        public Player P2 { get; set; }
        //GameStatus-> Starting, InProgress, Ending: (Win or Tie)}
    }
}
