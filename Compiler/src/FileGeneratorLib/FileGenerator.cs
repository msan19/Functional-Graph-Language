using ASTLib.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileGeneratorLib
{
    public class FileGenerator
    {
        private readonly GmlGenerator _gmlGenerator;
        private readonly FileHelper _helper;

        public FileGenerator(GmlGenerator gmlGenerator, FileHelper helper)
        {
            _gmlGenerator = gmlGenerator;
            _helper = helper;
        }

        public void Export(List<LabelGraph> output, bool writeToConsole, bool writeToFiles)
        {
            for (int i = 0; i < output.Count; i++)
            {
                string gmlStr = "";
                gmlStr = _gmlGenerator.Generate(output[i]);
                if (writeToFiles)
                {
                    string path = _helper.GetPathWith(output[i].FileName + ".gml");
                    File.WriteAllText(path , gmlStr);
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
    }
}