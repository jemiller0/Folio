using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.raw_records -> uchicago_mod_source_record_storage.raw_records_lb
    // RawRecord2 -> RawRecord
    [DisplayColumn(nameof(Id)), DisplayName("Raw Records"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("raw_records", Schema = "uc")]
    public partial class RawRecord2
    {
        [Column("id"), Display(Name = "Record 2", Order = 1), Editable(false), ForeignKey("Record2"), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Record 2", Order = 2)]
        public virtual Record2 Record2 { get; set; }

        [Column("content"), DataType(DataType.MultilineText), Display(Order = 3), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content} }}";

        public static RawRecord2 FromJObject(JObject jObject) => jObject != null ? new RawRecord2
        {
            Id = (Guid?)jObject.SelectToken("id")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id)).RemoveNullAndEmptyProperties();
    }
}
