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

        public void Export(List<double> output, string file)
        {
            string text = "";
            foreach(double d in output)
                text += d + "\n";
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