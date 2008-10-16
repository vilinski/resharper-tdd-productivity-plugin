using JetBrains.ReSharper.LiveTemplates.Storages;
using JetBrains.ReSharper.LiveTemplates.Templates;

namespace TddProductivity.Templates
{
    public class TemplateFetcher : ITemplateFetcher
    {
        private readonly IDefaultTemplateCreator _creator;
        private readonly IFolderTemplateFetcher _fetcher;
        private readonly ITemplateStoragePrioritizer _prioritizer;

        public TemplateFetcher(IDefaultTemplateCreator creator, IFolderTemplateFetcher fetcher,
                               ITemplateStoragePrioritizer prioritizer)
        {
            _creator = creator;
            _prioritizer = prioritizer;
            _fetcher = fetcher;
        }

        public Template FetchTemplate(TemplateDefinition definition)
        {
            foreach (ITemplateStorage folder in _prioritizer.EnumerateFolders())
            {
                Template template = _fetcher.FetchTemplateFromFolder(definition, folder);
                if (template != null)
                {
                    return template;
                }
            }

            Template newTemplate = _creator.CreateTemplate(definition);

            _prioritizer.EnumerateFolders()[0].Templates.Add(newTemplate);
            return newTemplate;
        }
    }

    public interface ITemplateFetcher
    {
    }
}