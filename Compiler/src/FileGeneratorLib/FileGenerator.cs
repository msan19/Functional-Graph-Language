using ASTLib.Objects;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileGeneratorLib
{
    public class FileGenerator
    {
        private readonly FileHelper _helper;

        public FileGenerator(FileHelper helper)
        {
            _helper = helper;
        }

        public void Export(List<Set> output, string file)
        {
            string text = "\nRESULTS:\n";
            for(int i = 0; i < output.Count; i++)
                text += $"\tOutput {i} = {output[i]} \n";
            try
            {
                Console.WriteLine(text);
                //File.WriteAllText(file, text);
            } catch
            {
                throw;
            }
        }
    }
}