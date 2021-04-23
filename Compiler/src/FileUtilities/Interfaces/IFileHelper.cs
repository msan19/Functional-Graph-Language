namespace FileUtilities.Interfaces
{
    public interface IFileHelper
    {
        string GetPathWith(string folder, string fileName);
        void EnsureOutputDirectoryCreated(string folderName);
    }
}