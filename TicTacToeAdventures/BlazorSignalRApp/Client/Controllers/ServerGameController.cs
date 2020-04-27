using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace BlazorSignalRApp.Client.Controllers
{
    //Responsible for talking to our GameHub. Seperation of UI and Talking to Server
    public class ServerGameController
    {
        private HubConnection _hubConnection;
        public ClientState State { get; set; }

        public event EventHandler StartGame;
        public event EventHandler OpponentUpdatedEvent;
        public event EventHandler UIUpdateEvent;

        public async Task Initialize()
        {
            State = new ClientState();

            _hubConnection = new HubConnectionBuilder()
           .WithUrl("https://localhost:44330/gamehub")
           .Build();

            _hubConnection.On<string>("ClientLog", ClientLog);
            _hubConnection.On<char, bool>("SetPlayer", SetPlayer);
            _hubConnection.On<int, char>("ReceiveOpponentMove", ReceiveOpponentMove);
            _hubConnection.On<string>("ReceiveEndGameUpdate", ReceiveEndGameUpdate);

            await _hubConnection.StartAsync();
        }

        public async Task SendMoveToOpponent(int move, char player) =>
            await _hubConnection.SendAsync("SendMove", move, player);

        public async Task SendEndGameUpdate(string update) =>
            await _hubConnection.SendAsync("SendEndGameUpdate", update);

        public bool IsConnected =>
            _hubConnection.State == HubConnectionState.Connected;

        private void ClientLog(string msg)
        {
            Console.WriteLine(msg.ToString());
        }

        private void SetPlayer(char player, bool uiDisabled)
        {
            State.ClientPiece = player;
            State.UIDisabled = uiDisabled;
            Console.WriteLine("Player Game Piece: " + State.ClientPiece);

            StartGame?.Invoke(this, new EventArgs());
        }

        private void ReceiveEndGameUpdate(string update)
        {
            State.StatusMessage = update;
            UIUpdateEvent?.Invoke(this, new EventArgs());
        }

        private void ReceiveOpponentMove(int move, char opponent)
        {
            State.OpponentMove = move;
            State.OpponentPiece = opponent;
            OpponentUpdatedEvent?.Invoke(this, new EventArgs());
        }

        public class ClientState
        {
            public bool UIDisabled { get; set; } = true;
            public string StatusMessage { get; set; }
            public char ClientPiece { get; set; }
            public char OpponentPiece { get; set; }
            public int OpponentMove { get; set; }
        }
    }
}
