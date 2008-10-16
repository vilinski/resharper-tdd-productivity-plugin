using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.LiveTemplates.Execution;
using JetBrains.ReSharper.LiveTemplates.Templates;
using JetBrains.TextControl;
using JetBrains.Util;

namespace TddProductivity.Templates
{
    public class TemplateFileCreator : ITemplateFileCreator
    {
        public void CreateFile(string fileName, Template template, IProjectFolder projectFolder)
        {
            IProjectFile file = AddNewItemUtil.AddFile(projectFolder, fileName);

            if (file == null) return;

            ITextControl textControl = EditorManager.GetInstance(file.GetSolution()).OpenProjectFile(file, true);
            textControl.WindowModel.Focus();
            LiveTemplatesController.Instance.ExecuteTemplate(projectFolder.GetSolution(), template, textControl);
        }
    }

    public interface ITemplateFileCreator
    {
    }
}