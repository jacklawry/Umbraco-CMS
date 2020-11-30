using System;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Configuration;
using Umbraco.Core.Exceptions;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations.Install;
using Umbraco.Core.Services;
using Umbraco.Web.Install;
using Umbraco.Web.Security;

namespace Umbraco.Web.Compose
{
    public class UnattendedInstallComponent : IComponent
    {
        private readonly IRuntimeState _runtimeState;
        private readonly ILogger _logger;
        private readonly DatabaseBuilder _databaseBuilder;

        public UnattendedInstallComponent(IRuntimeState runtimeState, ILogger logger, DatabaseBuilder databaseBuilder)
        {
            _runtimeState = runtimeState ?? throw new ArgumentNullException(nameof(runtimeState));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _databaseBuilder = databaseBuilder ?? throw new ArgumentNullException(nameof(databaseBuilder));
        }

        public void Initialize()
        {
            // check if we are doing an unattended install
            if (_runtimeState.Reason != RuntimeLevelReason.InstallEmptyDatabase) return;

            _logger.Info<UnattendedInstallComponent>("Installing Umbraco.");
            var result = _databaseBuilder.CreateSchemaAndData();
            _logger.Info<UnattendedInstallComponent>("Umbraco Installed.");

            if (result.Success == false)
                throw new InstallException("An error occurred while running the unattended installation.\n" + result.Message);

            // the install was successful, lets restart the application to get Umbraco to boot the correct state
            UmbracoApplication.Restart();
        }

        public void Terminate()
        {
        }
    }
}
