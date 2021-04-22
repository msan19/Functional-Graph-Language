using System;
using System.Collections.Generic;
using System.IO;

namespace FileGeneratorLib
{
    public class FileHelper : IFileHelper
    {
        private const string UNIX_PREFIX = "Unix";
        private const string WINDOWS_PREFIX = "Microsoft Windows";
        protected bool IsUnix => Environment.OSVersion.ToString().StartsWith(UNIX_PREFIX);
        protected bool IsWindows => Environment.OSVersion.ToString().StartsWith(WINDOWS_PREFIX);

        public string GetPathWith(string folder, string fileName)
        {
            string path = "";
            string projectDirectory = GetProjectDirectory();
            //Console.WriteLine($"OS: {Environment.OSVersion}");
            //Console.WriteLine(projectDirectory);

            if (IsUnix)
                path = $"{projectDirectory}/{folder}/{fileName}";
            else if (IsWindows)
                path = $"{projectDirectory}\\{folder}/{fileName}";

            return path;
        }

        protected string GetProjectDirectory()
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

    }
}