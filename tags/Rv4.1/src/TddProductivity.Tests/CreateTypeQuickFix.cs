using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace TddProductivity.Tests
{
    public class ClassNameSpecs : SpecBase<ClassName>
    {

        protected override ClassName SetupSUT()
        {
            return new ClassName();
        }

        [Test]
        public void Should_MehodName()
        {
            End
        }
    }
}
