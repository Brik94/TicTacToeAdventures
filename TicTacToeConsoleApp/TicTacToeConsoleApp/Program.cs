using System;
using System.Linq;

namespace TicTacToeConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Tic-Tac-Toe!\n");
            Console.WriteLine("Enter 'Y' to start. \n");
            var choice = Console.ReadLine().ToUpper(); ;

            while (choice == "Y")
            {
                Console.WriteLine("\nPlayer 1 is X. Player 2 is O.");
                Console.WriteLine("To make a move, enter a number 0-8.");

                StartGame();

                Console.WriteLine("\n Enter 'Y' to Play again.");
                choice = Console.ReadLine().ToUpper();
            }
        }

        static void StartGame()
        {
            var gameEngine = new GameEngine();
            var uiHandler = new UIHandler();

            char player1 = 'X';
            char player2 = 'O';
            char currentPlayer = '\0';
            bool isWinFound = false;
            bool isTimeGame = false;

            Console.WriteLine(uiHandler.DrawBoard(gameEngine.GameBoard));

            while (!(isWinFound || isTimeGame))
            {
                currentPlayer = currentPlayer == player2 || currentPlayer == '\0' ? player1 : player2;
                Console.WriteLine($" {currentPlayer}, your turn!");


                int move = uiHandler.VerifyUserInput();
                bool moveSucceeded = gameEngine.TryPlayerMove(move, currentPlayer);
                while (!moveSucceeded)
                {
                    Console.WriteLine("Someone has already moved here! Try again.");
                    move = uiHandler.VerifyUserInput();
                    moveSucceeded = gameEngine.TryPlayerMove(move, currentPlayer);
                }

                Console.WriteLine(uiHandler.DrawBoard(gameEngine.GameBoard));

                isWinFound = gameEngine.CheckForWin();
                isTimeGame = gameEngine.CheckForTie();
            }

            if (isWinFound) Console.WriteLine($"{currentPlayer} Wins!");
            else Console.WriteLine("There's a tie. GAME OVER.");
        }
    }
}