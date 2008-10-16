using System;
using System.Collections.Generic;
using JetBrains.ProjectModel;

namespace TddProductivity.Projects
{
    public class ProjectUtil
    {
        public static IEnumerable<IProject> GetReferencedProjects(IProject sourceProject)
        {
            var items = new List<IProject>();
            ICollection<IProjectReference> refs = sourceProject.GetProjectReferences();
            foreach (IProjectReference reference in refs)
            {
                IProject project = reference.ResolveReferencedProject();
                if (CanMoveToThisProject(sourceProject, project))
                {
                    items.Add(project);
                }
            }
            return items.ToArray();
        }

        public static bool CanMoveToThisProject(IProject source, IProject destination)
        {
            return destination.Kind == ProjectItemKind.PROJECT &&
                   !destination.Name.Equals(source.Name, StringComparison.InvariantCultureIgnoreCase) &&
                   destination.LanguageType == source.LanguageType;
        }
    }
}