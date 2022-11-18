using DirScannerLibrary.Entities;
using FileInfo = DirScannerLibrary.Entities.FileInfo;

namespace OutputLibrary
{
    public interface IOutputGenerator
    {
        public Task GenerateHtmlAsync(List<ScanStats> scanStats, List<DirInfo> dirInfos, List<FileInfo> fileInfos);
    }
}
