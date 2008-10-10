using JetBrains.ReSharper.CodeInsight.Services.Intentions;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.QuickFixes.Test;
using JetBrains.Util;

namespace TddProductivity.MoveClass
{
    [Intention("CSHARP", "CreateClass")]
    public class CreateClassQuickFix:IQuickFix
    {
        private readonly NotResolvedError _error;
        private IReference reference;

        public CreateClassQuickFix(NotResolvedError error)
        {
            _error = error;
            reference = error.Reference;
            this.ReferenceName = reference.GetElement() as IReferenceName;

        }

        private IReferenceName ReferenceName { get; set; }

        public bool IsAvailable(IUserDataHolder cache)
        {
            
            IReferenceName qualifier = ReferenceName.Qualifier;
            if (qualifier == null)
            {
                return (ReferenceName.GetContainingFile() != null);
            }
            IDeclaredElement declaredElement = qualifier.Reference.Resolve().DeclaredElement;
            if (declaredElement == null)
            {
                return false;
            }
            if (declaredElement is INamespace)
            {
                return true;
            }
            IExternAlias externAlias = declaredElement as IExternAlias;
            return ((externAlias != null) && (externAlias.Module == qualifier.GetProject()));
        }

        public IBulbItem[] Items
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}