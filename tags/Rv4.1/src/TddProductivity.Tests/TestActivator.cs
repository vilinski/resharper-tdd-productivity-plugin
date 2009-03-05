using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Psi.Resolve;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace TddProductivity.Tests
{
    public class ActivatorSpecs : SpecBase<Activator>
    {

        protected override Activator SetupSUT()
        {
            return new Activator();
        }

        [Test]
        public void Should_create_createClassClassFromNewFix()
        {
            IQuickFix fix = Activator.CreateCreateClassFix(new NotResolvedError(MockRepository.GenerateMock<IReference>()));
            Assert.That(fix, Is.InstanceOfType(typeof(IQuickFix)));
        }
    }
}
