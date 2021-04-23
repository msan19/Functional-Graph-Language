using System;
using System.Collections.Generic;
using System.IO;

namespace FileUtilities
{
    public class FileReader
    {
        private const string INPUT_FOLDER_NAME = "InputFiles";

        private readonly FileHelper _fileHelper;

        public FileReader(FileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }
        
        public string Read(List<string> fileNames, bool useProjectFolder)
        {
            string s = "";
            foreach (string f in fileNames)
                s += "\n//File: " + f + "\n" + Read(f, useProjectFolder) + "\n\n";
            return s;
        }

        private string Read(string fileName, bool useProjectFolder)
        {
            string path = useProjectFolder ?
                _fileHelper.GetPathWith(INPUT_FOLDER_NAME, fileName) :
                fileName;

            if (path == "")
                throw new Exception("Could not detect operating system!");

            return File.ReadAllText(path);
        }
    }
}