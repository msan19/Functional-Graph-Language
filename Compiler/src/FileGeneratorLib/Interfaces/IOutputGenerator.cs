using System.Collections.Generic;
using ASTLib.Objects;

namespace FileGeneratorLib.Interfaces
{
    public interface IOutputGenerator
    {
        public List<ExtensionalGraph> Generate(List<LabelGraph> graphs);
    }
}