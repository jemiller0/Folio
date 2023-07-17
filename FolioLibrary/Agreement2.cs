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
    [DisplayColumn(nameof(Name)), DisplayName("Agreements"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("agreements", Schema = "uc")]
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

        [Column("description"), Display(Order = 3), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("start_date"), DataType(DataType.Date), Display(Name = "Start Date", Order = 4), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("startDate")]
        public virtual DateTime? StartDate { get; set; }

        [Column("end_date"), DataType(DataType.Date), Display(Name = "End Date", Order = 5), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("endDate")]
        public virtual DateTime? EndDate { get; set; }

        [Column("cancellation_deadline"), Display(Name = "Cancellation Deadline", Order = 6), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("cancellationDeadline")]
        public virtual DateTime? CancellationDeadline { get; set; }

        [Column("date_created"), DataType(DataType.Date), Display(Name = "Date Created", Order = 7), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("dateCreated")]
        public virtual DateTime? DateCreated { get; set; }

        [Column("last_updated"), Display(Name = "Last Updated", Order = 8), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("lastUpdated")]
        public virtual DateTime? LastUpdated { get; set; }

        [Column("content"), CustomValidation(typeof(Agreement), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 9), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Order Items", Order = 10)]
        public virtual ICollection<OrderItem2> OrderItem2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Description)} = {Description}, {nameof(StartDate)} = {StartDate}, {nameof(EndDate)} = {EndDate}, {nameof(CancellationDeadline)} = {CancellationDeadline}, {nameof(DateCreated)} = {DateCreated}, {nameof(LastUpdated)} = {LastUpdated}, {nameof(Content)} = {Content} }}";

        public static Agreement2 FromJObject(JObject jObject) => jObject != null ? new Agreement2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Description = (string)jObject.SelectToken("description"),
            StartDate = ((DateTime?)jObject.SelectToken("startDate"))?.ToUniversalTime(),
            EndDate = ((DateTime?)jObject.SelectToken("endDate"))?.ToUniversalTime(),
            CancellationDeadline = (DateTime?)jObject.SelectToken("cancellationDeadline"),
            DateCreated = ((DateTime?)jObject.SelectToken("dateCreated"))?.ToUniversalTime(),
            LastUpdated = (DateTime?)jObject.SelectToken("lastUpdated"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("description", Description),
            new JProperty("startDate", StartDate?.ToLocalTime()),
            new JProperty("endDate", EndDate?.ToLocalTime()),
            new JProperty("cancellationDeadline", CancellationDeadline?.ToLocalTime()),
            new JProperty("dateCreated", DateCreated?.ToLocalTime()),
            new JProperty("lastUpdated", LastUpdated?.ToLocalTime())).RemoveNullAndEmptyProperties();
    }
}
