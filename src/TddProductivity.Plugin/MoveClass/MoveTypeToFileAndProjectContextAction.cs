using System;
using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions;
using JetBrains.ReSharper.Intentions.CSharp.ContextActions;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace TddProductivity.MoveClass
{
    [ContextAction(Description = "Move type to file and another project", 
        Name = "Move type to file and another project",
        Priority = -1, Group = "C#")]
    public class MoveTypeToFileAndProjectContextAction : IContextAction
    {
        private readonly ICSharpContextActionDataProvider _provider;
        private IProject _currentProject;

        public MoveTypeToFileAndProjectContextAction(ICSharpContextActionDataProvider provider)
        {
            _provider = provider;
        }     

        #region IContextAction Members

        public bool IsAvailable(IUserDataHolder cache)
        {
            var typeDeclaration = _provider.GetSelectedElement<ICSharpTypeDeclaration>(true, true);
            if (typeDeclaration == null || !typeDeclaration.IsValid()) return false;

            var element = typeDeclaration.DeclaredElement;
            if (element == null) return false;

            if (typeDeclaration.GetContainingTypeDeclaration() != null) return false;
            if (!typeDeclaration.GetNameRange().Contains(_provider.CaretOffset)) return false;

            _currentProject = typeDeclaration.GetProject();

            if (RenameFileToMatchTypeNameAction.CountTopLevelTypeDeclarations(_provider.PsiFile as ICSharpFile) <= 1) return false;
            return RenameFileToMatchTypeNameAction.TypeNameNameDoesNotCorrespondWithFileName(element, _provider.ProjectFile);
        }

        public IBulbItem[] Items
        {
            get
            {
                ICollection<IProjectReference> refs = _currentProject.GetProjectReferences();

                var items = new List<IBulbItem>();
                var typeDeclaration = _provider.GetSelectedElement<ICSharpTypeDeclaration>(true, true);
                if (typeDeclaration == null) throw new NullReferenceException("typeDeclaration == null");

                foreach (IProjectReference reference in refs)
                {
                    IProject project = reference.ResolveReferencedProject();
                    if (CanMoveToThisProject(project))
                        items.Add(new MoveClassBulbItem(typeDeclaration, project));
                }
                return items.ToArray();
            }
        }

        #endregion

        private bool CanMoveToThisProject(IProject project)
        {
            return project.Kind == ProjectItemKind.PROJECT &&
                   !project.Name.Equals(_currentProject.Name, StringComparison.InvariantCultureIgnoreCase) &&
                   project.LanguageType == _currentProject.LanguageType;
        }
    }
}