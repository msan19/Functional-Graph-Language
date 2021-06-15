using System.Collections.Generic;
using ASTLib;

namespace FileGeneratorLib.Interfaces
{
    public interface IFileGenerator
    {
        void Export(List<ExtensionalGraph> gmlGraphs, OutputLanguage output, bool writeToConsole, 
                                                      bool writeToFiles, bool useProjectFolder);
    }
}