using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.agreement_items -> uc_agreements.agreement_items
    // AgreementItem2 -> AgreementItem
    [DisplayColumn(nameof(StartDate)), DisplayName("Agreement Items"), JsonConverter(typeof(JsonPathJsonConverter<AgreementItem2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("agreement_items", Schema = "uc")]
    public partial class AgreementItem2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.AgreementItem.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("type"), JsonProperty("type"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string Type { get; set; }

        [Column("suppress_from_discovery"), Display(Name = "Suppress From Discovery", Order = 3), JsonProperty("suppressFromDiscovery")]
        public virtual bool? SuppressFromDiscovery { get; set; }

        [Column("note"), Display(Order = 4), JsonProperty("note"), StringLength(1024)]
        public virtual string Note { get; set; }

        [Column("description"), Display(Order = 5), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("custom_coverage"), Display(Name = "Custom Coverage", Order = 6), JsonProperty("customCoverage")]
        public virtual bool? CustomCoverage { get; set; }

        [Column("start_date"), DataType(DataType.Date), Display(Name = "Start Date", Order = 7), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("startDate")]
        public virtual DateTime? StartDate { get; set; }

        [Column("end_date"), DataType(DataType.Date), Display(Name = "End Date", Order = 8), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("endDate")]
        public virtual DateTime? EndDate { get; set; }

        [Column("active_from"), DataType(DataType.Date), Display(Name = "Active From Date", Order = 9), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("activeFrom")]
        public virtual DateTime? ActiveFromDate { get; set; }

        [Column("active_to"), DataType(DataType.Date), Display(Name = "Active To Date", Order = 10), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("activeTo")]
        public virtual DateTime? ActiveToDate { get; set; }

        [Column("content_updated"), DataType(DataType.DateTime), Display(Name = "Content Last Write Time", Order = 11), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("contentUpdated")]
        public virtual DateTime? ContentLastWriteTime { get; set; }

        [Column("have_access"), Display(Name = "Have Access", Order = 12), JsonProperty("haveAccess")]
        public virtual bool? HaveAccess { get; set; }

        [Column("date_created"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 13), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("dateCreated")]
        public virtual DateTime? CreationTime { get; set; }

        [Column("last_updated"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 14), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("lastUpdated")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Order = 15)]
        public virtual Agreement2 Agreement { get; set; }

        [Column("agreement_id"), Display(Name = "Agreement", Order = 16), JsonProperty("owner.id")]
        public virtual Guid? AgreementId { get; set; }

        [Column("content"), CustomValidation(typeof(AgreementItem), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 17), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Agreement Item Order Items", Order = 18), JsonProperty("poLines")]
        public virtual ICollection<AgreementItemOrderItem> AgreementItemOrderItems { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Type)} = {Type}, {nameof(SuppressFromDiscovery)} = {SuppressFromDiscovery}, {nameof(Note)} = {Note}, {nameof(Description)} = {Description}, {nameof(CustomCoverage)} = {CustomCoverage}, {nameof(StartDate)} = {StartDate}, {nameof(EndDate)} = {EndDate}, {nameof(ActiveFromDate)} = {ActiveFromDate}, {nameof(ActiveToDate)} = {ActiveToDate}, {nameof(ContentLastWriteTime)} = {ContentLastWriteTime}, {nameof(HaveAccess)} = {HaveAccess}, {nameof(CreationTime)} = {CreationTime}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(AgreementId)} = {AgreementId}, {nameof(Content)} = {Content}, {nameof(AgreementItemOrderItems)} = {(AgreementItemOrderItems != null ? $"{{ {string.Join(", ", AgreementItemOrderItems)} }}" : "")} }}";

        public static AgreementItem2 FromJObject(JObject jObject) => jObject != null ? new AgreementItem2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Type = (string)jObject.SelectToken("type"),
            SuppressFromDiscovery = (bool?)jObject.SelectToken("suppressFromDiscovery"),
            Note = (string)jObject.SelectToken("note"),
            Description = (string)jObject.SelectToken("description"),
            CustomCoverage = (bool?)jObject.SelectToken("customCoverage"),
            StartDate = ((DateTime?)jObject.SelectToken("startDate"))?.ToUniversalTime(),
            EndDate = ((DateTime?)jObject.SelectToken("endDate"))?.ToUniversalTime(),
            ActiveFromDate = ((DateTime?)jObject.SelectToken("activeFrom"))?.ToUniversalTime(),
            ActiveToDate = ((DateTime?)jObject.SelectToken("activeTo"))?.ToUniversalTime(),
            ContentLastWriteTime = (DateTime?)jObject.SelectToken("contentUpdated"),
            HaveAccess = (bool?)jObject.SelectToken("haveAccess"),
            CreationTime = (DateTime?)jObject.SelectToken("dateCreated"),
            LastWriteTime = (DateTime?)jObject.SelectToken("lastUpdated"),
            AgreementId = (Guid?)jObject.SelectToken("owner.id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            AgreementItemOrderItems = jObject.SelectToken("poLines")?.Where(jt => jt.HasValues).Select(jt => AgreementItemOrderItem.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("type", Type),
            new JProperty("suppressFromDiscovery", SuppressFromDiscovery),
            new JProperty("note", Note),
            new JProperty("description", Description),
            new JProperty("customCoverage", CustomCoverage),
            new JProperty("startDate", StartDate?.ToLocalTime()),
            new JProperty("endDate", EndDate?.ToLocalTime()),
            new JProperty("activeFrom", ActiveFromDate?.ToLocalTime()),
            new JProperty("activeTo", ActiveToDate?.ToLocalTime()),
            new JProperty("contentUpdated", ContentLastWriteTime?.ToLocalTime()),
            new JProperty("haveAccess", HaveAccess),
            new JProperty("dateCreated", CreationTime?.ToLocalTime()),
            new JProperty("lastUpdated", LastWriteTime?.ToLocalTime()),
            new JProperty("owner", new JObject(
                new JProperty("id", AgreementId))),
            new JProperty("poLines", AgreementItemOrderItems?.Select(aioi => aioi.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
