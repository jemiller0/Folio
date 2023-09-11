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
    // uc.agreements -> uc_agreements.agreements
    // Agreement2 -> Agreement
    [DisplayColumn(nameof(Name)), DisplayName("Agreements"), JsonConverter(typeof(JsonPathJsonConverter<Agreement2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("agreements", Schema = "uc")]
    public partial class Agreement2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Agreement.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), JsonProperty("name"), StringLength(255)]
        public virtual string Name { get; set; }

        [Column("start_date"), DataType(DataType.Date), Display(Name = "Start Date", Order = 3), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("startDate")]
        public virtual DateTime? StartDate { get; set; }

        [Column("end_date"), DataType(DataType.Date), Display(Name = "End Date", Order = 4), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("endDate")]
        public virtual DateTime? EndDate { get; set; }

        [Column("cancellation_deadline"), DataType(DataType.Date), Display(Name = "Cancellation Deadline Date", Order = 5), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("cancellationDeadline")]
        public virtual DateTime? CancellationDeadlineDate { get; set; }

        [Column("status_label"), Display(Order = 6), JsonProperty("status.label"), StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("is_perpetual_label"), Display(Name = "Is Perpetual", Order = 7), JsonProperty("isPerpetual.label"), StringLength(1024)]
        public virtual string IsPerpetual { get; set; }

        [Column("description"), Display(Order = 8), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("date_created"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 9), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("dateCreated")]
        public virtual DateTime? CreationTime { get; set; }

        [Column("last_updated"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 10), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("lastUpdated")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Column("content"), CustomValidation(typeof(Agreement), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 11), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Agreement Items", Order = 12)]
        public virtual ICollection<AgreementItem2> AgreementItem2s { get; set; }

        [Display(Name = "Agreement Organizations", Order = 13), JsonProperty("orgs")]
        public virtual ICollection<AgreementOrganization> AgreementOrganizations { get; set; }

        [Display(Name = "Order Items", Order = 14)]
        public virtual ICollection<OrderItem2> OrderItem2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(StartDate)} = {StartDate}, {nameof(EndDate)} = {EndDate}, {nameof(CancellationDeadlineDate)} = {CancellationDeadlineDate}, {nameof(Status)} = {Status}, {nameof(IsPerpetual)} = {IsPerpetual}, {nameof(Description)} = {Description}, {nameof(CreationTime)} = {CreationTime}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(Content)} = {Content}, {nameof(AgreementOrganizations)} = {(AgreementOrganizations != null ? $"{{ {string.Join(", ", AgreementOrganizations)} }}" : "")} }}";

        public static Agreement2 FromJObject(JObject jObject) => jObject != null ? new Agreement2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            StartDate = ((DateTime?)jObject.SelectToken("startDate"))?.ToUniversalTime(),
            EndDate = ((DateTime?)jObject.SelectToken("endDate"))?.ToUniversalTime(),
            CancellationDeadlineDate = ((DateTime?)jObject.SelectToken("cancellationDeadline"))?.ToUniversalTime(),
            Status = (string)jObject.SelectToken("status.label"),
            IsPerpetual = (string)jObject.SelectToken("isPerpetual.label"),
            Description = (string)jObject.SelectToken("description"),
            CreationTime = (DateTime?)jObject.SelectToken("dateCreated"),
            LastWriteTime = (DateTime?)jObject.SelectToken("lastUpdated"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            AgreementOrganizations = jObject.SelectToken("orgs")?.Where(jt => jt.HasValues).Select(jt => AgreementOrganization.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("startDate", StartDate?.ToLocalTime()),
            new JProperty("endDate", EndDate?.ToLocalTime()),
            new JProperty("cancellationDeadline", CancellationDeadlineDate?.ToLocalTime()),
            new JProperty("status", new JObject(
                new JProperty("label", Status))),
            new JProperty("isPerpetual", new JObject(
                new JProperty("label", IsPerpetual))),
            new JProperty("description", Description),
            new JProperty("dateCreated", CreationTime?.ToLocalTime()),
            new JProperty("lastUpdated", LastWriteTime?.ToLocalTime()),
            new JProperty("orgs", AgreementOrganizations?.Select(ao => ao.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
