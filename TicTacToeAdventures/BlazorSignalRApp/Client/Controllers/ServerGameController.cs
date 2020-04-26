using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSignalRApp.Client.Controllers
{
    //Responsible for talking to our GameHub
    //Seperation of UI and Talking to Server
    public class ServerGameController
    {
        private HubConnection _hubConnection;
        public char ClientPlayer { get; set; }
        public string StatusMessage { get; set; }
        public OpponentPlayer Opponent { get; set; }

        public async Task StartGame()
        {
            _hubConnection = new HubConnectionBuilder()
           .WithUrl("https://localhost:44330/gamehub")
           .Build();

            _hubConnection.On<string>("ClientLog", ClientLog);
            _hubConnection.On<char>("SetPlayer", SetPlayer);
            _hubConnection.On<int, char>("ReceiveOpponentMove", ReceiveOpponentMove);
            _hubConnection.On<string>("ReceiveEndGameUpdate", ReceiveEndGameUpdate);

            await _hubConnection.StartAsync();
        }

        private void ClientLog(string msg)
        {
            Console.WriteLine(msg.ToString());
        }

        private void SetPlayer(char player)
        {
            ClientPlayer = player;
            Console.WriteLine("Player Game Piece: " + ClientPlayer);
        }

        private void ReceiveEndGameUpdate(string update)
        {
            StatusMessage = update;
        }

        private void ReceiveOpponentMove(int move, char opponent)
        {
            Opponent.Move = move;
            Opponent.Piece = opponent;
        }

        public async Task SendMoveToOpponent(int move, char player) =>
            await _hubConnection.SendAsync("SendMove", move, player);

        public async Task SendEndGameUpdate(string update) =>
            await _hubConnection.SendAsync("SendEndGameUpdate", update);

        public bool IsConnected =>
            _hubConnection.State == HubConnectionState.Connected;

        public class OpponentPlayer
        {
            public char Piece { get; set; }
            public int Move { get; set; }
        }
    }
}
