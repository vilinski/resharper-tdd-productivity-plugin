using EnvDTE;
using JetBrains.ProjectModel;
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
            foreach (string folder in folders)
            {
                projectItem = GetOrCreateFolder(projectItems, folder);
                if (projectItem == null) return null;
                projectItems = projectItem.ProjectItems;
            }

            return project.FindProjectItemByLocation(new FileSystemPath(projectItem.get_FileNames(0))) as IProjectFolder;
        }

        private static ProjectItem GetOrCreateFolder(ProjectItems projectItems, string folder)
        {
            foreach (ProjectItem projectItem in projectItems)
            {
                if (projectItem.Kind == Constants.vsProjectItemKindPhysicalFolder && projectItem.Name == folder)
                {
                    return projectItem;
                }
            }

            ProjectItem newFolder = projectItems.AddFolder(folder, Constants.vsProjectItemKindPhysicalFolder);
            return newFolder;
        }
    }
}