using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToeConsoleApp;

namespace TicTacToeTests
{
    [TestClass]
    public class WhenCreatingAGameEngine
    {
        [TestMethod]
        public void AGameBoardIsInitializedCorrectly()
        {
            var gameEngine = new GameEngine();
            
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


            Assert.IsNotNull(gameEngine.GameBoard);
            CollectionAssert.AreEquivalent(gameBoard, gameEngine.GameBoard);
        }
    }
}
