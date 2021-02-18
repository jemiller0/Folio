using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.request_identifiers -> diku_mod_circulation_storage.request
    // RequestIdentifier -> Request
    [DisplayColumn(nameof(Id)), DisplayName("Request Identifiers"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("request_identifiers", Schema = "uc")]
    public partial class RequestIdentifier
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Request2 Request { get; set; }

        [Column("request_id"), Display(Name = "Request", Order = 3), Required]
        public virtual Guid? RequestId { get; set; }

        [Column("value"), Display(Order = 4), JsonProperty("value"), Required, StringLength(1024)]
        public virtual string Value { get; set; }

        [Display(Name = "Identifier Type", Order = 5)]
        public virtual IdType2 IdentifierType { get; set; }

        [Column("identifier_type_id"), Display(Name = "Identifier Type", Order = 6), JsonProperty("identifierTypeId"), Required]
        public virtual Guid? IdentifierTypeId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RequestId)} = {RequestId}, {nameof(Value)} = {Value}, {nameof(IdentifierTypeId)} = {IdentifierTypeId} }}";

        public static RequestIdentifier FromJObject(JObject jObject) => jObject != null ? new RequestIdentifier
        {
            Value = (string)jObject.SelectToken("value"),
            IdentifierTypeId = (Guid?)jObject.SelectToken("identifierTypeId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("value", Value),
            new JProperty("identifierTypeId", IdentifierTypeId)).RemoveNullAndEmptyProperties();
    }
}
