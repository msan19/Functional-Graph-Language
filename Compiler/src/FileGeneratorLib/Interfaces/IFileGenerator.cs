using System.Collections.Generic;
using ASTLib.Objects;

namespace FileGeneratorLib.Interfaces
{
    public interface IFileGenerator
    {
        void Export(List<ExtensionalGraph> gmlGraphs, bool writeToConsole, bool writeToFiles, bool useProjectFolder);
    }
}