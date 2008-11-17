using System;
using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using ProjectUtil = TddProductivity.Projects.ProjectUtil;

namespace TddProductivity.MoveClass
{
    [QuickFix(100)]
    public class CreateInterfaceQuickFix : IQuickFix
    {
        private readonly NotResolvedError error;
        private readonly IQuickFix quickFix;

        public CreateInterfaceQuickFix(NotResolvedError error)
        {
            this.error = error;

            quickFix = Activator.CreateInterfaceClassFix(error);
        }

        #region IQuickFix Members

        public bool IsAvailable(IUserDataHolder cache)
        {
            bool available = quickFix.IsAvailable(cache);
            return available;
        }

        public IBulbItem[] Items
        {
            get
            {
                var quickFixItems = new List<IBulbItem>();

                IProjectFile projectFile = GetProjectFile();

                string classname = GetClassName();
                if (!classname.StartsWith("I", StringComparison.InvariantCultureIgnoreCase))
                {
                    return quickFixItems.ToArray();
                }

                IProject sourceProject = projectFile.GetProject();

                string relativeNamespace = GetRelativeNamespace(sourceProject);

                foreach (IProject project in ProjectUtil.GetReferencedProjects(sourceProject))
                {
                    BulbItem item = CreateBulbItem(classname, relativeNamespace, project);

                    quickFixItems.Add(item);
                }
                return quickFixItems.ToArray();
            }
        }

        #endregion

        private BulbItem CreateBulbItem(string interfacename, string relativeNamespace, IProject project)
        {
            var DTO = new CreateInterfaceRequestMessage
                          {
                              Interfacename = interfacename,
                              Namespace = relativeNamespace,
                              Project = project
                          };
            string QuickFixText = "Create Interface in " + project.Name;
            return new BulbItem(QuickFixText, new CreateInterfaceAction(DTO));
        }

        private string GetRelativeNamespace(IProject sourceProject)
        {
            string nameSpace = GetNameSpace();
            string defaultNamespace = sourceProject.GetDefaultNamespaceProperty();
            return nameSpace.Replace(defaultNamespace, "");
        }

        private string GetNameSpace()
        {
            var namespaceBodyNode =
                error.Reference.GetElement().GetContainingElement<INamespaceBodyNode>(false);
            return namespaceBodyNode.NamespaceDeclaration.DeclaredName;
        }

        private string GetClassName()
        {
            return ((IReferenceName)error.Reference.GetElement()).ShortName;
        }

        private IProjectFile GetProjectFile()
        {
            return error.Reference.GetElement().GetProjectFile();
        }
    }
}