using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;

namespace TddProductivity.MoveClass
{
    public class ElementFinder : IElementFinder
    {
        private readonly DocumentManager _documentManager;
        private readonly PsiManager _psiManager;
        private readonly ISolution _solution;
        private readonly ITextControl _textControl;

        public ElementFinder(ISolution solution, ITextControl textControl)
        {
            _solution = solution;
            _textControl = textControl;
            _documentManager = DocumentManager.GetInstance(_solution);
            _psiManager = PsiManager.GetInstance(_solution);
        }

        public ElementFinder(ISolution solution, ITextControl textControl, DocumentManager documentManager,
                             PsiManager psiManager)

        {
            _solution = solution;
            _textControl = textControl;
            _documentManager = documentManager;
            _psiManager = psiManager;
        }

        #region IElementFinder Members

        public IElement GetElementAtCaret()
        {
            IProjectFile projectFile = _documentManager.GetProjectFile(_textControl.Document);
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

            var element = file.FindTokenAt(_textControl.CaretModel.Offset);

            if (element is JetBrains.ReSharper.Psi.CSharp.Tree.IWhitespaceNode)
                element = file.FindTokenAt(_textControl.CaretModel.Offset-1);
            
            return element;
        }

        #endregion
    }
}