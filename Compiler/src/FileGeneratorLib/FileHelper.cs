using System;
using System.IO;

namespace FileGeneratorLib
{
    public class FileHelper
    {
        private const string UNIX_PREFIX = "Unix";
        private const string WINDOWS_PREFIX = "Microsoft Windows";
        
        public string GetPathWith(string fileName)
        {
            string path = "";
            Console.WriteLine($"OS: {Environment.OSVersion}");
            
            string projectDirectory = GetProjectDirectory();
            Console.WriteLine(projectDirectory);

            if (IsUnix)
                path = $"{projectDirectory}/{fileName}";
            else if (IsWindows)
                path = $"{projectDirectory}\\{fileName}";

            return path;
        }
        
        public string GetProjectDirectory()
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

        public string AppendStr(string path, string str)
        {
            string separator = null;
            if (IsUnix)
                separator = "/";
            else if (IsWindows)
                separator = "\\";
            return path + separator + str;
        }
        
        private bool IsUnix => Environment.OSVersion.ToString().StartsWith(UNIX_PREFIX);
        private bool IsWindows => Environment.OSVersion.ToString().StartsWith(WINDOWS_PREFIX);
    }
}