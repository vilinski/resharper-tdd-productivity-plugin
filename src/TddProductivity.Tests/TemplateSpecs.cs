using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using TddProductivity.MoveClass;
using EnvDTE;

namespace TddProductivity.Tests
{
    //public class MoveClassBulbItemSpecs : SpecBase<MoveClassBulbItem>
    //{
    //    protected override MoveClassBulbItem SetupSUT()
    //    {
    //        var project = CreateDependency<IProject>();
    //        var action = CreateDependency<IBulbItem>();
    //        action.Stub(a => a.Execute(null, null)).IgnoreArguments();
    //        var elementFinder = CreateDependency<IElementFinder>();
    //        elementFinder.Stub(e => e.GetElementAtCaret()).Return(CreateDependency<IElement>());

    //        return new MoveClass(project, action, elementFinder);
    //    }

    //    [Test]
    //    public void Should_Copy_the_new_file_to_the_destination_project()
    //    {
    //        EnvDTE.SolutionClass c = new SolutionClass();
    //        EnvDTE.DTEClass dte = new DTEClass()
    //    }

    //}

}