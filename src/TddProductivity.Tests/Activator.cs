using System;
using System.Reflection;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;

namespace TddProductivity
{
    public class Activator
    {
        public static IQuickFix CreateCreateClassFix(NotResolvedError error)
        {
            Type type =
                Type.GetType(
                    "JetBrains.ReSharper.Intentions.CSharp.QuickFixes.CreateClassFromNewFix,JetBrains.ReSharper.Intentions.CSharp");
            ConstructorInfo ci =
                type.GetConstructor(new[] {typeof (NotResolvedError)});


            object instance = ci.Invoke(new object[] {error});


            return instance as IQuickFix;
        }
    }
}