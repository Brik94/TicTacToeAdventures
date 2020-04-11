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
            Console.WriteLine(gameEngine.DrawBoard() + "\n");


            var player1 = 'X';
            var player2 = 'O';
            char currentPlayer = '\0';

            while (!gameEngine.CheckForWin(currentPlayer) || !gameEngine.CheckForTie())
            {
                currentPlayer = currentPlayer == player2 || currentPlayer == '\0' ? player1 : player2;
                Console.WriteLine($" {currentPlayer}, your turn!");


                int move = VerifyUserInput();
                bool moveSucceeded = gameEngine.TryPlayerMove(move, currentPlayer);
                while (!moveSucceeded)
                {
                    Console.WriteLine("Someone has already moved here! Try again.");
                    move = VerifyUserInput();
                    moveSucceeded = gameEngine.TryPlayerMove(move, currentPlayer);
                }


                Console.WriteLine();
                Console.WriteLine(gameEngine.DrawBoard() + "\n");
            }
        }

        //This new method seperates business logic from UI logic (the WriteLine).
        //Also makes this easily testable.
        private static bool IsInputValid(string input)
        {
            int[] validMoves = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            return int.TryParse(input, out int userMove) && validMoves.Contains(userMove);
        }

        static int VerifyUserInput()
        {
            var input = Console.ReadLine();
            if (IsInputValid(input))
            {
                return int.Parse(input);
            }

            Console.WriteLine("This input is incorrect. Enter a number that's 0-8.");
            return VerifyUserInput();
        }
    }
}