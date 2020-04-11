using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToe.Shared;

namespace TicTacToeTests.GameEngineTests
{
    [TestClass]
    public class WhenAMoveIsMade
    {
        GameEngine _gameEngine;

        [TestInitialize]
        public void TestInitialize()
        {
            _gameEngine = new GameEngine();
        }

        [TestMethod]
        public void AndTheMoveFailsThenFalseIsReturned()
        {
            _gameEngine.TryPlayerMove(0, 'X');

            Assert.IsFalse(_gameEngine.TryPlayerMove(0, 'X'));
        }

        [TestMethod]
        public void AndTheMoveSucceedsThenTrueIsReturnedAndBoardIsDrawnCorrectly()
        {
            var didMoveSucceed = _gameEngine.TryPlayerMove(0, 'X');
            var expectedBoard = new char[9] { 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };

            Assert.IsTrue(didMoveSucceed);
            CollectionAssert.AreEquivalent(expectedBoard, _gameEngine.GameBoard);
        }

        [TestMethod]
        public void AndTheMoveFailsThenFalseIsReturnedAndBoardStateIsRetained()
        {
            _gameEngine.TryPlayerMove(0, 'X');

            var didMoveSucceed = _gameEngine.TryPlayerMove(0, 'X');
            var expectedBoard = new char[9] { 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };

            Assert.IsFalse(didMoveSucceed);
            CollectionAssert.AreEquivalent(expectedBoard, _gameEngine.GameBoard);
        }


        [TestMethod]
        [DataRow(0, 1, 2, DisplayName = "WinByRowCombo1")]
        [DataRow(3, 4, 5, DisplayName = "WinByRowCombo2")]
        [DataRow(6, 7, 8, DisplayName = "WinByRowCombo3")]
        [DataRow(0, 3, 6, DisplayName = "WinByColumnCombo1")]
        [DataRow(1, 4, 7, DisplayName = "WinByColumnCombo2")]
        [DataRow(2, 5, 8, DisplayName = "WinByColumnCombo3")]
        [DataRow(0, 4, 8, DisplayName = "WinByDiagonalCombo1")]
        [DataRow(2, 4, 6, DisplayName = "WinByDiagonalCombo2")]
        public void AndItsAWinningMoveThenTrueIsReturned(int a, int b, int c)
        {
            _gameEngine.TryPlayerMove(a, 'X');
            _gameEngine.TryPlayerMove(b, 'X');
            _gameEngine.TryPlayerMove(c, 'X');

            Assert.IsTrue(_gameEngine.CheckForWin());
        }

        [TestMethod]
        public void AndItsADrawThenTrueIsReturned()
        {
            _gameEngine.TryPlayerMove(0, 'X');
            _gameEngine.TryPlayerMove(1, 'X');
            _gameEngine.TryPlayerMove(2, 'O');
            _gameEngine.TryPlayerMove(3, 'O');
            _gameEngine.TryPlayerMove(4, 'O');
            _gameEngine.TryPlayerMove(5, 'X');
            _gameEngine.TryPlayerMove(6, 'X');
            _gameEngine.TryPlayerMove(7, 'O');
            _gameEngine.TryPlayerMove(8, 'X');

            Assert.IsFalse(_gameEngine.CheckForWin());
            Assert.IsTrue(_gameEngine.CheckForTie());
        }

        [TestMethod]
        public void AndItsNOTADrawOrWinThenFalseIsReturned()
        {
            _gameEngine.TryPlayerMove(0, 'X');

            Assert.IsFalse(_gameEngine.CheckForTie());
            Assert.IsFalse(_gameEngine.CheckForWin());
        }
    }
}
