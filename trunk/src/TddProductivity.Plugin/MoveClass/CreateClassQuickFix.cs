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
    public class CreateClassQuickFix : IQuickFix
    {
        private readonly NotResolvedError error;
        private readonly IQuickFix quickFix;

        public CreateClassQuickFix(NotResolvedError error)
        {
            this.error = error;

            quickFix = Activator.CreateCreateClassFix(error);
        }

        

        public bool IsAvailable(IUserDataHolder cache)
        {
            bool available = quickFix.IsAvailable(cache);
            return available;
        }

        public IBulbItem[] Items
        {
            get
            {
                string templateFilename="tdd.class";
                string templateName="Class";
                var quickFixItems = new List<IBulbItem>();

                IProjectFile projectFile = GetProjectFile();

                string classname = GetClassName();
                if (classname.StartsWith("I", StringComparison.InvariantCultureIgnoreCase))
                {
                    templateFilename = "tdd.interface";
                    templateName = "Interface";
                }

                IProject sourceProject = projectFile.GetProject();

                string relativeNamespace = GetRelativeNamespace(sourceProject);

                foreach (IProject project in ProjectUtil.GetReferencedProjects(sourceProject))
                {
                    BulbItem item = CreateBulbItem(classname, relativeNamespace, project, templateFilename , templateName);

                    quickFixItems.Add(item);
                }
                return quickFixItems.ToArray();
            }
        }

        

        private BulbItem CreateBulbItem(string classname, string relativeNamespace, IProject project, string templateName, string codeFileToCreateName)
        {
            var DTO = new CreateClassRequestMessage
                          {
                              Classname = classname,
                              Namespace = relativeNamespace,
                              Project = project,
                              Template= templateName
                          };
            string QuickFixText = string.Format("Create {0} in {1}" ,codeFileToCreateName, project.Name);
            return new BulbItem(QuickFixText, new CreateClassAction(DTO));
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
