using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml;

namespace Extellect.Nuvo
{
    public class DeploymentPackager
    {
        public void Package(Version version, string projectDirectory, string spaDirectoryName, string pubxmlPath, string outputDirectory, string outputFilePrefix)
        {
            var prompt = new Prompt(Console.Out, Console.In);

            // TODO: perform the following steps automatically in future (but it's easier to do manually for now)

            if (!prompt.Ask("Have you generated a new version?", false).Value)
                throw new Exception("Generate a new version before continuing");

            if (!prompt.Ask("Have you published the web API?", false).Value)
                throw new Exception("Publish the web API before continuing");

            if (!prompt.Ask("Have you built NG with base-href?", false).Value)
                throw new Exception("Build NG before continuing");

            var ngBuildConfiguration = prompt.Ask("NG build configuration", ["production", "staging"]);
            if (!ngBuildConfiguration.HasValue)
                throw new Exception("NG build configuguration is required");

            if (!Path.IsPathRooted(pubxmlPath))
                pubxmlPath = Path.Combine(projectDirectory, pubxmlPath);

            if (!File.Exists(pubxmlPath))
                throw new Exception("Web API hasn't been published");

            var xml = new XmlDocument();
            xml.Load(pubxmlPath);
            var publishUrl = xml.SelectSingleNode("/Project/PropertyGroup/PublishUrl")!.InnerText;

            var dotnetPublish = Path.Combine(projectDirectory, publishUrl);
            var ngPublish = Path.Combine(projectDirectory, $@"{spaDirectoryName}\dist\{spaDirectoryName}-{ngBuildConfiguration.Value}\browser");

            var excludes = new List<Regex>
            {
                new Regex(@"^appsettings(\.[^.]+)?\.json$"),
                //new Regex(@"^publish.*\.zip"),
                new Regex(@"^\w{2}(-[^\\]+)?\\Microsoft\.Data\.SqlClient\.resources\.dll$"),
                new Regex($@"^{spaDirectoryName}\\.*"),
                new Regex(@"^wwwroot\\.*"),
            };

            var dotnets = Directory.GetFiles(dotnetPublish, "*", SearchOption.AllDirectories)
                .Select(path => Path.GetRelativePath(dotnetPublish, path))
                .Where(x => !excludes.Any(y => y.IsMatch(x)))
                .Select(relative => new { Relative = relative, Absolute = Path.Combine(dotnetPublish, relative) })
                .ToArray();

            var ngs = Directory.GetFiles(ngPublish, "*", SearchOption.AllDirectories)
                .Select(absolute => new { Relative = Path.GetRelativePath(ngPublish, absolute), Absolute = absolute })
                .ToArray();

            var zipPath = Path.Combine(outputDirectory, $"{outputFilePrefix}-{version}.zip");

            if (File.Exists(zipPath))
                throw new NotSupportedException($"The output file {zipPath} already exists");

            using var stream = new MemoryStream();
            using var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create);
            foreach (var dotnet in dotnets)
            {
                zipArchive.CreateEntryFromFile(dotnet.Absolute, dotnet.Relative, CompressionLevel.Optimal);
            }
            foreach (var ng in ngs)
            {
                zipArchive.CreateEntryFromFile(ng.Absolute, Path.Combine("wwwroot", ng.Relative), CompressionLevel.Optimal);
            }
            zipArchive.Dispose();
            var bytes = stream.ToArray();

            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            using var fileStream = File.Create(zipPath);
            fileStream.Write(bytes, 0, bytes.Length);

            Console.WriteLine($"Successfully compressed {dotnets.Length + ngs.Length} files to {zipPath}");
        }
    }
}
