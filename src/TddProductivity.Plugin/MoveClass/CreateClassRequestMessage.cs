using JetBrains.ProjectModel;

namespace TddProductivity.MoveClass
{
    public class CreateClassRequestMessage
    {
        public string Template;
        public string Classname { get; set; }

        public string Namespace { get; set; }

        public IProjectFolder Project { get; set; }
    }
}