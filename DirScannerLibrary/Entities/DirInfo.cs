namespace DirScannerLibrary.Entities
{
    public class DirInfo
    {
        public long Size { get; set; }

        public string Name { get; set; }

        public DirInfo(long _size, string _name)
        {
            Size = _size;
            Name = _name;
        }
    }
}
