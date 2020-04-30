using System.Linq;

namespace TicTacToe.Shared
{
    //TODO: Consider adding logic for GameEngine to know about WinningPlayer
    //This may be useful info to send to the server hub.
    public class GameEngine
    {
        private char[] _gameBoard;
        public char[] GameBoard { get { return _gameBoard; } }

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
            return _gameBoard[move] == ' ';
        }

        public bool CheckForWin()
        {
            return (DidWinByRow() || DidWinByColumn() || DidWinByDiagonal());
        }

        private bool DidWinByRow()
        {
            if (_gameBoard[0] != ' ' && _gameBoard[0] == _gameBoard[1] && _gameBoard[1] == _gameBoard[2])
            {
                return true;
            }
            else if (_gameBoard[3] != ' ' && _gameBoard[3] == _gameBoard[4] && _gameBoard[4] == _gameBoard[5])
            {
                return true;
            }
            else if (_gameBoard[6] != ' ' && _gameBoard[6] == _gameBoard[7] && _gameBoard[7] == _gameBoard[8])
            {
                return true;
            }

            return false;
        }

        private bool DidWinByColumn()
        {
            if (_gameBoard[0] != ' ' && _gameBoard[0] == _gameBoard[3] && _gameBoard[3] == _gameBoard[6])
            {
                return true;
            }
            else if (_gameBoard[1] != ' ' && _gameBoard[1] == _gameBoard[4] && _gameBoard[4] == _gameBoard[7])
            {
                return true;
            }
            else if (_gameBoard[2] != ' ' && _gameBoard[2] == _gameBoard[5] && _gameBoard[5] == _gameBoard[8])
            {
                return true;
            }

            return false;
        }

        private bool DidWinByDiagonal()
        {
            if (_gameBoard[0] != ' ' && _gameBoard[0] == _gameBoard[4] && _gameBoard[4] == _gameBoard[8])
            {
                return true;
            }
            else if (_gameBoard[2] != ' ' && _gameBoard[2] == _gameBoard[4] && _gameBoard[4] == _gameBoard[6])
            {
                return true;
            }
            return false;
        }

        public bool CheckForTie()
        {
            return !_gameBoard.Any(x => x == ' ');
        }
    }
}
