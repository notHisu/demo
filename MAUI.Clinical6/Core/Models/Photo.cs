using System;
namespace Xamarin.Forms.Clinical6.Core.Models
{
    public class Photo
    {
        public byte[] Contents { get; }
        public string FileName { get; }

        public Photo(byte[] contents, string fileName)
        {
            Contents = contents;
            FileName = fileName;
        }
    }
}
