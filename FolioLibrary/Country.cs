using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [DisplayColumn(nameof(Name)), Table("countries", Schema = "uc")]
    public partial class Country
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual int? Id { get; set; }

        [Column("alpha2_code"), Display(Name = "Alpha 2 Code", Order = 2), Required, StringLength(2)]
        public virtual string Alpha2Code { get; set; }

        [Column("alpha3_code"), Display(Name = "Alpha 3 Code", Order = 3), Required, StringLength(3)]
        public virtual string Alpha3Code { get; set; }

        [Column("name"), Display(Order = 4), Required, StringLength(128)]
        public virtual string Name { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Alpha2Code)} = {Alpha2Code}, {nameof(Alpha3Code)} = {Alpha3Code}, {nameof(Name)} = {Name} }}";
    }
}
