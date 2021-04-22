using System;
using System.Collections.Generic;
using System.IO;

namespace Main
{
    public class FileReader
    {
        private const string INPUT_FOLDER_NAME = "InputFiles";
        private const string UNIX_PREFIX = "Unix";
        private const string WINDOWS_PREFIX = "Microsoft Windows";

        public string Read(List<string> fileNames)
        {
            string s = "";
            foreach (string f in fileNames)
                s += "\n//File: " + f + "\n" + Read(f) + "\n\n";
            return s;
        }

        public string Read(string fileName)
        {
            string path = "";
            Console.WriteLine($"OS: {Environment.OSVersion}");
            
            string projectDirectory = GetProjectDirectory();
            Console.WriteLine(projectDirectory);

            if (IsUnix)
                path = $"{projectDirectory}/{INPUT_FOLDER_NAME}/{fileName}";
            else if (IsWindows)
                path = $"{projectDirectory}\\{INPUT_FOLDER_NAME}\\{fileName}";
                
            if (path == "")
                throw new Exception("Could not detect operating system!");
                    
            return File.ReadAllText(path);
        }

        private string GetProjectDirectory()
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
        
        private bool IsUnix => Environment.OSVersion.ToString().StartsWith(UNIX_PREFIX);
        private bool IsWindows => Environment.OSVersion.ToString().StartsWith(WINDOWS_PREFIX);

    }
}