using System;
using System.Collections.Generic;
using System.IO;
using FileUtilities.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FileUtilities
{
    public class FileReader : IFileReader
    {
        private readonly Settings _conf;
        private readonly FileHelper _fileHelper;

        public FileReader(FileHelper fileHelper)
        {
            _fileHelper = fileHelper;
            _conf = GetConfig();
        }
        
        public string Read(List<string> fileNames, bool useProjectFolder)
        {
            string s = "";
            foreach (string f in fileNames)
                s += "\n//File: " + f + "\n" + Read(f, useProjectFolder) + "\n\n";
            return s;
        }

        public string Read(string fileName, bool useProjectFolder)
        {
            string path = useProjectFolder ?
                _fileHelper.GetPathWith(_conf.InputFolderName, fileName) :
                fileName;

            if (path == "")
                throw new Exception("Could not detect operating system!");

            return File.ReadAllText(path);
        }
        
        private Settings GetConfig()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var section = config.GetSection("FileUtilities").Get<Settings>();
            return section;
        }
    }
}