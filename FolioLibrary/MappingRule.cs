using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("mapping_rules", Schema = "diku_mod_source_record_manager")]
    public partial class MappingRule
    {
        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content} }}";

        public static MappingRule FromJObject(JObject jObject) => new MappingRule
        {
            Content = jObject.ToString()
        };

        public JObject ToJObject() => JObject.Parse(Content);
    }
}
