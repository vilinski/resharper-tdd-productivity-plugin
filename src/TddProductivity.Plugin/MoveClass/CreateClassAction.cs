using System;
using EnvDTE;

using TddProductivity.Folders;
using TddProductivity.Templates;

namespace TddProductivity.MoveClass
{
    public class CreateClassAction : IExecuteAction
    {
        private readonly CreateClassRequestMessage _createClassRequestMessage;

        public CreateClassAction(CreateClassRequestMessage createClassRequestMessage)
        {
            _createClassRequestMessage = createClassRequestMessage;
        }

        public void Execute()
        {
            CreateFoldersAndCodeFile();
        }

        private void CreateFoldersAndCodeFile()
        {
            var folder = new VsFolderCreator().CreateVsFolder(_createClassRequestMessage.Project.GetProject(),
                                                       ConvertNamespaceToFolderNameArray(_createClassRequestMessage.Namespace));

            CreateNewFileFromVsTemplate(folder);
        }

        private void CreateNewFileFromVsTemplate(ProjectItems folder)
        {
            
            new TemplateFileCreator().CreateFile(_createClassRequestMessage.Project.GetSolution(),folder,_createClassRequestMessage.Classname + ".cs",_createClassRequestMessage.Classname,_createClassRequestMessage.Template);
        }

        private string[] ConvertNamespaceToFolderNameArray(string nameSpace)
        {
            return nameSpace.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}