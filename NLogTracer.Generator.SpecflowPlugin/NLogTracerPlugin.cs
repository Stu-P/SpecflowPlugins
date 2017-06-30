using TechTalk.SpecFlow.Infrastructure;
using BoDi;
using TechTalk.SpecFlow.Configuration;
using TechTalk.SpecFlow.Tracing;
using NLogTracer.SpecflowPlugin;
using NLog;
using System;

[assembly: RuntimePlugin(typeof(NLogTracerPlugin))]
namespace NLogTracer.SpecflowPlugin
{
    public class NLogTracerPlugin : IRuntimePlugin
    {




        public void RegisterCustomizations(ObjectContainer container, RuntimeConfiguration runtimeConfiguration)
        {
            container.RegisterTypeAs<NLogTracer, ITraceListener>();
        }

        public void RegisterConfigurationDefaults(RuntimeConfiguration runtimeConfiguration)
        {

        }

        public void RegisterDependencies(ObjectContainer container)
        {
        }
    }

    public class NLogTracer : ITraceListener
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void WriteTestOutput(string message)
        {
            logger.Info(message);
        }

        public void WriteToolOutput(string message)
        {
            logger.Info(message);
        }
    }
}
