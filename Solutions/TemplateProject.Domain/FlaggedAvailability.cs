using System;

namespace TemplateProject.Domain
{
    [Flags]
    public enum FlaggedAvailability
    {
        Online = 1,
        Store = 2,
        ThirdParty = 4
    }
}
