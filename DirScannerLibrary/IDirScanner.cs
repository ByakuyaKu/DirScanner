using DirScannerLibrary.Entities;
using FileInfo = DirScannerLibrary.Entities.FileInfo;

namespace DirScannerLibrary
{
    public interface IDirScanner
    {
        public List<ScanStats> ScanDir(out List<DirInfo> dirsInfo, out List<FileInfo> filesInfo);
    }
}
