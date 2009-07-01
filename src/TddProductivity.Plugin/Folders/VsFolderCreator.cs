using System.Collections;
using System.Collections.Generic;
using EnvDTE;
using JetBrains.ProjectModel;
using JetBrains.VSIntegration.ProjectModel;

namespace TddProductivity.Folders
{
    public class VsFolderCreator
    {
        //public IProjectFolder GetOrCreateNestedFolders(IProject project, string[] folders)
        //{
        //    VSProjectInfo projectInfo;
        //    projectInfo = ProjectModelSynchronizer.GetInstance(project.GetSolution()).GetProjectInfoByProject(project);

        //    if (projectInfo == null) return null;


        //    Project envProject = projectInfo.GetExtProject();
        //    ProjectItem projectItem = null;
        //    ProjectItems projectItems = envProject.ProjectItems;
        //    foreach (string folder in folders)
        //    {
        //        projectItem = GetOrCreateFolder(projectItems, folder);
        //        if (projectItem == null) return null;
        //        envProject.Save(envProject.FullName);
        //        projectItems = projectItem.ProjectItems;
        //    }
        //    IProjectFolder createNestedFolders=null;
        //    do
        //    {
        //        //Application.DoEvents();
        //        System.Threading.Thread.Sleep(100);
        //        createNestedFolders =
        //            project.GetSolution().FindProjectItemByLocation(new FileSystemPath(projectItem.get_FileNames(0))) as
        //            IProjectFolder;
        //    } while (createNestedFolders == null);

        //    return createNestedFolders;
        //    //return project.FindProjectItemByLocation(new FileSystemPath(projectItem.get_FileNames(0))) as IProjectFolder;
        //}

        public static string[] FilterDuplicationFolderNames(string defaultNamespace, string[] foldersToCreate)
        {
            List<string> output = new List<string>(foldersToCreate);
            foreach (string nameSpace in defaultNamespace.Split('.'))
            {
                if (output[0].Equals(nameSpace))
                {
                    output.RemoveAt(0);
                }
            }


            return output.ToArray();
        }

        public ProjectItems CreateVsFolder(IProject project, string[] folders)
        {
            
            VSProjectInfo projectInfo;
            projectInfo = ProjectModelSynchronizer.GetInstance(project.GetSolution()).GetProjectInfoByProject(project);

            if (projectInfo == null) return null;

            folders = FilterDuplicationFolderNames(project.GetDefaultNamespaceProperty(), folders);

            Project envProject = projectInfo.GetExtProject();
            
            ProjectItems projectItems = envProject.ProjectItems;
            ProjectItem projectItem = null;
            foreach (string folder in folders)
            {
                projectItem = GetOrCreateFolder(projectItems, folder);
                if (projectItem == null) return null;
                envProject.Save(envProject.FullName);
                projectItems = projectItem.ProjectItems;
            }
            return projectItems;
            //return project.FindProjectItemByLocation(new FileSystemPath(projectItem.get_FileNames(0))) as IProjectFolder;
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