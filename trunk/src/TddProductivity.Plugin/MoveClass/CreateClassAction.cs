using System;
using EnvDTE;
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


        public void Execute()
        {
            CreateFoldersAndCodeFile();
        }

        private void CreateFoldersAndCodeFile()
        {
            var folder = new VsFolderCreator().CreateVsFolder(createClassRequestMessage.Project.GetProject(),
                                                       ConvertNamespaceToFolderNameArray(createClassRequestMessage.Namespace));

            CreateNewFileFromVsTemplate(folder);
        }

        private void CreateNewFileFromVsTemplate(ProjectItem folder)
        {
            
            new TemplateFileCreator().CreateFile(createClassRequestMessage.Project.GetSolution(),folder,createClassRequestMessage.Classname + ".cs",createClassRequestMessage.Classname,createClassRequestMessage.Template);
        }

        //private void CreateClassInProject()
        //{
        //    var folderCreator = new VsFolderCreator();
        //    IProjectFolder folder = folderCreator.GetOrCreateNestedFolders(createClassRequestMessage.Project.GetProject(),
        //                                                                   ConvertNamespaceToFolderNameArray());
        //    var fetcher = new TemplateFetcher(new DefaultTemplateCreator(), new FolderTemplateFetcher(),
        //                                      new TemplateFolderPrioritizer());

        //    Template template;
        //    template = fetcher.FetchTemplate(new TemplateDefinition {Name = "TDD.Class"});

        //    new TemplateFieldPopulator().PopulateTemplate(template, createClassRequestMessage.Classname);
        //    new TemplateFileCreator().CreateFile(createClassRequestMessage.Classname + ".cs", template, folder);
        //}


        private string[] ConvertNamespaceToFolderNameArray(string nameSpace)
        {
            return nameSpace.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}