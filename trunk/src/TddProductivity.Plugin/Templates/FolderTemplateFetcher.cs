using JetBrains.ReSharper.LiveTemplates.Storages;
using JetBrains.ReSharper.LiveTemplates.Templates;

namespace TddProductivity.Templates
{
    public class FolderTemplateFetcher : IFolderTemplateFetcher
    {
        #region IFolderTemplateFetcher Members

        public Template FetchTemplateFromFolder(TemplateDefinition definition, ITemplateStorage folder)
        {
            foreach (Template template in folder.Templates)
            {
                if (template.Description == definition.TemplateName)
                {
                    return template;
                }
            }

            return null;
        }

        #endregion
    }

    public interface IFolderTemplateFetcher
    {
        Template FetchTemplateFromFolder(TemplateDefinition definition, ITemplateStorage folder);
    }
}