using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Umbraco.Web.Compose
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Install, MaxLevel = RuntimeLevel.Install)]
    public class UnattendedInstallComposer : ComponentComposer<UnattendedInstallComponent>
    {
    }
}
