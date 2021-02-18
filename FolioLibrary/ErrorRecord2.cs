using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.error_records -> diku_mod_source_record_storage.error_records_lb
    // ErrorRecord2 -> ErrorRecord
    [DisplayColumn(nameof(Id)), DisplayName("Error Records"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("error_records", Schema = "uc")]
    public partial class ErrorRecord2
    {
        [Column("id"), Display(Name = "Record 2", Order = 1), Editable(false), ForeignKey("Record2"), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Record 2", Order = 2)]
        public virtual Record2 Record2 { get; set; }

        [Column("content"), DataType(DataType.MultilineText), Display(Order = 3), Editable(false)]
        public virtual string Content { get; set; }

        [Column("description"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Description { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(Description)} = {Description} }}";

        public static ErrorRecord2 FromJObject(JObject jObject) => jObject != null ? new ErrorRecord2
        {
            Id = (Guid?)jObject.SelectToken("id")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id)).RemoveNullAndEmptyProperties();
    }
}
