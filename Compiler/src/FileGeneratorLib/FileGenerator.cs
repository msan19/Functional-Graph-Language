using ASTLib.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileGeneratorLib
{
    public class FileGenerator : IFileGenerator
    {
        private const string INPUT_FOLDER_NAME = "InputFiles";
        private const string OUTPUT_FOLDER_NAME = "OutputFiles";
        private readonly IGmlGenerator _gmlGenerator;
        private readonly IFileHelper _helper;

        public FileGenerator(IGmlGenerator gmlGenerator, IFileHelper helper)
        {
            _gmlGenerator = gmlGenerator;
            _helper = helper;
        }

        public void Export(List<LabelGraph> output, bool writeToConsole, bool writeToFiles, bool useProjectFolder)
        {
            for (int i = 0; i < output.Count; i++)
            {
                string gmlStr = "";
                gmlStr = _gmlGenerator.Generate(output[i]);
                if (writeToFiles)
                {
                    string path = useProjectFolder ? 
                                  _helper.GetPathWith(OUTPUT_FOLDER_NAME, output[i].FileName + ".gml") : 
                                  output[i].FileName + ".gml";
                    if (useProjectFolder)
                        _helper.EnsureOutputDirectoryCreated(OUTPUT_FOLDER_NAME);
                    File.WriteAllText(path, gmlStr);
                }
                if (writeToConsole)
                {
                    if (i == 0)
                        Console.WriteLine("\nRESULTS:\n");
                    Console.WriteLine($"Output GML graph {i}: \n");
                    Console.WriteLine(gmlStr);
                }
            }
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
                                  _helper.GetPathWith(INPUT_FOLDER_NAME, fileName) :
                                  fileName;

            if (path == "")
                throw new Exception("Could not detect operating system!");

            return File.ReadAllText(path);
        }
    }
}