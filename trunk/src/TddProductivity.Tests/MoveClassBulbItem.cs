using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using TddProductivity.MoveClass;

namespace TddProductivity.Tests
{
    public class MoveClassBulbItemSpecs : SpecBase<MoveClassBulbItem>
    {
        protected override MoveClassBulbItem SetupSUT()
        {
            var project = CreateDependency<IProject>();
            var action = CreateDependency<IBulbItem>();
            action.Stub(a => a.Execute(null, null)).IgnoreArguments();
            var elementFinder = CreateDependency<IElementFinder>();
            elementFinder.Stub(e => e.GetElementAtCaret()).Return(CreateDependency<IElement>());

            return new MoveClass(project, action, elementFinder);
        }

        [Test]
        public void Should_Copy_the_new_file_to_the_destination_project()
        {
            var solution = CreateDependency<ISolution>();
            var textControl = CreateDependency<ITextControl>();

            SUT.Execute(solution, textControl);
            var sut = SUT as MoveClass;
            Assert.That(sut.CopiedFileToNewProject,Is.True);
        }





    }

    public class MoveClass : MoveClassBulbItem
    {
        public MoveClass(IProject project, IBulbItem action, IElementFinder elementFinder)
            : base(project, action, elementFinder)
        {
            CopiedFileToNewProject = false;
        }

        public override void AssertReadAccess()
        {
        }

        public override IProjectItem GetSourceFile(IElement element)
        {
            return MockRepository.GenerateStub<IProjectItem>();
        }

        public override IProjectFile MoveFileToProject(IProjectItem sourceFile)
        {
            return MockRepository.GenerateStub<IProjectFile>();
        }

        public override void OpenFileInEditor(ISolution solution, IProjectFile newFile)
        {
            CopiedFileToNewProject = true;
        }

        public bool CopiedFileToNewProject { get; set; }
    }
}