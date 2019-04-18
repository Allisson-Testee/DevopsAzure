﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace WpfCoreApp.Telemetry
{
    class AppVersionTelemetryInitializer : ITelemetryInitializer
    {
        private readonly string _wpfVersion;        
        private readonly string _appVersion;

        public AppVersionTelemetryInitializer()
        {
            _wpfVersion = typeof(System.Windows.Application).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;            
            _appVersion = typeof(DiagnosticsClient).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
                                                           .FirstOrDefault(ama => string.Equals(ama.Key, "CloudBuildNumber", StringComparison.OrdinalIgnoreCase))
                                                           ?.Value;
        }

        public void Initialize(ITelemetry telemetry)
        {            
            telemetry.Context.GlobalProperties["WPF version"] = _wpfVersion;            
            telemetry.Context.Component.Version = _appVersion;
        }
    }

}
