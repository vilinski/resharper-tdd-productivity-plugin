using System;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.LiveTemplates.Templates;
using TddProductivity.Folders;
using TddProductivity.Services.Templates.Impl;
using TddProductivity.Templates;

namespace TddProductivity.MoveClass
{
    public class CreateClassAction : IExecuteAction
    {
        private readonly CreateClassRequestMessage createClassRequestMessage;

        public CreateClassAction(CreateClassRequestMessage createClassRequestMessage)
        {
            this.createClassRequestMessage = createClassRequestMessage;
        }

        #region IExecuteAction Members

        public void Execute()
        {
            var folderCreator = new FolderCreator();
            IProjectFolder folder = folderCreator.GetOrCreateNestedFolders(createClassRequestMessage.Project,
                                                                           ConvertNamespaceToFolderNameArray());
            var fetcher = new TemplateFetcher(new DefaultTemplateCreator(), new FolderTemplateFetcher(),
                                              new TemplateFolderPrioritizer());

            Template template;
            template = fetcher.FetchTemplate(new TemplateDefinition {Name = "TDD.Class"});

            new TemplateFieldPopulator().PopulateTemplate(template, createClassRequestMessage.Classname);
            new TemplateFileCreator().CreateFile(createClassRequestMessage.Classname + ".cs", template, folder);
        }

        #endregion

        private string[] ConvertNamespaceToFolderNameArray()
        {
            return createClassRequestMessage.Namespace.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}