using System.Diagnostics;
using System.Xml;

namespace Extellect.Nuvo
{
    public class PackagePublisher
    {
        public string BuildConfiguration { get; set; } = "Release";
        public string SolutionDirectory { get; set; } = string.Empty;
        public string NugetSourceDirectory { get; set; } = FileSystem.UserProfile(@"AppData\Roaming\Nuget\Local");
        public string PropsFileName { get; set; } = string.Empty;
        public bool IncludeSource { get; set; } = false;
        public bool IncludeSymbols { get; set; } = false;

        public void Publish(string solutionName, string[] projectNames)
        {
            if (string.IsNullOrEmpty(BuildConfiguration))
                throw new InvalidOperationException();
            if (string.IsNullOrEmpty(SolutionDirectory))
                throw new InvalidOperationException();
            if (string.IsNullOrEmpty(NugetSourceDirectory))
                throw new InvalidOperationException();
            if (string.IsNullOrEmpty(PropsFileName))
                throw new InvalidOperationException();

            if (!Directory.Exists(NugetSourceDirectory))
                Directory.CreateDirectory(NugetSourceDirectory);

            string getProjectFile(string project)
            {
                return Path.Combine(SolutionDirectory, project, project + ".csproj");
            }

            string escape(string path)
            {
                return "\"" + path.Replace("\"", "\"\"") + "\"";
            }

            var propsPath = Path.Combine(SolutionDirectory, PropsFileName);

            var props = new XmlDocument();
            props.Load(propsPath);
            var packageVersionElement = props.SelectSingleNode("/Project/PropertyGroup/PackageVersion")
                ?? throw new InvalidOperationException("PackageVersion element is missing");

            var oldVersion = SemanticVersion.Parse(packageVersionElement.InnerText);

            Console.WriteLine("How would you like to increment the version?");
            Console.WriteLine("0. No change: " + oldVersion + " [DEFAULT]");
            Console.WriteLine("1. Increment patch: " + oldVersion.Bump(SemanticVersionParts.Patch));
            Console.WriteLine("2. Increment minor: " + oldVersion.Bump(SemanticVersionParts.Minor));
            Console.WriteLine("3. Increment major: " + oldVersion.Bump(SemanticVersionParts.Major));

            var dictionary = new Dictionary<ConsoleKey, SemanticVersionParts?>
            {
                { ConsoleKey.Enter, null },
                { ConsoleKey.D0, null },
                { ConsoleKey.NumPad0, null },
                { ConsoleKey.D1, SemanticVersionParts.Patch },
                { ConsoleKey.NumPad1, SemanticVersionParts.Patch },
                { ConsoleKey.D2, SemanticVersionParts.Minor },
                { ConsoleKey.NumPad2, SemanticVersionParts.Minor },
                { ConsoleKey.D3, SemanticVersionParts.Major },
                { ConsoleKey.NumPad3, SemanticVersionParts.Major },
            };

            ConsoleKey key;
            SemanticVersionParts? bump;
            do
            {
                key = Console.ReadKey().Key;
            } while (!dictionary.TryGetValue(key, out bump));

            if (key != ConsoleKey.Enter)
                Console.WriteLine();
            Console.WriteLine($"You chose `{key}` (Bump = {bump})");

            SemanticVersion newVersion;
            if (bump != null)
            {
                newVersion = oldVersion.Bump(bump.Value);
                packageVersionElement.InnerText = newVersion.ToString();
                props.Save(propsPath);
            }
            else
            {
                newVersion = oldVersion;
            }

            string getNupkgFile(string project)
            {
                return Path.Combine(SolutionDirectory, project, "bin", BuildConfiguration, $"{solutionName}.{project}.{newVersion}.nupkg");
            }

            void copyNupkg(string nupkg)
            {
                if (!File.Exists(nupkg))
                    throw new InvalidOperationException();
                var target = Path.Combine(NugetSourceDirectory, Path.GetFileName(nupkg));
                if (File.Exists(target))
                    throw new InvalidOperationException("Version was already published: " + nupkg);
                File.Copy(nupkg, target);
            }

            foreach (var projectName in projectNames)
            {
                var args = new List<string>
                {
                    "pack",
                    escape(getProjectFile(projectName)),
                    "--configuration",
                    BuildConfiguration
                };

                if (IncludeSource)
                    args.Add("--include-source");

                if (IncludeSymbols)
                    args.Add("--include-symbols");

                var dependentProcess = Process.Start(new ProcessStartInfo("dotnet", string.Join(" ", args)))
                    ?? throw new InvalidOperationException();

                dependentProcess.WaitForExit();

                if (dependentProcess.ExitCode != 0)
                    throw new InvalidOperationException();

                copyNupkg(getNupkgFile(projectName));
            }
        }
    }
}
