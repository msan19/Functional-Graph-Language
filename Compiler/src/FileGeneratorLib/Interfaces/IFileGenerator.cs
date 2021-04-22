using ASTLib.Objects;
using System.Collections.Generic;

namespace FileGeneratorLib
{
    public interface IFileGenerator
    {
        void Export(List<LabelGraph> output, bool writeToConsole, bool writeToFiles, bool useProjectFolder);

        string Read(List<string> fileNames, bool useProjectFolder);
    }
}