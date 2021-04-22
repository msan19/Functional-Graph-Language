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

        public void Export(List<LabelGraph> output, string file)
        {
            string text = "\nRESULTS:\n";
            
            //for(int i = 0; i < output.Count; i++)
            //    text += $"\tOutput {i} =\n\n {GetGraphString(output[i])} \n";
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
            return "graph [ \n\tdirected 1\n" + s + "]\n"; 
        }

        private string GetVertexString(Element element, int i)
        {
            return "\tnode [ \n\t\tid " + i + "\n\t\tlabel \"" + GetIndicesString(element.Indices) + "\"\n\t]\n";
        }

        private string GetIndicesString(List<int> indices)
        {
            string s = "";
            foreach (int i in indices)
                s += i + " ";
            return s;
        }
    }
}