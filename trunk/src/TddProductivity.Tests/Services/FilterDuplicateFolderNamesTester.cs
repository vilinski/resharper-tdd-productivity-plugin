using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TddProductivity.Folders;

namespace TddProductivity.Tests.Services
{
    [TestFixture]
    public class FilterDuplicateFolderNamesTester
    {
        [Test]
        public void Should_filter_duplicate_names()
        {
            //arrange
            string defaultNamespace = "CodeCampServer.UI";
            string[] folders = new string[]
                                   {
                                    "UI",
                                    "Controllers",
                                    "Area"
                                    };
            //act
            var results = VsFolderCreator.FilterDuplicationFolderNames(defaultNamespace, folders);
            //assert
            Assert.That(results.Length, Is.EqualTo(2));
            Assert.AreEqual(results[0],"Controllers");
            Assert.AreEqual(results[1], "Area");
        }

    }
}