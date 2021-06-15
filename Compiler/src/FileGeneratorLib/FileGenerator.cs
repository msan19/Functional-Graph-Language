using System;
using System.Collections.Generic;
using System.IO;
using ASTLib;
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

        public void Export(List<ExtensionalGraph> extensionalGraphs, OutputLanguage output, bool writeToConsole, 
                                                                     bool writeToFiles, bool useProjectFolder)
        {
            string fileExtension = GetExtension(output);
            for (int i = 0; i < extensionalGraphs.Count; i++)
            {
                ExtensionalGraph extensionalGraph = extensionalGraphs[i];
                if (writeToFiles)
                {
                    string path = useProjectFolder ? 
                                  _helper.GetPathWith(_conf.OutputFolderName, extensionalGraph.FileName + fileExtension) : 
                                  extensionalGraph.FileName + fileExtension;
                    if (useProjectFolder)
                        _helper.EnsureOutputDirectoryCreated(_conf.OutputFolderName);
                    File.WriteAllText(path, extensionalGraph.GraphString);
                }
                if (writeToConsole)
                {
                    if (i == 0)
                        Console.WriteLine("\nRESULTS:\n");
                    Console.WriteLine($"Output graph {i}: \n");
                    Console.WriteLine(extensionalGraph.GraphString);
                }
            }
        }
        
        private string GetExtension(OutputLanguage language)
        {
            return language switch
            {
                OutputLanguage.GML => _conf.GmlFileExtension,
                OutputLanguage.DOT => _conf.DotFileExtension,
                _ => ""
            };
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