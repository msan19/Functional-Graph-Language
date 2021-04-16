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
                text += $"\tOutput {i} = {GetGraphString(output[i])} \n";
            try
            {
                Console.WriteLine(text);
                //File.WriteAllText(file, text);
            } catch
            {
                throw;
            }
        }

        private string GetGraphString(Set set)
        {
            string s = "";
            for (int i = 0; i < set.Elements.Count; i++)
                s += GetVertexString(set.Elements[i], i);
            return "graph [ \n\tdirected 1\n" + s + "]"; 
        }

        private string GetVertexString(Element element, int i)
        {
            return "\tgraph [ \n\t\tid " + i + "\n\t]";
        }
    }
}