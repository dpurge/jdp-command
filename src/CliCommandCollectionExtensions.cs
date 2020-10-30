namespace Microsoft.Extensions.DependencyInjection
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.Linq;

    using jdp.command;

    public static class CliCommandCollectionExtensions
    {
        public static IServiceCollection AddCliCommands(this IServiceCollection services)
        {
            Type commandType = typeof(Command);
			string commandNamespace = "jdp.command";

            IEnumerable<Type> commands = typeof(JdpCommand).Assembly.GetExportedTypes()
                .Where(x => x.Namespace == commandNamespace && commandType.IsAssignableFrom(x));

            foreach (Type command in commands)
            {
                services.AddSingleton(commandType, command);
            }

            services.AddSingleton(serviceProvider =>
            {
                var cfg = serviceProvider.GetRequiredService<IConfiguration>().Get<IConfigurationRoot>();
                return cfg;
            });

            return services;
        }
    }
}