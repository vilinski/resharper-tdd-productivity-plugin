using System.IO;
using System.Reflection;
using System.Xml;
using JetBrains.ReSharper.LiveTemplates.FileTemplates;
using JetBrains.ReSharper.LiveTemplates.Storages;
using JetBrains.ReSharper.LiveTemplates.Templates;

namespace TddProductivity.Templates
{
    public class DefaultTemplateCreator : IDefaultTemplateCreator
    {
        #region IDefaultTemplateCreator Members

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

        #endregion

        private static ITemplateStorage GetOrCreateTestDriveFolder()
        {
            //ITemplateStorage storage = UserStorage.

            return FileTemplatesManager.Instance.TemplateFamily.UserStorage;
            //TemplateManager.Instance.FileTemplatesStorageGroup.UserStorage;
            //storage.
            //  foreach (ITemplateStorage folder in storage.SubFolders)
            //{
            //  if (folder.Name == TDConstants.TemplateFolderName)
            //  {
            //    return folder;
            //  }
            //}

            //  storage.Templates.
            //return storage.AddSubFolder(TDConstants.TemplateFolderName);
        }
    }

    public class TemplateDefinition
    {
        public string Name;
        public string TemplateName;
    }

    public interface IDefaultTemplateCreator
    {
        Template CreateTemplate(TemplateDefinition definition);
    }
}