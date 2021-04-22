using System;
using System.IO;

namespace Main
{
    public static class FileReader
    {
        private const string INPUT_FOLDER_NAME = "InputFiles";
        private const string UNIX_PREFIX = "Unix";
        private const string WINDOWS_PREFIX = "Microsoft Windows";
        
        public static string Read(string fileName)
        {
            string path = "";
            string projectDirectory = GetProjectDirectory();

            if (IsUnix)
                path = $"{projectDirectory}/{INPUT_FOLDER_NAME}/{fileName}";
            else if (IsWindows)
                path = $"{projectDirectory}\\{INPUT_FOLDER_NAME}\\{fileName}";
                
            if (path == "")
                throw new Exception("Could not detect operating system!");
                    
            return File.ReadAllText(path);
        }

        private static string GetProjectDirectory()
        {
            string separator = null;
            string projectDirectory = Directory.GetCurrentDirectory();
            if (IsUnix)
                separator = "/";
            else if (IsWindows)
                separator = "\\";
            string[] dirNames = projectDirectory.Split(separator);
            if (dirNames.Length >= 3 && dirNames[dirNames.Length - 3].Equals("bin"))
                projectDirectory = (Directory.GetParent(projectDirectory).Parent).Parent.FullName;
            return projectDirectory;
        }
        
        private static bool IsUnix => Environment.OSVersion.ToString().StartsWith(UNIX_PREFIX);
        private static bool IsWindows => Environment.OSVersion.ToString().StartsWith(WINDOWS_PREFIX);

    }
}