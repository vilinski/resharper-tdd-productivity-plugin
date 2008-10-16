using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.TextControl;

namespace TddProductivity.MoveClass
{
    public class BulbItem : IBulbItem
    {
        private readonly string _text;
        private readonly CreateClassAction action;

        public BulbItem(string text, CreateClassAction action)
        {
            _text = text;
            this.action = action;
        }

        #region IBulbItem Members

        public void Execute(ISolution solution, ITextControl textControl)
        {
            action.Execute();
        }

        public string Text
        {
            get { return _text; }
        }

        #endregion
    }
}