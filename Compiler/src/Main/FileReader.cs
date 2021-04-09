using System;
using System.IO;

namespace Main
{
    public static class FileReader
    {
        private const string INPUT_FOLDER_NAME = "InputFiles";
        
        public static string Read(string fileName)
        {
            string path = "";
            Console.WriteLine($"OS: {Environment.OSVersion}");
            
            string projectDirectory = GetProjectDirectory();
            Console.WriteLine(projectDirectory);

            if (Environment.OSVersion.ToString().StartsWith("Unix"))
                path = $"{projectDirectory}/{INPUT_FOLDER_NAME}/{fileName}";
            else if (Environment.OSVersion.ToString().StartsWith("Microsoft Windows"))
                path = $"{projectDirectory}\\{INPUT_FOLDER_NAME}\\{fileName}";
                
            if (path == "")
                throw new Exception("Could not detect operating system!");
                    
            return File.ReadAllText(path);
        }

        private static string GetProjectDirectory()
        {
            string separator = null;
            string projectDirectory = Directory.GetCurrentDirectory();
            if (Environment.OSVersion.ToString().StartsWith("Unix"))
                separator = "/";
            else if (Environment.OSVersion.ToString().StartsWith("Microsoft Windows"))
                separator = "\\";
            string[] dirNames = projectDirectory.Split(separator);
            if (dirNames.Length >= 3 && dirNames[dirNames.Length - 3].Equals("bin"))
                projectDirectory = (Directory.GetParent(projectDirectory).Parent).Parent.FullName;
            return projectDirectory;
        }
    }
}