using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [ComplexType]
    public class Font
    {
        [Required]
        public string Family { get; set; }
        [Required]
        public int? Size { get; set; }
        [Required]
        public FontWeight? Weight { get; set; }
    }
}
