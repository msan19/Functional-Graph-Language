using System.Collections.Generic;

namespace FileUtilities.Interfaces
{
    public interface IFileReader
    {
        List<string> Read(List<string> fileNames, bool useProjectFolder);
    }
}