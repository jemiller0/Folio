using System.ComponentModel.DataAnnotations;

namespace FolioLibrary
{
    public class Label
    {
        public string Id { get; set; }

        public dynamic/*Item1*/ Item { get; set; }

        public Font Font { get; set; }

        [Required]
        public Orientation? Orientation { get; set; }

        [Required]
        public string Text { get; set; }

        public int? Width { get; set; }

        public bool IsSerial { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CallNumber { get; set; }
        public string CollectionCode { get; set; }
    }
}
