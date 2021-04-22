using ASTLib.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileGeneratorLib
{
    public class FileGenerator
    {
        private const string UNIX_PREFIX = "Unix";
        private const string WINDOWS_PREFIX = "Microsoft Windows";
        
        private readonly FileHelper _helper;

        public FileGenerator(FileHelper helper)
        {
            _helper = helper;
        }

        public void Export(List<LabelGraph> output, bool writeToFiles)
        {
            try
            {
                for (int i = 0; i < output.Count; i++)
                {
                    string gmlStr = "";
                    gmlStr = GetGraphString(output[i]);
                    if (writeToFiles)
                    {
                        string path = GetPathWith(output[i].FileName);
                        File.WriteAllText(path , gmlStr);
                    }
                    else
                    {
                        if (i == 0)
                            Console.WriteLine("\nRESULTS:\n");
                        Console.WriteLine($"Output GML graph {i}: \n");
                        Console.WriteLine(gmlStr);
                    }
                }
            } catch
            {
                throw;
            }
        }

        private string GetGraphString(LabelGraph graph)
        {
            string s = "";

            for (int i = 0; i < graph.VertexCount; i++)
                s += GetVertexString(i);

            s += GetEdgesAsString(graph);
            
            return "graph [ \n\tdirected 1\n" + s + "]\n"; 
        }

        private string GetEdgesAsString(LabelGraph graph)
        {
            string s = "";
            for (int i = 0; i < graph.SrcList.Count; i++)
            {
                s += GetEdgeAsString(graph, i);
            }
            return s;
        }

        private string GetEdgeAsString(LabelGraph graph, int i)
        {
            StringBuilder sb = new StringBuilder("\tedge [ ");
            sb.AppendLine($"\n\t    source {graph.SrcList[i]}");
            sb.AppendLine($"\t    target {graph.DstList[i]}");
            // Add additional vertex labels here
            sb.Append("\t]\n");
            return sb.ToString(); 
        }

        private string GetVertexString(int i)
        {
            StringBuilder sb = new StringBuilder("\tnode [ ");
            sb.AppendLine($"\n\t    id {i + 1}");
            // Add additional vertex labels here
            sb.Append("\t]\n");
            return sb.ToString(); 
        }
        

        private string GetIndicesString(List<int> indices)
        {
            string s = "";
            foreach (int i in indices)
                s += i + " ";
            return s;
        }
        
        private string GetPathWith(string fileName)
        {
            string path = "";
            Console.WriteLine($"OS: {Environment.OSVersion}");
            
            string projectDirectory = GetProjectDirectory();
            Console.WriteLine(projectDirectory);

            if (IsUnix)
                path = $"{projectDirectory}/{fileName}";
            else if (IsWindows)
                path = $"{projectDirectory}\\{fileName}";

            return path;
        }
        
        private static string GetProjectDirectory()
        {
            string separator = null;
            string projectDirectory = Directory.GetCurrentDirectory();
            if (IsUnix)
                separator = "/";
            else if (IsWindows)
                separator = "\\";
            string[] dirNames = projectDirectory.Split(separator);
            if (dirNames.Length >= 3 && dirNames[dirNames.Length - 3].Equals("bin"))
                projectDirectory = (Directory.GetParent(projectDirectory).Parent).Parent.FullName;
            return projectDirectory;
        }
        
        private static bool IsUnix => Environment.OSVersion.ToString().StartsWith(UNIX_PREFIX);
        private static bool IsWindows => Environment.OSVersion.ToString().StartsWith(WINDOWS_PREFIX);

    }
}