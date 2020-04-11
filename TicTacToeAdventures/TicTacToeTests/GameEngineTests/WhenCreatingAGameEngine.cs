using TicTacToe.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicTacToeTests.GameEngineTests
{
    [TestClass]
    public class WhenCreatingAGameEngine
    {
        [TestMethod]
        public void ThenGameBoardIsInitializedCorrectly()
        {
            var gameEngine = new GameEngine();
            
            var expectedGameBoard = new char[9];
            expectedGameBoard[0] = ' ';
            expectedGameBoard[1] = ' ';
            expectedGameBoard[2] = ' ';
            expectedGameBoard[3] = ' ';
            expectedGameBoard[4] = ' ';
            expectedGameBoard[5] = ' ';
            expectedGameBoard[6] = ' ';
            expectedGameBoard[7] = ' ';
            expectedGameBoard[8] = ' ';


            Assert.IsNotNull(gameEngine.GameBoard);
            CollectionAssert.AreEquivalent(expectedGameBoard, gameEngine.GameBoard);
        }
    }
}
