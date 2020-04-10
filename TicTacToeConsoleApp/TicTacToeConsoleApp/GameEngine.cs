﻿using System;
using System.Linq;

namespace TicTacToeConsoleApp
{
    /***
     * I need access to GameBoard outside of this class, for the UI
     * Consider a Player Class. How would this look?
     */
    public class GameEngine
    {
        private char[] _gameBoard;
        private char _playerOne;
        private char _playerTwo;

        public char[] GameBoard { get { return _gameBoard; } }

        //TODO: revisit constructor.
        public GameEngine()
        {
            _playerOne = 'X';
            _playerTwo = 'O';
            _gameBoard = InitalizeGameBoard();
        }

        private char[] InitalizeGameBoard()
        {
            var gameBoard = new char[9];
            gameBoard[0] = '_';
            gameBoard[1] = '_';
            gameBoard[2] = '_';

            gameBoard[3] = '_';
            gameBoard[4] = '_';
            gameBoard[5] = '_';

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


        //TODO: Test
        public bool CheckForWin(char player)
        {
            #region Horizontal Wins
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
            #endregion

            # region Vertical Wins
            else if (_gameBoard[0] == player && _gameBoard[3] == player && _gameBoard[6] == player)
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
            #endregion

            # region Diagonal Wins
            else if (_gameBoard[0] == player && _gameBoard[4] == player && _gameBoard[8] == player)
            {
                Console.WriteLine($"{player} Wins!");
                return true;
            }
            else if (_gameBoard[2] == player && _gameBoard[4] == player && _gameBoard[6] == player)
            {
                Console.WriteLine($"{player} Wins!");
                return true;
            }
            #endregion

            return false;
        }

        public bool CheckForTie()
        {
            int openSpots = _gameBoard.Length;
            foreach(var spot in _gameBoard)
            {
                if (spot == 'X' || spot == 'O')
                {
                    openSpots--;
                }
            }

            if(openSpots == 0)
            {
                Console.WriteLine("There's a tie. GAME OVER.");
                return true;
            }
            return false;
        }

        public string DrawBoard()
        {
            return $" {_gameBoard[0]}|{_gameBoard[1]}|{_gameBoard[2]}"
                + $"\n {_gameBoard[3]}|{_gameBoard[4]}|{_gameBoard[5]}"
                + $"\n {_gameBoard[6]}|{_gameBoard[7]}|{_gameBoard[8]}"
                + "\n";
        }
    }
}