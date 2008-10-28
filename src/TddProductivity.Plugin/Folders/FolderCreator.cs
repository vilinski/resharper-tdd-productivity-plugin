using EnvDTE;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.Impl;
using JetBrains.Util;
using JetBrains.VSIntegration.ProjectModel;

namespace TddProductivity.Folders
{
    public class FolderCreator
    {
        public IProjectFolder GetOrCreateNestedFolders(IProject project, string[] folders)
        {
            VSProjectInfo projectInfo;
            projectInfo = ProjectModelSynchronizer.GetInstance(project.GetSolution()).GetProjectInfoByProject(project);

            if (projectInfo == null) return null;

            Project envProject = projectInfo.GetExtProject();
            ProjectItem projectItem = null;
            ProjectItems projectItems = envProject.ProjectItems;


            IProjectFolder parent = project;
            
            foreach (string folder in folders)
            {
                var item = parent.GetSubItem(folder);
                if (item == null)
                {
                    JetBrains.ProjectModel.Impl.ProjectFolderImpl fold = (JetBrains.ProjectModel.Impl.ProjectFolderImpl)parent;
                    
                    item = fold.DoCreateFolder(folder);
                    fold.BuildFromFileSystem();
                }
                parent = (IProjectFolder)item;
            }
            project.BuildFromFileSystem();
            project.SaveSettings();
            return parent;
        }

        private static IProjectFolder GetOrCreateFolder(ProjectItems projectItems, string folder)
        {
            
            foreach (ProjectItem projectItem in projectItems)
            {
                if (projectItem.Kind == Constants.vsProjectItemKindPhysicalFolder && projectItem.Name == folder)
                {
                    return (IProjectFolder) projectItem;
                }
            }
            ProjectItem newFolder = projectItems.AddFolder(folder,null);
            //ProjectModelSynchronizer.FixProjectFolder();   
            
            return  null;
        }
    }
}