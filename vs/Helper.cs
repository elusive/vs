namespace vs
{
    using Microsoft.Extensions.Configuration;

    using System.Collections.Generic;
    using System.Linq;
    using System.IO;

    public class Helper
    {
        /// <summary>
        /// Use file extension to determine if the file is
        /// a solution file or a c# project file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns><c>True</c> if is VS file type, otherwise <c>False</c>.</returns>
        public static bool IsVisualStudioFile(string path)
        {
            if (!File.Exists(path)) return false;

            return Path.GetExtension(path) == Program.ProjectFileType
                || Path.GetExtension(path) == Program.SolutionFileType;
        }

        /// <summary>
        /// Reads app settings configuration and finds first existing executable
        /// for the Visual Studio ide.
        /// </summary>
        /// <returns>Path to existing Vusual Studio executable.</returns>
        public static string GetVsCommand()
        {
            // use configuration to find an existing visual studio exe
            var possibles = Program.Configuration.GetSection("VsConfig:VisualStudioExecutables").Get<List<string>>();
            return possibles.First(exe => File.Exists(exe));
        }
    }
}
