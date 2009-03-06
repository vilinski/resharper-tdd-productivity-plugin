using System;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.LiveTemplates;
using JetBrains.ReSharper.LiveTemplates.Execution;
using JetBrains.ReSharper.LiveTemplates.Templates;

namespace TddProductivity.Services.Templates.Impl
{
    public class TemplateFieldPopulator : ITemplateFieldPopulator
    {
        public void PopulateTemplate(Template template, string classUnderTest)
        {
            PopulateField(template, "CLASSUNDERTEST", classUnderTest);
            PopulateField(template, "FIELDUNDERTEST",
                          new CamelCaseUnderscoreStrategy().GetFieldName(classUnderTest));
        }

        private void PopulateField(Template template, string fieldName, string value)
        {
            TemplateField field = null;
            foreach (TemplateField templateField in template.Fields)
            {
                if (templateField.Name == fieldName)
                    field = templateField;
                break;
            }

            if (field == null) return;

            field.Expression = new StaticTemplateExpression(value);
        }

        #region Nested type: StaticTemplateExpression

        private class StaticTemplateExpression : IHotspotExpression
        {
            private readonly string _text;

            public StaticTemplateExpression(string text)
            {
                _text = text;
            }

            #region ITemplateExpression Members


            public string EvaluateQuickResult(IHotspotContext context)
            {
                return _text;
            }

            public bool HandleExpansion(IHotspotContext context)
            {
                return true;
            }

            public HotspotItems GetLookupItems(IHotspotContext context)
            {
                return HotspotItems.Empty;
            }

            public string Serialize()
            {
                return null;
            }

            public object Clone()
            {
                return new StaticTemplateExpression(_text);
            }

            #endregion
        }

        #endregion
    }

    internal class CamelCaseUnderscoreStrategy
    {
        public string GetFieldName(string name)
        {
            name = NameUtilities.GetClassName(name);
            return String.Format("_{0}{1}", Char.ToLowerInvariant(name[0]), name.Substring(1));
        }
    }


    public static class NameUtilities
    {
        private static readonly string[] TestSuffixes = new[] {".Tests", "Tests"};

        public static string GetNamespace(string fullClassName)
        {
            int lastDot = fullClassName.LastIndexOf('.');
            if (lastDot < 0) return String.Empty;

            return fullClassName.Substring(0, lastDot);
        }

        public static string GetClassName(string fullClassName)
        {
            int lastDot = fullClassName.LastIndexOf('.') + 1;

            return fullClassName.Remove(0, lastDot);
        }

        public static string RemoveTestSuffix(string fullClassName)
        {
            foreach (string suffix in TestSuffixes)
            {
                if (fullClassName.EndsWith(suffix))
                {
                    return fullClassName.Remove(fullClassName.Length - suffix.Length);
                }
            }

            return fullClassName;
        }
    }

    public class TestClass
    {
    }

    public class ClassUnderTest
    {
        public string Name;
    }

    public interface ITemplateFieldPopulator
    {
    }
}