using System;
using System.IO;
using JetBrains.Application;
using JetBrains.DocumentModel;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Intentions.CSharp.ContextActions;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;

namespace TddProductivity.MoveClass
{
    public class MoveClassBulbItem : IBulbItem
    {
        private readonly MoveTypeToAnotherFileAction _action;
        private readonly IProject _project;


        public MoveClassBulbItem(IProject project, MoveTypeToAnotherFileAction action) 
        {
            _project = project;
            _action = action;
        }

        public void Execute(ISolution solution, ITextControl textControl)
        {
            ElementFinder elementFinder = new ElementFinder(solution,textControl);
            IElement element = elementFinder.GetElementAtCaret();
            
            if (element == null)
                throw new InvalidOperationException();

            string classname = element.GetText();

            IProjectFolder parentFolder = element.GetProjectFile().ParentFolder;

            _action.Execute(solution, textControl);

            Shell.Instance.Locks.AssertReadAccessAllowed();


            IProjectItem sourceFile = parentFolder.GetSubItem(classname + ".cs");

            string destFileName = _project.Location.Combine(sourceFile.Location.Name).FullPath;

            if (!File.Exists(destFileName))
            {
                File.Move(sourceFile.Location.FullPath, destFileName);
            }

            IProjectFile newFile = _project.CreateFile(_project.Location.Combine(sourceFile.Location.Name));

            EditorManager editor = EditorManager.GetInstance(solution);
            ITextControl textcontrol = editor.OpenFile(newFile.Location.FullPath, true, false);
            sourceFile.Remove();
        }

        string IBulbItem.Text
        {
            get { return "Move class to " + _project.Name; }
        }
   }
}