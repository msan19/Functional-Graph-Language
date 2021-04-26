using System;
using System.IO;
using FileUtilities.Interfaces;

namespace FileUtilities
{
    public class FileHelper : IFileHelper
    {
        private const string UNIX_PREFIX = "Unix";
        private const string WINDOWS_PREFIX = "Microsoft Windows";
        protected bool IsUnix => Environment.OSVersion.ToString().StartsWith(UNIX_PREFIX);
        protected bool IsWindows => Environment.OSVersion.ToString().StartsWith(WINDOWS_PREFIX);

        public string GetPathWith(string folder, string fileName)
        {
            string separator = GetSeparator();
            string projectDirectory = GetProjectDirectory();
            return $"{projectDirectory}{separator}{folder}{separator}{fileName}";
        }

        public void EnsureOutputDirectoryCreated(string folderName)
        {
            string currentPath = GetProjectDirectory();
            string separator = GetSeparator();
            string dirPath = currentPath + separator + folderName;
            bool exists = Directory.Exists(dirPath);
            if (!exists)
                Directory.CreateDirectory(dirPath);
        }
        
        protected string GetProjectDirectory()
        {
            string separator = GetSeparator();
            string projectDirectory = Directory.GetCurrentDirectory();
            string[] dirNames = projectDirectory.Split(separator);
            if (dirNames.Length >= 3 && dirNames[dirNames.Length - 3].Equals("bin"))
                projectDirectory = (Directory.GetParent(projectDirectory).Parent).Parent.FullName;
            return projectDirectory;
        }

        private string GetSeparator()
        {
            if (IsUnix)
                return "/";
            else if (IsWindows)
                return "\\";
            return "/";
        }
    }
}