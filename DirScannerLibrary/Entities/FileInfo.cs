namespace DirScannerLibrary.Entities
{
    public class FileInfo
    {
        public string Name { get; set; }

        public string MIMEType { get; set; }

        public long Size { get; set; }

        public FileInfo(string _name, string _MIMEType, long _size)
        {
            Name = _name;
            MIMEType = _MIMEType;
            Size = _size;
        }
    }
}
