using System;
using System.Collections.Generic;
using AgentJohnson.MoveClass.asdf;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Intentions.CSharp.ContextActions;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace AgentJohnson.MoveClass
{
    [ContextAction(Description = "Move type to another project", Name = "Move type to another project", Priority = -1, Group = "C#")]
    public class MoveClassContextAction:ContextActionBase
    {
        private IProject _currentProject;
        private JetBrains.ReSharper.Intentions.CSharp.ContextActions.MoveTypeToAnotherFileAction action;
        public MoveClassContextAction(ISolution solution, ITextControl textControl) : base(solution, textControl)
        {
            action = new MoveTypeToAnotherFileAction(solution,textControl);
        }
        protected override void Execute(JetBrains.ReSharper.Psi.Tree.IElement element)
        {
            throw new System.NotImplementedException();
        }
        protected override JetBrains.ReSharper.Daemon.IBulbItem[] GetItems()
        {
            var refs = _currentProject.GetProjectReferences();
            
            List<IBulbItem> items = new List<IBulbItem>(    );
            foreach (IProjectReference reference in refs)
            {
                var project = reference.GetProject();
                if (project.Kind == ProjectItemKind.PROJECT && !project.Name.Equals(_currentProject.Name,StringComparison.InvariantCultureIgnoreCase)&& project.LanguageType==_currentProject.LanguageType)
                {
                    items.Add(new MoveClassBulbItem(project));
                }
            }
            return items.ToArray();
        }
        protected override string GetText()
        {
            return "Move file to another project";
        }

        public bool IsAvailable(IUserDataHolder cache)
        {
            return action.IsAvailable(cache); 
            //Shell.Instance.Locks.AssertReadAccessAllowed();

            //IElement element = GetElementAtCaret();
            //if (element == null)
            //{
            //    return false;
            //}

            //return IsAvailable(element);
        }

        protected override bool IsAvailable(JetBrains.ReSharper.Psi.Tree.IElement element)
        {
           
            
            _currentProject = element.GetProject();
            ITypeDeclaration declaration= element.GetContainingElement<JetBrains.ReSharper.Psi.Tree.ITypeDeclaration>(true);
            
            if (declaration==null)
                return false;
            return true;
        }
    }
}