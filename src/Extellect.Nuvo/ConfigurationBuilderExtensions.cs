using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Extellect.Nuvo
{
    public static class ConfigurationBuilderExtensions
    {
        public static void AddPerMachineConfig(this IConfigurationBuilder configurationBuilder, string machineName)
        {
            var jsonConfigurationSource = new JsonConfigurationSource();
            jsonConfigurationSource.Path = $"appsettings.{machineName}.json";
            jsonConfigurationSource.ReloadOnChange = true;
            jsonConfigurationSource.Optional = true;
            configurationBuilder.Add(jsonConfigurationSource);
        }
    }
}
