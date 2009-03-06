using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.FileTemplates;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Storages;
using JetBrains.ReSharper.LiveTemplates.FileTemplates;
using JetBrains.Util;

namespace TddProductivity.Templates
{
    public class TemplateFolderPrioritizer : ITemplateStoragePrioritizer
    {

        public ITemplateStorage[] EnumerateFolders()
        {
            IList<ITemplateStorage> folders = new List<ITemplateStorage>();

            //FileTemplatesManager.Instance.TemplateFamily.TemplateStorages.Where(
            //    i => i.Name == "Shared Solution Templates");


            foreach (string name in new[] {"Shared Solution Templates", "Personal Solution Templates", "User Templates"}
                )
            {
                foreach (ITemplateStorage i in FileTemplatesManager.Instance.TemplateFamily.TemplateStorages)
                {
                    if (i.Name == name)
                    {
                        folders.Add(i);
                    }
                }
            }
            return folders.ToArray();
        }
    }

    public interface ITemplateStoragePrioritizer
    {
        ITemplateStorage[] EnumerateFolders();
    }
}