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
            gameBoard[0] = '_';
            gameBoard[1] = '_';
            gameBoard[2] = '_';

            gameBoard[3] = '_';
            gameBoard[4] = '_';
            gameBoard[5] = '_';

            gameBoard[6] = ' ';
            gameBoard[7] = ' ';
            gameBoard[8] = ' ';

            Assert.IsNotNull(gameEngine.GameBoard);
            CollectionAssert.AreEquivalent(gameBoard, gameEngine.GameBoard);
        }

        [TestMethod]
        public void ThenAnEmptyBoardIsDrawnCorrectly()
        {
            var gameEngine = new GameEngine();
            var expectedBoard = $" {'_'}|{'_'}|{'_'}"
                + $"\n {'_'}|{'_'}|{'_'}"
                + $"\n {' '}|{' '}|{' '}"
                + "\n";

            Assert.AreEqual(expectedBoard, gameEngine.DrawBoard());
        }
    }
}
