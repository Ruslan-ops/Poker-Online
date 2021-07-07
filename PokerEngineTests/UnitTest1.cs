using NUnit.Framework;
using NUnit.Framework.Constraints;
using PokerEngine;

namespace PokerEngineTests
{
    public class Tests
    {
        private string _text;

        [SetUp]
        public void Setup()
        {
            _text = "Hello";
        }

        [Test]
        public void Test1()
        {
            Assert.IsTrue(_text.StartsWithUpper());
            SecondInternal si = new SecondInternal();
            Assert.IsTrue(si.GetTrue());
        }
    }
}