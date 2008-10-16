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
        private readonly ElementFinder _elementFinder;
        private readonly ISolution _solution;
        private readonly ITextControl _textControl;


        public MoveTypeToFileAndProjectContextAction(ISolution solution, ITextControl textControl)

        {
            _solution = solution;
            _textControl = textControl;
            _elementFinder = new ElementFinder(solution, textControl);
            _action = new MoveTypeToAnotherFileAction(solution, textControl);
        }

        public IProject CurrentProject { get; set; }

        #region IContextAction Members

        public bool IsAvailable(IUserDataHolder cache)
        {
            IElement element = _elementFinder.GetElementAtCaret();

            if (element == null)
                return false;

            CurrentProject = element.GetProject();

            return _action.IsAvailable(cache);
        }

        public IBulbItem[] Items
        {
            get
            {
                ICollection<IProjectReference> refs = CurrentProject.GetProjectReferences();

                var items = new List<IBulbItem>();
                foreach (IProjectReference reference in refs)
                {
                    IProject project = reference.ResolveReferencedProject();
                    if (CanMoveToThisProject(project))
                    {
                        items.Add(new MoveClassBulbItem(project, _action, new ElementFinder(_solution, _textControl)));
                    }
                }
                return items.ToArray();
            }
        }

        #endregion

        public bool CanMoveToThisProject(IProject project)
        {
            return project.Kind == ProjectItemKind.PROJECT &&
                   !project.Name.Equals(CurrentProject.Name, StringComparison.InvariantCultureIgnoreCase) &&
                   project.LanguageType == CurrentProject.LanguageType;
        }
    }
}