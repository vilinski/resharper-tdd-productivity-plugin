using System.IO;
using System.Reflection;
using System.Xml;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.FileTemplates;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Storages;
using JetBrains.ReSharper.LiveTemplates.Templates;

namespace TddProductivity.Templates
{
    public class DefaultTemplateCreator : IDefaultTemplateCreator
    {
        

        public Template CreateTemplate(TemplateDefinition definition)
        {
            var reader =
                new StreamReader(
                    Assembly.GetExecutingAssembly().GetManifestResourceStream(
                        "TddProductivity.Resources.Templates." + definition.Name + ".xml"));

            var document = new XmlDocument();
            document.Load(reader);
            ITemplateStorage folder = GetOrCreateTestDriveFolder();

            Template template = Template.CreateFromXml(document.DocumentElement);
            folder.Templates.Add(template);


            return template;
        }

        
        private static ITemplateStorage GetOrCreateTestDriveFolder()
        {
            return FileTemplatesManager.Instance.TemplateFamily.UserStorage;
        }
    }
}