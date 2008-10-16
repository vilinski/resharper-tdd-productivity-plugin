using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.TextControl;

namespace TddProductivity.MoveClass
{
    public class ActionChainProcessor : IBulbItem
    {
        private readonly List<IBulbItem> _items;
        private readonly string _itemText;

        public ActionChainProcessor(IBulbItem[] items, string itemText)
        {
            _items = new List<IBulbItem>(items);
            _itemText = itemText;
        }

        #region IBulbItem Members

        public void Execute(ISolution solution, ITextControl textControl)
        {
            //_items.AddRange(i.Items);
            foreach (IBulbItem bulbItem in _items)
            {
                bulbItem.Execute(solution, textControl);
            }
        }

        public string Text
        {
            get { return _itemText; }
        }

        #endregion
    }
}