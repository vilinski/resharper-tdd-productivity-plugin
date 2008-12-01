using JetBrains.ReSharper.LiveTemplates.Templates;

namespace TddProductivity.Templates
{
    public interface IDefaultTemplateCreator
    {
        Template CreateTemplate(TemplateDefinition definition);
    }
}