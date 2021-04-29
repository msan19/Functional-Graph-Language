using System;
using System.Collections.Generic;
using System.IO;
using FileGeneratorLib.Interfaces;
using FileUtilities.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FileGeneratorLib
{
    public class FileGenerator : IFileGenerator
    {
        private readonly Settings _conf;

        private readonly IFileHelper _helper;

        public FileGenerator(IFileHelper helper)
        {
            _helper = helper;
            _conf = GetConfig();
        }

        public void Export(List<ExtensionalGraph> gmlGraphs, bool writeToConsole, bool writeToFiles, bool useProjectFolder)
        {
            for (int i = 0; i < gmlGraphs.Count; i++)
            {
                ExtensionalGraph extensionalGraph = gmlGraphs[i];
                if (writeToFiles)
                {
                    string path = useProjectFolder ? 
                                  _helper.GetPathWith(_conf.OutputFolderName, extensionalGraph.FileName + _conf.GmlFileExtension) : 
                                  extensionalGraph.FileName + _conf.GmlFileExtension;
                    if (useProjectFolder)
                        _helper.EnsureOutputDirectoryCreated(_conf.OutputFolderName);
                    File.WriteAllText(path, extensionalGraph.GraphString);
                }
                if (writeToConsole)
                {
                    if (i == 0)
                        Console.WriteLine("\nRESULTS:\n");
                    Console.WriteLine($"Output GML graph {i}: \n");
                    Console.WriteLine(extensionalGraph.GraphString);
                }
            }
        }
        
        private Settings GetConfig()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var section = config.GetSection("FileGenerator").Get<Settings>();
            return section;
        }
    }
}