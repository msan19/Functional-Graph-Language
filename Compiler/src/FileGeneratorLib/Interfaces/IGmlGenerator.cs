using ASTLib.Objects;

namespace FileGeneratorLib
{
    public interface IGmlGenerator
    {
        string Generate(LabelGraph graph);
    }
}