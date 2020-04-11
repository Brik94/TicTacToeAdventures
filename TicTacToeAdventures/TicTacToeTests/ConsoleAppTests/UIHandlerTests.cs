using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToeConsoleApp;

namespace TicTacToeTests.ConsoleAppTests
{
    [TestClass]
    public class UIHandlerTests
    {
        UIHandler _uiHandler;

        [TestInitialize]
        public void TestInitialize()
        {
            _uiHandler = new UIHandler();
        }

        [TestMethod]
        public void WhenInputIsValidAnIntIsReturned()
        {
            Assert.IsTrue(_uiHandler.IsInputValid("1"));
        }

        [TestMethod]
        public void WhenInputIsNOTValidFalseIsReturned()
        {
            Assert.IsFalse(_uiHandler.IsInputValid("wrong"));
        }

        [TestMethod]
        public void DrawsBoardCorrectly()
        {
            char[] board = new char[9] { 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };

            var expectedBoard = "\n     |     |      \n"
                + $"  X  |     |   \n"

                + "_____|_____|_____ \n"

                + "     |     |      \n"

                + $"     |     |   \n"

                + "_____|_____|_____ \n"

                + "     |     |      \n"

                + $"     |     |   \n"

                + "     |     |      \n";

            Assert.AreEqual(expectedBoard, _uiHandler.DrawBoard(board)); 
        }
    }
}
