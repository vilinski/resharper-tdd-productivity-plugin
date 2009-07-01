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
            Assert.AreEqual(results[0], "Controllers");
            Assert.AreEqual(results[1], "Area");
        }

        [Test]
        public void Should_leave_well_enough_alone()
        {
            //arrange
            string defaultNamespace = "CodeCampServer.FooFighters";
            string[] folders = new string[]
                                   {
                                    "UI",
                                    "Controllers",
                                    "Area"
                                    };
            //act
            var results = VsFolderCreator.FilterDuplicationFolderNames(defaultNamespace, folders);
            //assert
            Assert.That(results.Length, Is.EqualTo(3));
            Assert.AreEqual(results[0], "UI");
            Assert.AreEqual(results[1], "Controllers");
            Assert.AreEqual(results[2], "Area");
        }

        [Test]
        public void Should_not_change_empty_folder_collection()
        {
            //arrange
            string defaultNamespace = "CodeCampServer.FooFighters";
            string[] folders = new string[0];

            //act
            var results = VsFolderCreator.FilterDuplicationFolderNames(defaultNamespace, folders);
            //assert
            Assert.That(results.Length, Is.EqualTo(0));
        }
    }
}