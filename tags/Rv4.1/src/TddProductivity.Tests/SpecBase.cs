using NUnit.Framework;
using Rhino.Mocks;

namespace TddProductivity.Tests
{
    [TestFixture]
    public abstract class SpecBase<T>
    {
        protected T SUT        { get; private set;        }

        [SetUp]
        public void Setup()
        {
            SUT = SetupSUT();
        }

#pragma warning disable 693
        protected T CreateDependency<T>() where T : class
#pragma warning restore 693
        {
            return MockRepository.GenerateStub<T>();
        }


        protected abstract T SetupSUT();
    }
}