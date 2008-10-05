using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.TextControl;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TddProductivity.MoveClass;
using Rhino.Mocks;

namespace TddProductivity.Tests
{
    public class ElementFinderSpecs:SpecBase<ElementFinder>
    {
        private ISolution _solution;
        private ITextControl _textControl;
        private PsiManager _psiManager;
        private DocumentManager _documentManager;

        protected override ElementFinder SetupSUT()
        {
            _solution = CreateDependency<ISolution>();
            _textControl = CreateDependency<ITextControl>();
            _psiManager = CreateDependency<PsiManager>();
            _documentManager = CreateDependency<DocumentManager>();

            return new ElementFinder(_solution,_textControl,_documentManager,_psiManager);
        }

        [Test]
        public void Should_find_an_element()
        {
            _documentManager.Stub(d => d.GetProjectFile(null)).IgnoreArguments().Return(CreateDependency<IProjectFile>());

            var element = SUT.GetElementAtCaret();
            
            Assert.That(element,Is.Not.Null);
        }
    }
}
