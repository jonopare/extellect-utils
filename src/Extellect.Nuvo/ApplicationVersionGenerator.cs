using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

namespace Extellect.Nuvo
{
    public static class ApplicationVersionGenerator
    {
        public static void Generate(string solutionDirectory, string outputFileName, int year, bool removeVersionFromCsproj)
        {
            var path = Path.Combine(solutionDirectory, outputFileName);
            var oldText = File.ReadAllText(path);

            var generator = new VersionGenerator(new DateTime(year, 1, 1));

            var versionRegex = new Regex(@"public const string Version = ""(?<version>\d+\.\d+\.\d+\.\d+)""");
            var versionMatch = versionRegex.Matches(oldText).Single();
            var oldVersionText = versionMatch.Groups["version"].Value;
            var oldVersion = Version.Parse(oldVersionText);
            var newVersion = generator.Generate((short)oldVersion.Major, (short)oldVersion.Minor);
            var newVersionLine = versionMatch.Value.Replace(oldVersionText, newVersion.ToString());

            var buildDateRegex = new Regex(@"public const long BuildDate = (?<buildDate>\d+)L");
            var buildDateMatch = buildDateRegex.Matches(oldText).Single();
            var oldBuildDateText = buildDateMatch.Groups["buildDate"].Value;
            var newBuildDateText = DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture);
            var newBuildDateLine = buildDateMatch.Value.Replace(oldBuildDateText, newBuildDateText);

            var newText = oldText.Replace(versionMatch.Value, newVersionLine)
                .Replace(buildDateMatch.Value, newBuildDateLine);

            File.WriteAllText(path, newText);

            foreach (var csproj in Directory.GetFiles(solutionDirectory, "*.csproj", SearchOption.AllDirectories))
            {
                var xml = new XmlDocument();
                xml.Load(csproj);

                var foundSolutionInfoCs = false;
                var foundSolutionVersionInfoCs = false;
                foreach (XmlElement compileElement in xml.SelectNodes("Project/ItemGroup/Compile")!)
                {
                    var include = compileElement.Attributes["Include"]?.Value;
                    var link = compileElement.Attributes["Link"]?.Value;
                    if (include != null)
                    {
                        switch (Path.GetFileName(include))
                        {
                            case "SolutionInfo.cs":
                                Debug.Assert(link == @"Properties\SolutionInfo.cs");
                                foundSolutionInfoCs = true;
                                break;
                            case "SolutionVersionInfo.cs":
                                Debug.Assert(link == @"Properties\SolutionVersionInfo.cs");
                                foundSolutionVersionInfoCs = true;
                                break;
                        }
                        if (foundSolutionInfoCs && foundSolutionVersionInfoCs)
                            break;
                    }
                }

                if (!foundSolutionInfoCs || !foundSolutionVersionInfoCs)
                    Console.Error.WriteLine($"Project is missing at least one shared assembly attribute: {csproj}");

                if (removeVersionFromCsproj)
                {
                    var versionElement = (XmlElement)xml.SelectSingleNode("Project/PropertyGroup/Version")!;
                    if (versionElement != null)
                    {
                        versionElement.ParentNode!.RemoveChild(versionElement);
                        xml.Save(csproj);
                        Console.Error.WriteLine($"Project should not redefine version: {csproj}");
                    }
                }
            }
        }
    }
}
