using System;
using System.Linq;

namespace TicTacToeConsoleApp
{
    /***
     * Consider a Player Class. How would this look?
     */
    public class GameEngine
    {
        private char[] _gameBoard;
        public char[] GameBoard { get { return _gameBoard; } }

        //TODO: revisit constructor.
        public GameEngine()
        {
            _gameBoard = InitalizeGameBoard();
        }

        private char[] InitalizeGameBoard()
        {
            var gameBoard = new char[9];
            gameBoard[0] = ' ';
            gameBoard[1] = ' ';
            gameBoard[2] = ' ';

            gameBoard[3] = ' ';
            gameBoard[4] = ' ';
            gameBoard[5] = ' ';

            gameBoard[6] = ' ';
            gameBoard[7] = ' ';
            gameBoard[8] = ' ';

            return gameBoard;
        }

        public bool TryPlayerMove(int move, char player)
        {
            bool validMove = IsMoveValid(move);
            if (validMove)
            {
                _gameBoard[move] = player;
                return true;
            }
            return false;
        }

        private bool IsMoveValid(int move)
        {
            if (_gameBoard[move] == 'X' || _gameBoard[move] == 'O')
            {
                return false;
            }
            return true;
        }

        //TODO: Test all combos.
        public bool CheckForWin(char player)
        {
            return (DidWinByRow(player) || DidWinByColumn(player) || DidWinByAcross(player));
        }

        private bool DidWinByRow(char player)
        {
            if (_gameBoard[0] == player && _gameBoard[1] == player && _gameBoard[2] == player)
            {
                Console.WriteLine($"{player} Wins!");
                return true;
            }
            else if (_gameBoard[3] == player && _gameBoard[4] == player && _gameBoard[5] == player)
            {
                Console.WriteLine($"{player} Wins!");
                return true;
            }
            else if (_gameBoard[6] == player && _gameBoard[7] == player && _gameBoard[8] == player)
            {
                Console.WriteLine($"{player} Wins!");
                return true;
            }

            return false;
        }

        private bool DidWinByColumn(char player)
        {
            if (_gameBoard[0] == player && _gameBoard[3] == player && _gameBoard[6] == player)
            {
                Console.WriteLine($"{player} Wins!");
                return true;
            }
            else if (_gameBoard[1] == player && _gameBoard[4] == player && _gameBoard[7] == player)
            {
                Console.WriteLine($"{player} Wins!");
                return true;
            }
            else if (_gameBoard[2] == player && _gameBoard[5] == player && _gameBoard[8] == player)
            {
                Console.WriteLine($"{player} Wins!");
                return true;
            }

            return false;
        }

        private bool DidWinByAcross(char player)
        {
            if (_gameBoard[0] == player && _gameBoard[4] == player && _gameBoard[8] == player)
            {
                Console.WriteLine($"{player} Wins!");
                return true;
            }
            else if (_gameBoard[2] == player && _gameBoard[4] == player && _gameBoard[6] == player)
            {
                Console.WriteLine($"{player} Wins!");
                return true;
            }
            return false;
        }

        //TODO: Print a tie message in UI "Console.WriteLine("There's a tie. GAME OVER.");"
        public bool CheckForTie()
        {
            return !_gameBoard.Any(x => x == ' ');
        }
    }
}
