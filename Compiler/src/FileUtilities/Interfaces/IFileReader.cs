using System.Collections.Generic;

namespace FileUtilities.Interfaces
{
    public interface IFileReader
    {
        string Read(List<string> fileNames, bool useProjectFolder);
    }
}