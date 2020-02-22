using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [DisplayColumn(nameof(Name)), Table("countries", Schema = "uc")]
    public partial class Country
    {
        [Column("alpha2_code"), Display(Name = "Alpha 2 Code", Order = 1), Editable(false), Key, StringLength(2)]
        public virtual string Alpha2Code { get; set; }

        [Column("alpha3_code"), Display(Name = "Alpha 3 Code", Order = 2), Editable(false), StringLength(3)]
        public virtual string Alpha3Code { get; set; }

        [Column("name"), Display(Order = 3), Editable(false), StringLength(128)]
        public virtual string Name { get; set; }

        public override string ToString() => $"{{ {nameof(Alpha2Code)} = {Alpha2Code}, {nameof(Alpha3Code)} = {Alpha3Code}, {nameof(Name)} = {Name} }}";
    }
}
