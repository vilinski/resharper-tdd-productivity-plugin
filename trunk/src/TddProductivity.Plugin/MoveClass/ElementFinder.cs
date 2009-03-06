using System;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Intentions.CSharp.ContextActions;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;

namespace TddProductivity.MoveClass
{
    public class ElementFinder : IElementFinder
    {
        private readonly ICSharpContextActionDataProvider _provider;
        private readonly DocumentManager _documentManager;
        private readonly PsiManager _psiManager;
        //private readonly ISolution _solution;
        //private readonly ITextControl _textControl;

        public ElementFinder(ICSharpContextActionDataProvider provider)
        {
            _provider = provider;
            //_solution = solution;
            //_textControl = textControl;
            _documentManager = DocumentManager.GetInstance(provider.Solution);
            _psiManager = PsiManager.GetInstance(provider.Solution);
        }

        public ElementFinder(ISolution solution, ITextControl textControl, DocumentManager documentManager,
                             PsiManager psiManager)
        {
            //_solution = solution;
            //_textControl = textControl;
            _documentManager = documentManager;
            _psiManager = psiManager;
        }

        #region IElementFinder Members

        public IElement GetElementAtCaret()
        {
            try
            {
                IProjectFile projectFile = _documentManager.GetProjectFile(_provider.TextControl.Document);
                if (projectFile == null)
                {
                    return null;
                }

                if (_psiManager == null)
                {
                    return null;
                }

                var file = _psiManager.GetPsiFile(projectFile) as ICSharpFile;
                if (file == null)
                {
                    return null;
                }

                var element = file.FindTokenAt(_provider.TextControl.CaretModel.Offset);

                if (element is JetBrains.ReSharper.Psi.CSharp.Tree.IWhitespaceNode)
                    element = file.FindTokenAt(_provider.TextControl.CaretModel.Offset - 1);

                return element;
            }catch
            {
                ;
            }
            return null;
        }

        #endregion
    }
}