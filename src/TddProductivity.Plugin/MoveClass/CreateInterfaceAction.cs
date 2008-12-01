using System;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.LiveTemplates.Templates;
using TddProductivity.Folders;
using TddProductivity.Services.Templates.Impl;
using TddProductivity.Templates;

namespace TddProductivity.MoveClass
{
    public class CreateInterfaceAction : IExecuteAction
    {
        private readonly CreateInterfaceRequestMessage createInterfaceRequestMessage;

        public CreateInterfaceAction(CreateInterfaceRequestMessage createInterfaceRequestMessage)
        {
            this.createInterfaceRequestMessage = createInterfaceRequestMessage;
        }

        #region IExecuteAction Members

        public void Execute()
        {
            //var folderCreator = new VsFolderCreator();
            //IProjectFolder folder = folderCreator.GetOrCreateNestedFolders(createInterfaceRequestMessage.Project.GetProject(),
            //                                                               ConvertNamespaceToFolderNameArray());
            //var fetcher = new TemplateFetcher(new DefaultTemplateCreator(), new FolderTemplateFetcher(),
            //                                  new TemplateFolderPrioritizer());

            //Template template = fetcher.FetchTemplate(new TemplateDefinition { Name = "TDD.Interface" });

            //new TemplateFieldPopulator().PopulateTemplate(template, createInterfaceRequestMessage.Interfacename);
            //new TemplateFileCreator().CreateFile(createInterfaceRequestMessage.Interfacename + ".cs", template, folder);
        }

        #endregion

        private string[] ConvertNamespaceToFolderNameArray()
        {
            return createInterfaceRequestMessage.Namespace.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}