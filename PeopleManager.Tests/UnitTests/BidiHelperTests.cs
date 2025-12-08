using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeopleManager.Helpers;

namespace PeopleManager.Tests.UnitTests
{
    [TestClass]
    public class BidiHelperTests
    {
        [TestMethod]
        public void ReverseHebrewRunsAndOrder_NullOrEmpty_ReturnsSame()
        {
            Assert.AreEqual(null, BidiHelper.ReverseHebrewRunsAndOrder(null));
            Assert.AreEqual(string.Empty, BidiHelper.ReverseHebrewRunsAndOrder(string.Empty));
        }

        [TestMethod]
        public void ReverseHebrewRunsAndOrder_HebrewOnly_Reverses()
        {
            var input = "????"; // shalom
            var expected = "????";
            var actual = BidiHelper.ReverseHebrewRunsAndOrder(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReverseHebrewRunsAndOrder_MixedHebrewAndEnglish_ReordersRuns()
        {
            var input = "Hello ????";
            var actual = BidiHelper.ReverseHebrewRunsAndOrder(input);
            // We only assert that Hebrew run is reversed and overall length matches
            Assert.IsTrue(actual.Contains("????"));
            Assert.AreEqual(input.Length, actual.Length);
        }
    }
}
