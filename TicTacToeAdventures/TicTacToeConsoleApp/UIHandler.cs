using System;
using System.Linq;

namespace TicTacToeConsoleApp
{
    //Taking better name suggestions :P
    //TODO: Consider using an Interface. WebApp may need similar functionality. ConsoleUIHandler : UIHandler
    public class UIHandler
    {
        public int VerifyUserInput()
        {
            var input = Console.ReadLine();
            if (IsInputValid(input))
            {
                return int.Parse(input);
            }

            Console.WriteLine("This input is incorrect. Enter a number that's 0-8.");
            return VerifyUserInput();
        }

        public bool IsInputValid(string input)
        {
            int[] validMoves = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            return int.TryParse(input, out int userMove) && validMoves.Contains(userMove);
        }

        public string DrawBoard(char[] gameBoard)
        {
            return "\n     |     |      \n"
                + $"  {gameBoard[0]}  |  {gameBoard[1]}  |  {gameBoard[2]}\n"
                
                + "_____|_____|_____ \n"
                
                + "     |     |      \n"
                
                + $"  {gameBoard[3]}  |  {gameBoard[4]}  |  {gameBoard[5]}\n"
                
                + "_____|_____|_____ \n"
                
                + "     |     |      \n"
                
                + $"  {gameBoard[6]}  |  {gameBoard[7]}  |  {gameBoard[8]}\n"
                
                + "     |     |      \n";
        }
    }
}
