﻿using BoDi;
using MTMGeneratorProvider.Generator.SpecFlowPlugin;
using TechTalk.SpecFlow.Generator.Configuration;
using TechTalk.SpecFlow.Generator.Plugins;
using TechTalk.SpecFlow.Generator.UnitTestProvider;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.UnitTestProvider;
using TechTalk.SpecFlow.Utils;

[assembly: GeneratorPlugin(typeof(MTMGeneratorPlugin))]

namespace MTMGeneratorProvider.Generator.SpecFlowPlugin
{
    /// <summary>
    /// The CodedUI generator plugin.
    /// </summary>
    /// 
    public class MTMGeneratorPlugin : IGeneratorPlugin
    {
        /// <summary>
        /// The register dependencies.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public void RegisterDependencies(ObjectContainer container)
        {
        }

        /// <summary>
        /// The register customizations.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="generatorConfiguration">
        /// The generator configuration.
        /// </param>
        public void RegisterCustomizations(ObjectContainer container, SpecFlowProjectConfiguration generatorConfiguration)
        {
            container.RegisterTypeAs<MTMGeneratorProvider, IUnitTestGeneratorProvider>();
            container.RegisterTypeAs<MsTest2010RuntimeProvider, IUnitTestRuntimeProvider>();
        }

        /// <summary>
        /// The register configuration defaults.
        /// </summary>
        /// <param name="specFlowConfiguration">
        /// The spec flow configuration.
        /// </param>
        public void RegisterConfigurationDefaults(SpecFlowProjectConfiguration specFlowConfiguration)
        {
        }
    }
}
