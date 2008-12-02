using System;
using System.IO;
using JetBrains.Application;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;

namespace TddProductivity.MoveClass
{
    public class MoveClassBulbItem : IBulbItem
    {
        private readonly IBulbItem _action;
        private readonly IElementFinder _elementFinder;
        private readonly IProject _project;


        public MoveClassBulbItem(IProject project, IBulbItem action, IElementFinder elementFinder)
        {
            _project = project;
            _action = action;
            _elementFinder = elementFinder;
        }

        #region IBulbItem Members

        public void Execute(ISolution solution, ITextControl textControl)
        {
            IElement element = _elementFinder.GetElementAtCaret();
            if (element == null)
                throw new InvalidOperationException();
            IProjectItem parentFolder = GetParentFolder(element);
            _action.Execute(solution, textControl);
            AssertReadAccess();
            IProjectItem sourceFile = GetSourceFile(parentFolder,element);
            IProjectFile newFile = MoveFileToProject(sourceFile);
            OpenFileInEditor(solution, newFile);
            sourceFile.Remove();
        }

        string IBulbItem.Text
        {
            get { return "Move class to " + _project.Name; }
        }

        #endregion

        public virtual void AssertReadAccess()
        {
            Shell.Instance.Locks.AssertReadAccessAllowed();
        }

        public virtual void OpenFileInEditor(ISolution solution, IProjectFile newFile)
        {
            EditorManager editor = EditorManager.GetInstance(solution);
            ITextControl textcontrol = editor.OpenFile(newFile.Location.FullPath, true, false);
        }

        public virtual IProjectFile MoveFileToProject(IProjectItem sourceFile)
        {
            string destFileName = _project.Location.Combine(sourceFile.Location.Name).FullPath;

            if (!File.Exists(destFileName))
            {
                File.Move(sourceFile.Location.FullPath, destFileName);
            }

            return _project.CreateFile(_project.Location.Combine(sourceFile.Location.Name));
        }

        private static IProjectItem GetSourceFile(IProjectItem parentFolder, IElement element)
        {
            string classname = element.GetText();
            return parentFolder.GetSubItem(classname + ".cs");
        }

        public virtual IProjectItem GetSourceFile(IElement element)
        {
            return GetSourceFile(GetParentFolder(element), element);
        }
        public virtual IProjectItem GetParentFolder(IElement element)
        {
            IProjectFile file = element.GetProjectFile();
            if (file==null)
            {
                throw new NullReferenceException("Unable to get Project File from Element");
            }
            return file.ParentFolder;
        }
    }
}