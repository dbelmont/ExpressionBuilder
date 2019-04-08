#if NETSTANDARD2_0
using Microsoft.Extensions.Configuration;
#endif
using System;
using System.Collections.Generic;
using System.Configuration;
using ExpressionBuilder.Common;

namespace ExpressionBuilder.Configuration
{
    public class Settings
    {
        public List<SupportedType> SupportedTypes { get; private set; }

        public static void LoadSettings(Settings settings)
        {
#if (NETSTANDARD2_0 || NETSTANDARD2_1 || NETSTANDARD2_2)

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json",
                    optional: true,
                    reloadOnChange: true);

            var _config = builder.Build();

            settings.SupportedTypes = new List<SupportedType>();
            foreach (var supportedType in _config.GetSection("supportedTypes").GetChildren())
            {
                var typeGroup = supportedType.GetValue<TypeGroup>("typeGroup");
                var type = Type.GetType(supportedType.GetValue<string>("Type"), false, true);
                if (type != null)
                {
                    settings.SupportedTypes.Add(new SupportedType { TypeGroup = typeGroup, Type = type });
                }
            }
#else
            var configSection = ConfigurationManager.GetSection(ExpressionBuilderConfig.SectionName) as ExpressionBuilderConfig;
            if (configSection == null)
            {
                return;
            }

            settings.SupportedTypes = new List<SupportedType>();
            foreach (ExpressionBuilderConfig.SupportedTypeElement supportedType in configSection.SupportedTypes)
            {
                Type type = Type.GetType(supportedType.Type, false, true);
                if (type != null)
                {
                    settings.SupportedTypes.Add(new SupportedType { TypeGroup = supportedType.TypeGroup, Type = type });
                }
            }
#endif
        }
    }
}