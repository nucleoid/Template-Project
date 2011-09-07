using System;

namespace TemplateProject.Domain
{
    [Flags]
    public enum FlaggedAvailability
    {
        Online = 0,
        Store = 1,
        ThirdParty = 2
    }
}
