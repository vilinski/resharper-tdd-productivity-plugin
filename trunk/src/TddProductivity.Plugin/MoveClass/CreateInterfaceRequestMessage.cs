using JetBrains.ProjectModel;

namespace TddProductivity.MoveClass
{
    public class CreateInterfaceRequestMessage
    {
        public string Interfacename { get; set; }

        public string Namespace { get; set; }

        public IProjectFolder Project { get; set; }
    }
}