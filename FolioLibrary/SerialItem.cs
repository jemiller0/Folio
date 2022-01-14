using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.serial_items -> diku_mod_inventory_storage.holdings_record, diku_mod_inventory_storage.instance
    // SerialItem -> Holding, Instance
    [DisplayColumn(nameof(Title)), DisplayName("Serial Items"), JsonConverter(typeof(JsonPathJsonConverter<SerialItem>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("serial_items", Schema = "uc")]
    public partial class SerialItem
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Column("serial_id"), Display(Name = "Serial Id", Order = 2), StringLength(1024)]
        public virtual string SerialId { get; set; }

        [Column("bib_id"), Display(Name = "Bib Id", Order = 3), JsonProperty("instanceId")]
        public virtual Guid? BibId { get; set; }

        [Column("title"), Display(Order = 4), JsonProperty("title"), StringLength(1024)]
        public virtual string Title { get; set; }

        [Column("author"), Display(Order = 5), JsonProperty("contributors.0.name"), StringLength(1024)]
        public virtual string Author { get; set; }

        [Column("holding_id"), Display(Name = "Holding Id", Order = 6)]
        public virtual Guid? HoldingId { get; set; }

        [Column("call_number"), Display(Name = "Call Number", Order = 7), JsonProperty("callNumber"), StringLength(1024)]
        public virtual string CallNumber { get; set; }

        [Column("copy_number"), Display(Name = "Copy Number", Order = 8), JsonProperty("copyNumber"), StringLength(1024)]
        public virtual string CopyNumber { get; set; }

        [Column("location_id"), Display(Name = "Location Id", Order = 9), JsonProperty("effectiveLocationId")]
        public virtual Guid? LocationId { get; set; }

        [Column("work_unit"), Display(Name = "Work Unit", Order = 10), StringLength(1024)]
        public virtual string WorkUnit { get; set; }

        [Column("enumeration"), Display(Order = 11), StringLength(1024)]
        public virtual string Enumeration { get; set; }

        [Column("chronology"), Display(Order = 12), StringLength(1024)]
        public virtual string Chronology { get; set; }

        [Column("type"), Display(Order = 13), StringLength(1024)]
        public virtual string Type { get; set; }

        [Column("status"), Display(Order = 14), StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("last_write_time"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Column("last_write_user_id"), Display(Name = "Last Write User Id", Order = 16), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(SerialId)} = {SerialId}, {nameof(BibId)} = {BibId}, {nameof(Title)} = {Title}, {nameof(Author)} = {Author}, {nameof(HoldingId)} = {HoldingId}, {nameof(CallNumber)} = {CallNumber}, {nameof(CopyNumber)} = {CopyNumber}, {nameof(LocationId)} = {LocationId}, {nameof(WorkUnit)} = {WorkUnit}, {nameof(Enumeration)} = {Enumeration}, {nameof(Chronology)} = {Chronology}, {nameof(Type)} = {Type}, {nameof(Status)} = {Status}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId} }}";

        public static SerialItem FromJObject(JObject jObject) => jObject != null ? new SerialItem
        {
            BibId = (Guid?)jObject.SelectToken("instanceId"),
            Title = (string)jObject.SelectToken("title"),
            Author = (string)jObject.SelectToken("contributors.0.name"),
            CallNumber = (string)jObject.SelectToken("callNumber"),
            CopyNumber = (string)jObject.SelectToken("copyNumber"),
            LocationId = (Guid?)jObject.SelectToken("effectiveLocationId"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("instanceId", BibId),
            new JProperty("title", Title),
            new JProperty("contributors", new JObject(
                new JProperty("0", new JObject(
                    new JProperty("name", Author))))),
            new JProperty("callNumber", CallNumber),
            new JProperty("copyNumber", CopyNumber),
            new JProperty("effectiveLocationId", LocationId),
            new JProperty("metadata", new JObject(
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId)))).RemoveNullAndEmptyProperties();
    }
}
