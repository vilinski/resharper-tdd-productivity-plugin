using JetBrains.ReSharper.Psi.Tree;

namespace TddProductivity.MoveClass
{
    public interface IElementFinder
    {
        IElement GetElementAtCaret();
    }
}