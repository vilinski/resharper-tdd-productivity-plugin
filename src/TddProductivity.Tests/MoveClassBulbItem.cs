using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
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
            var typeDeclaration = CreateDependency<ICSharpTypeDeclaration>();
            return new MoveClass(typeDeclaration, project);
        }

        [Test]
        [Ignore("Should be rewritten")]
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
        public MoveClass([NotNull] ICSharpTypeDeclaration sourceTypeDeclaration, IProject destinationProject) : 
            base(sourceTypeDeclaration, destinationProject)
        {
            CopiedFileToNewProject = false;
        }

        protected override IProjectFile MoveFileToProject(IProjectItem sourceFile)
        {
            return MockRepository.GenerateStub<IProjectFile>();
        }

        protected override void OpenFileInEditor(ISolution solution, IProjectFile newFile)
        {
            CopiedFileToNewProject = true;
        }

        public bool CopiedFileToNewProject { get; set; }
    }
}