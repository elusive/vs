namespace vs
{
    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Configuration;

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;

    [Command(Name = "Visual Studio Driver",
        Description = "Opens a solution or directory in Visual Studio from the command line.",
        ExtendedHelpText = @"
Arguments are passed to devenv.exe:
  The first argument for devenv is usually a solution file, project file or a folder.
You can also use any other file as the first argument if you want to have the
file open automatically in an editor. When you enter a project file, the IDE
looks for an .sln file with the same base name as the project file in the
parent directory for the project file. If no such .sln file exists, then the
IDE looks for a single .sln file that references the project. If no such single
.sln file exists, then the IDE creates an unsaved solution with a default .sln
file name that has the same base name as the project file.
")]
    [HelpOption("-?|-h|--help")]
    public class Program
    {
        public const string SolutionFileType = "sln";
        public const string ProjectFileType = "csproj";

        public static IConfigurationRoot Configuration { get; private set; }

        /// <summary>
        /// Main method
        /// </summary>
        public static int Main(string[] args) =>
            CommandLineApplication.Execute<Program>(args);

        /// <summary>
        /// Gets the path provided for vs to open.
        /// </summary>
        [Argument(0, Description = "Path to the solution root.")]
        public string Path { get; }


        private int OnExecute(CommandLineApplication app, CancellationToken token = default)
        {
            try
            {
                // load configuration from json
                Configuration = new ConfigurationBuilder()
                    .AddJsonFile("appSettings.json")
                    .Build();

                // output provided value
                LogTrace(TraceLevel.Verbose, $"Path entered as: {Path}");
                if (string.IsNullOrEmpty(Path)) app.ShowHelp();

                // resolve path
                var path = System.IO.Path.GetFullPath(Path);
                LogTrace(TraceLevel.Info, $"Path resolved: {path}");

                // look for solution file or project file
                if (Helper.IsVisualStudioFile(path))
                {
                    var cmd = Helper.GetVsCommand();
                    Process.Start(cmd, path);
                    LogTrace(TraceLevel.Info, $"Executing {cmd} with parameter {path}");
                    return 0;
                }

                // no file so check directory
                if (Directory.Exists(path))
                {
                    // find a solution file to open
                    var sln = Directory.GetFiles(
                        path,
                        $"*.{SolutionFileType}",
                        SearchOption.TopDirectoryOnly)
                        .FirstOrDefault();
                    LogTrace(TraceLevel.Verbose, $"Solution file: {sln}");
                    if (sln != null)
                    {
                        var cmd = Helper.GetVsCommand();
                        Process.Start(cmd, sln);
                        LogTrace(TraceLevel.Info, $"Executing {cmd} with parameter {path}");
                        return 0;
                    }

                    // find a project file in the path -R
                    var proj = Directory.GetFiles(
                        path,
                        $"*.{ ProjectFileType}",
                        SearchOption.AllDirectories)
                        .FirstOrDefault();
                    LogTrace(TraceLevel.Verbose, $"Project file: {proj}");
                    if (proj != null)
                    {
                        var cmd = Helper.GetVsCommand();
                        Process.Start(cmd, proj);
                        LogTrace(TraceLevel.Info, $"Executing {cmd} with parameter {path}");
                        return 0;
                    }
                    
                    LogTrace(TraceLevel.Info, "Failed to find an sln file in this folder.");
                    app.ShowHelp();
                    return -1;
                }

                // failed
                LogTrace(TraceLevel.Info, "Failed to launch Visual Studio.");
                return -1; 
            }
            catch (Exception ex)
            {
                LogTrace(TraceLevel.Info, ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// Property types of ValueTuple{bool,T} translate to
        /// CommandOptionType.SingleOrNoValue
        /// Input            | Value
        /// ------------------------------------------------
        /// (none)           | (false, default(TraceLevel))
        /// --trace          | (true, TraceLevel.Normal)
        /// --trace:normal   | (true, TraceLevel.Normal)
        /// --trace:verbose  | (true, TraceLevel.Verbose)
        /// </summary>
        [Option]
        public (bool HasValue, TraceLevel level) Trace { get; }

        public enum TraceLevel
        {
            Info = 0,
            Verbose,
        }



        private void LogTrace(TraceLevel level, string message)
        {
            if (!Trace.HasValue) return;
            if (Trace.level >= level)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"{level}: {message}");
                Console.ResetColor();
            }
        }
    }
}
