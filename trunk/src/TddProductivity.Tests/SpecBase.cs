using NUnit.Framework;
using Rhino.Mocks;

namespace TddProductivity.Tests
{
    [TestFixture]
    public abstract class SpecBase<T>
    {
        protected T SUT        { get; private set;        }
        protected MockRepository mockRepository;

        [SetUp]
        public void Setup()
        {
            mockRepository= new MockRepository();
            SUT = SetupSUT();
        }

#pragma warning disable 693
        protected T CreateDependency<T>()
#pragma warning restore 693
        {
            return mockRepository.CreateMock<T>();
        }


        protected abstract T SetupSUT();
    }
}