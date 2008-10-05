using System;
using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Intentions.CSharp.ContextActions;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace TddProductivity.MoveClass
{
    [ContextAction(Description = "Move type to file and another project", Name = "Move type to file and another project"
        , Priority = -1, Group = "C#")]
    public class MoveTypeToFileAndProjectContextAction : IContextAction
    {
        private readonly MoveTypeToAnotherFileAction _action;
        private readonly ISolution _solution;
        private readonly ITextControl _textControl;
        private IProject _currentProject;
        private ElementFinder _elementFinder;

        public MoveTypeToFileAndProjectContextAction(ISolution solution, ITextControl textControl)

        {
            _solution = solution;
            _textControl = textControl;
            _elementFinder = new ElementFinder(solution,textControl);
            _action = new MoveTypeToAnotherFileAction(solution, textControl);
        }

        public bool IsAvailable(IUserDataHolder cache)
        {
            IElement element = _elementFinder.GetElementAtCaret();

            if (element == null)
                return false;

            _currentProject = element.GetProject();

            return _action.IsAvailable(cache);
        }

        public IBulbItem[] Items
        {
            get
            {
                ICollection<IProjectReference> refs = _currentProject.GetProjectReferences();

                var items = new List<IBulbItem>();
                foreach (IProjectReference reference in refs)
                {
                    IProject project = reference.ResolveReferencedProject();
                    if (CanMoveToThisProject(project))
                    {
                        items.Add(new MoveClassBulbItem(project, _action));
                    }
                }
                return items.ToArray();
            }
        }

        public bool CanMoveToThisProject(IProject project)
        {
            return project.Kind == ProjectItemKind.PROJECT &&
                   !project.Name.Equals(_currentProject.Name, StringComparison.InvariantCultureIgnoreCase) &&
                   project.LanguageType == _currentProject.LanguageType;
        }

    }
}