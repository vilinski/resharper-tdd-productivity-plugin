using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
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
            IProjectFile projectFile = CreateDependency<IProjectFile>();
            _textControl.Stub(t => t.Document).Return(CreateDependency<IDocument>()).Repeat.Any();
            _documentManager.Stub(d => d.GetProjectFile(_textControl.Document)).Return(projectFile);
            IFile file = CreateDependency<ICSharpFile>();
            _psiManager.Stub(p => p.GetPsiFile(projectFile)).Return(file);
            file.Stub(f => f.FindTokenAt(0)).IgnoreArguments().Return(CreateDependency<IElement>());
            _textControl.Stub(t => t.CaretModel).Return(CreateDependency<ICaretModel>());
            var element = SUT.GetElementAtCaret();
            
            Assert.That(element,Is.Not.Null);
        }
    }
}
