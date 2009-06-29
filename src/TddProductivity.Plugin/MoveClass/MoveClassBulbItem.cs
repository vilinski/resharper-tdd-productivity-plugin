using System;
using System.IO;
using JetBrains.ActionManagement;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.CSharp.ContextActions;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Refactorings.MoveTopLevelClass;
using JetBrains.ReSharper.Refactorings.MoveTopLevelClass.DataProviders;
using JetBrains.ReSharper.Refactorings.Workflow;
using JetBrains.TextControl;

namespace TddProductivity.MoveClass
{
    public class MoveClassBulbItem : IBulbItem
    {
        private readonly ICSharpTypeDeclaration _sourceTypeDeclaration;
        private readonly IProject _destinationProject;

        public MoveClassBulbItem([NotNull] ICSharpTypeDeclaration sourceTypeDeclaration, IProject destinationProject)
        {
            if (sourceTypeDeclaration == null) throw new ArgumentNullException("typeDeclaration");
            _sourceTypeDeclaration = sourceTypeDeclaration;
            _destinationProject = destinationProject;
        }

        #region IBulbItem Members

        public void Execute(ISolution solution, ITextControl textControl)
        {
            using (ReadLockCookie.Create())
            {
                var manager = PsiManager.GetInstance(solution);
                manager.CommitAllDocuments();

                IProjectItem parentFolder = GetParentFolder(_sourceTypeDeclaration);
                MoveToOuterFile();

                IProjectItem sourceFile = GetSourceFile(parentFolder, _sourceTypeDeclaration);
                IProjectFile newFile = MoveFileToProject(sourceFile);
                OpenFileInEditor(solution, newFile);
                sourceFile.Remove();
            }
        }

        private void MoveToOuterFile()
        {
            var declaredElement = _sourceTypeDeclaration.DeclaredElement;
            var solution = _sourceTypeDeclaration.GetSolution();

            var newName = RenameFileToMatchTypeNameAction.GetFileName(declaredElement, _sourceTypeDeclaration.GetProjectFile());
            if (newName == null) return;

            var dataProvider = new MoveMoFileMoveTypeDataProvider(newName, null, new[] { _sourceTypeDeclaration });
            var context = new DataContext();
            context.SetData(MoveTypeWorkflow.DATA_PROVIDER, dataProvider);
            context.SetData(DataConstants.SOLUTION, solution);
            context.SetData(JetBrains.ReSharper.Psi.Services.DataConstants.PSI_LANGUAGE_TYPE, _sourceTypeDeclaration.Language);
            context.SetData(JetBrains.ReSharper.Psi.Services.DataConstants.DECLARED_ELEMENT, declaredElement);

            var workflow = new MoveTypeWorkflow(solution);
            RefactoringActionUtil.ExecuteRefactoring(context, workflow);
        }

        string IBulbItem.Text
        {
            get { return "Move class to " + _destinationProject.Name; }
        }

        #endregion

        protected virtual void OpenFileInEditor(ISolution solution, IProjectFile newFile)
        {
            EditorManager editor = EditorManager.GetInstance(solution);
            editor.OpenFile(newFile.Location.FullPath, true, false);
        }

        protected virtual IProjectFile MoveFileToProject([NotNull] IProjectItem sourceFile)
        {
            if (sourceFile == null) throw new ArgumentNullException("sourceFile");
            string destFileName = _destinationProject.Location.Combine(sourceFile.Location.Name).FullPath;

            if (!File.Exists(destFileName)) File.Move(sourceFile.Location.FullPath, destFileName);
            return _destinationProject.CreateFile(_destinationProject.Location.Combine(sourceFile.Location.Name));
        }

        private static IProjectItem GetSourceFile(IProjectItem parentFolder, IDeclaration typeDeclaration)
        {
            string classname = typeDeclaration.DeclaredName;
            return parentFolder.GetSubItem(classname + ".cs");
        }

        private IProjectItem GetParentFolder(IElement element)
        {
            IProjectFile file = element.GetProjectFile();
            if (file == null) throw new NullReferenceException("Unable to get Project File from Element");
            return file.ParentFolder;
        }
    }
}