using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using EnvDTE80;
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

        public void CreateFile(ISolution solution, ProjectItem projectItem, string fileName,
                               string itemName,string templateName)
        {
            string filePath = Path.Combine(projectItem.get_FileNames(0), fileName);

            var dteSolution = projectItem.DTE.Solution as Solution2;

            string templatepath = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), "Resources\\"+templateName + "\\ClassUnderTest.vstemplate");
                //dteSolution.GetProjectItemTemplate(templateName +".zip", "CSharp");
            ProjectItem fileitem = null;
            projectItem.ProjectItems.AddFromTemplate(templatepath, fileName);
            foreach (ProjectItem item in projectItem.ProjectItems)
            {
                if(item.Name==fileName)
                {
                    fileitem = item;
                    break;
                }
            }

            //File.CreateText(filePath).Close();
            //ProjectItem fileitem = projectItem.ProjectItems .AddFromFile(filePath);
            projectItem.ContainingProject.Save(null);
            
            var window = fileitem.Open("{7651A701-06E5-11D1-8EBD-00A0C90F26EA}");
            window.Close(vsSaveChanges.vsSaveChangesYes);

            //var progitem = solution.FindProjectItemByLocation(new FileSystemPath(filePath)) as IProjectFile;

            //ITextControl textControl = EditorManager.GetInstance(solution).OpenProjectFile(progitem, true);
            ////ITextControl textControl = EditorManager.GetInstance(solution).OpenFile(filePath,true,false);
            //textControl.WindowModel.Focus();

            //bool result = LiveTemplatesController.Instance.ExecuteTemplate(solution, template, textControl);
        }
    }

    public interface ITemplateFileCreator
    {
    }
}