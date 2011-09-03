
using SharpArch.Domain.Commands;

namespace TemplateProject.Tasks.Commands
{
    public class MassCategoryChangeResult : CommandResult
    {
        public MassCategoryChangeResult(bool success) : base(success)
        {
        }
    }
}
