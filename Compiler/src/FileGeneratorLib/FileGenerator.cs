using ASTLib.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileGeneratorLib.Interfaces;
using FileUtilities.Interfaces;

namespace FileGeneratorLib
{
    public class FileGenerator : IFileGenerator
    {
        private const string OUTPUT_FOLDER_NAME = "OutputFiles";
        private readonly IFileHelper _helper;

        public FileGenerator(IFileHelper helper)
        {
            _helper = helper;
        }

        public void Export(List<ExtensionalGraph> gmlGraphs, bool writeToConsole, bool writeToFiles, bool useProjectFolder)
        {
            for (int i = 0; i < gmlGraphs.Count; i++)
            {
                ExtensionalGraph extensionalGraph = gmlGraphs[i];
                if (writeToFiles)
                {
                    string path = useProjectFolder ? 
                                  _helper.GetPathWith(OUTPUT_FOLDER_NAME, extensionalGraph.FileName + ".gml") : 
                                  extensionalGraph.FileName + ".gml";
                    if (useProjectFolder)
                        _helper.EnsureOutputDirectoryCreated(OUTPUT_FOLDER_NAME);
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
    }
}