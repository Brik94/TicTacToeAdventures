using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToeConsoleApp;

namespace TicTacToeTests
{
    [TestClass]
    public class WhenAMoveIsMade
    {
        [TestMethod]
        public void AndTheMoveIsValidThenTrueIsReturned()
        {
            var gameEngine = new GameEngine();

            var result = gameEngine.TryPlayerMove(0, 'X');

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AndTheMoveIsNOTValidThenFalseIsReturned()
        {
            var gameEngine = new GameEngine();

            gameEngine.TryPlayerMove(0, 'X');
            var result = gameEngine.TryPlayerMove(0, 'X');

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AndTheMoveIsValidThenAnUpdatedBoardIsDrawnCorrectly()
        {
            var gameEngine = new GameEngine();
            gameEngine.TryPlayerMove(0, 'X');

            var expectedBoard = $" {'X'}|{'_'}|{'_'}"
                + $"\n {'_'}|{'_'}|{'_'}"
                + $"\n {' '}|{' '}|{' '}"
                + "\n";

            Assert.AreEqual(expectedBoard, gameEngine.DrawBoard());
        }

        [TestMethod]
        public void AndTheMoveIsNOTValidThenTheBoardStateIsRetained()
        {
            var gameEngine = new GameEngine();

            gameEngine.TryPlayerMove(0, 'X');
            gameEngine.TryPlayerMove(0, 'X');

            var expectedBoard = $" {'X'}|{'_'}|{'_'}"
                + $"\n {'_'}|{'_'}|{'_'}"
                + $"\n {' '}|{' '}|{' '}"
                + "\n";

            Assert.AreEqual(expectedBoard, gameEngine.DrawBoard());
        }

        [TestMethod]
        public void AndItsAWinningMoveThenTrueIsReturned()
        {
            var gameEngine = new GameEngine();

            gameEngine.TryPlayerMove(0, 'X');
            gameEngine.TryPlayerMove(1, 'X');
            gameEngine.TryPlayerMove(2, 'X');

            Assert.IsTrue(gameEngine.CheckForWin('X'));
        }

        [TestMethod]
        public void AndItsADrawThenTrueIsReturned()
        {
            var gameEngine = new GameEngine();

            gameEngine.TryPlayerMove(0, 'X');
            gameEngine.TryPlayerMove(1, 'X');
            gameEngine.TryPlayerMove(2, 'X');
            gameEngine.TryPlayerMove(3, 'X');
            gameEngine.TryPlayerMove(4, 'X');
            gameEngine.TryPlayerMove(5, 'X');
            gameEngine.TryPlayerMove(6, 'X');
            gameEngine.TryPlayerMove(7, 'X');
            gameEngine.TryPlayerMove(8, 'X');

            Assert.IsTrue(gameEngine.CheckForTie());
        }
    }
}
