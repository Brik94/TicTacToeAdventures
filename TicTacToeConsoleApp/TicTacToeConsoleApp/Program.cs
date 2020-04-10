using System;
using System.Linq;

namespace TicTacToeConsoleApp
{
    /***
     * TODO
     * 1. Finish winning combos
     * 2. Determine a Tie
     * 3. User Input error checking
     * 4. 
     */
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Tic-Tac-Toe!\n");
            Console.WriteLine("Player 1 is X. Player 2 is O.");
            Console.WriteLine("To make a move, enter a number 0-8.");


            var gameEngine = new GameEngine();
            Console.WriteLine(gameEngine.DrawBoard());


            var player1 = 'X';
            var player2 = 'O';


            while (true)//We don't quit, or someone didn't win/lose/cat
            {
                Console.WriteLine("Player 1, your turn");
                int move = VerifyUserInput();

                bool moveSucceeded = gameEngine.TryPlayerMove(move, player1);
                while (!moveSucceeded)
                {
                    Console.WriteLine("Someone has already moved here! Try again.");
                    move = Convert.ToInt32(Console.ReadLine());
                    moveSucceeded = gameEngine.TryPlayerMove(move, player1);
                }
                Console.WriteLine(gameEngine.DrawBoard());

                //TODO: Check for Tie. Check for Cat.
                if (gameEngine.CheckForWin(player1))
                {
                    break;
                }



                Console.WriteLine("Player 2, your turn");
                move = VerifyUserInput();

                moveSucceeded = gameEngine.TryPlayerMove(move, player2);
                while (!moveSucceeded)
                {
                    Console.WriteLine("Someone has already moved here! Try again.");
                    move = Convert.ToInt32(Console.ReadLine()); //Sanitize
                    moveSucceeded = gameEngine.TryPlayerMove(move, player2);
                }
                Console.WriteLine(gameEngine.DrawBoard());

                //TODO: Check for Tie. Check for Cat.
                if (gameEngine.CheckForWin(player2))
                {
                    break;
                }
            }

        }

        static int VerifyUserInput()
        {
            int userMove = -1;
            int[] validMoves = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            bool validInput = false;

            while (!validInput)
            {
                try
                {
                    userMove = Convert.ToInt32(Console.ReadLine());
                    if (validMoves.Contains(userMove))
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("This input is incorrect. Enter numbers 0 - 8");
                    }
                }
                catch
                {
                    Console.WriteLine("This input is incorrect. Enter numbers 0 - 8");
                }
            }


            return userMove;
        }
    }
}