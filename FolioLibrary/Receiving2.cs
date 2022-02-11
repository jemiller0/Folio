using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.receivings -> uchicago_mod_orders_storage.pieces
    // Receiving2 -> Receiving
    [DisplayColumn(nameof(Id)), DisplayName("Receivings"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("receivings", Schema = "uc")]
    public partial class Receiving2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Receiving.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("caption"), Display(Order = 2), JsonProperty("caption"), StringLength(1024)]
        public virtual string Caption { get; set; }

        [Column("comment"), Display(Order = 3), JsonProperty("comment"), StringLength(1024)]
        public virtual string Comment { get; set; }

        [Column("format"), Display(Order = 4), JsonProperty("format"), RegularExpression(@"^(Physical|Electronic|Other)$"), Required, StringLength(1024)]
        public virtual string Format { get; set; }

        [Display(Order = 5)]
        public virtual Item2 Item { get; set; }

        [Column("item_id"), Display(Name = "Item", Order = 6), JsonProperty("itemId")]
        public virtual Guid? ItemId { get; set; }

        [Display(Order = 7)]
        public virtual Location2 Location { get; set; }

        [Column("location_id"), Display(Name = "Location", Order = 8), JsonProperty("locationId")]
        public virtual Guid? LocationId { get; set; }

        [Display(Name = "Order Item", Order = 9)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("po_line_id"), Display(Name = "Order Item", Order = 10), JsonProperty("poLineId"), Required]
        public virtual Guid? OrderItemId { get; set; }

        [Display(Order = 11)]
        public virtual Title2 Title { get; set; }

        [Column("title_id"), Display(Name = "Title", Order = 12), JsonProperty("titleId"), Required]
        public virtual Guid? TitleId { get; set; }

        [Display(Order = 13)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id"), Display(Name = "Holding", Order = 14), JsonProperty("holdingId")]
        public virtual Guid? HoldingId { get; set; }

        [Column("display_on_holding"), Display(Name = "Display On Holding", Order = 15), JsonProperty("displayOnHolding")]
        public virtual bool? DisplayOnHolding { get; set; }

        [Column("enumeration"), Display(Order = 16), JsonProperty("enumeration"), StringLength(1024)]
        public virtual string Enumeration { get; set; }

        [Column("chronology"), Display(Order = 17), JsonProperty("chronology"), StringLength(1024)]
        public virtual string Chronology { get; set; }

        [Column("discovery_suppress"), Display(Name = "Discovery Suppress", Order = 18), JsonProperty("discoverySuppress")]
        public virtual bool? DiscoverySuppress { get; set; }

        [Column("receiving_status"), Display(Name = "Receiving Status", Order = 19), JsonProperty("receivingStatus"), RegularExpression(@"^(Received|Expected)$"), Required, StringLength(1024)]
        public virtual string ReceivingStatus { get; set; }

        [Column("supplement"), Display(Order = 20), JsonProperty("supplement")]
        public virtual bool? Supplement { get; set; }

        [Column("receipt_date"), DataType(DataType.DateTime), Display(Name = "Receipt Time", Order = 21), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("receiptDate")]
        public virtual DateTime? ReceiptTime { get; set; }

        [Column("received_date"), DataType(DataType.DateTime), Display(Name = "Receive Time", Order = 22), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("receivedDate")]
        public virtual DateTime? ReceiveTime { get; set; }

        [Column("content"), CustomValidation(typeof(Receiving), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 23), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Caption)} = {Caption}, {nameof(Comment)} = {Comment}, {nameof(Format)} = {Format}, {nameof(ItemId)} = {ItemId}, {nameof(LocationId)} = {LocationId}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(TitleId)} = {TitleId}, {nameof(HoldingId)} = {HoldingId}, {nameof(DisplayOnHolding)} = {DisplayOnHolding}, {nameof(Enumeration)} = {Enumeration}, {nameof(Chronology)} = {Chronology}, {nameof(DiscoverySuppress)} = {DiscoverySuppress}, {nameof(ReceivingStatus)} = {ReceivingStatus}, {nameof(Supplement)} = {Supplement}, {nameof(ReceiptTime)} = {ReceiptTime}, {nameof(ReceiveTime)} = {ReceiveTime}, {nameof(Content)} = {Content} }}";

        public static Receiving2 FromJObject(JObject jObject) => jObject != null ? new Receiving2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Caption = (string)jObject.SelectToken("caption"),
            Comment = (string)jObject.SelectToken("comment"),
            Format = (string)jObject.SelectToken("format"),
            ItemId = (Guid?)jObject.SelectToken("itemId"),
            LocationId = (Guid?)jObject.SelectToken("locationId"),
            OrderItemId = (Guid?)jObject.SelectToken("poLineId"),
            TitleId = (Guid?)jObject.SelectToken("titleId"),
            HoldingId = (Guid?)jObject.SelectToken("holdingId"),
            DisplayOnHolding = (bool?)jObject.SelectToken("displayOnHolding"),
            Enumeration = (string)jObject.SelectToken("enumeration"),
            Chronology = (string)jObject.SelectToken("chronology"),
            DiscoverySuppress = (bool?)jObject.SelectToken("discoverySuppress"),
            ReceivingStatus = (string)jObject.SelectToken("receivingStatus"),
            Supplement = (bool?)jObject.SelectToken("supplement"),
            ReceiptTime = (DateTime?)jObject.SelectToken("receiptDate"),
            ReceiveTime = (DateTime?)jObject.SelectToken("receivedDate"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("caption", Caption),
            new JProperty("comment", Comment),
            new JProperty("format", Format),
            new JProperty("itemId", ItemId),
            new JProperty("locationId", LocationId),
            new JProperty("poLineId", OrderItemId),
            new JProperty("titleId", TitleId),
            new JProperty("holdingId", HoldingId),
            new JProperty("displayOnHolding", DisplayOnHolding),
            new JProperty("enumeration", Enumeration),
            new JProperty("chronology", Chronology),
            new JProperty("discoverySuppress", DiscoverySuppress),
            new JProperty("receivingStatus", ReceivingStatus),
            new JProperty("supplement", Supplement),
            new JProperty("receiptDate", ReceiptTime?.ToLocalTime()),
            new JProperty("receivedDate", ReceiveTime?.ToLocalTime())).RemoveNullAndEmptyProperties();
    }
}
