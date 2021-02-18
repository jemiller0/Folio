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
    // uc.fiscal_years -> diku_mod_finance_storage.fiscal_year
    // FiscalYear2 -> FiscalYear
    [DisplayColumn(nameof(Name)), DisplayName("Fiscal Years"), JsonConverter(typeof(JsonPathJsonConverter<FiscalYear2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("fiscal_years", Schema = "uc")]
    public partial class FiscalYear2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.FiscalYear.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), JsonProperty("name"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("code"), Display(Order = 3), JsonProperty("code"), Required, StringLength(1024)]
        public virtual string Code { get; set; }

        [Column("currency"), Display(Order = 4), JsonProperty("currency"), StringLength(1024)]
        public virtual string Currency { get; set; }

        [Column("description"), Display(Order = 5), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("period_start"), DataType(DataType.Date), Display(Name = "Start Date", Order = 6), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("periodStart"), Required]
        public virtual DateTime? StartDate { get; set; }

        [Column("period_end"), DataType(DataType.Date), Display(Name = "End Date", Order = 7), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("periodEnd"), Required]
        public virtual DateTime? EndDate { get; set; }

        [Column("series"), Display(Order = 8), JsonProperty("series"), StringLength(1024)]
        public virtual string Series { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 9), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 10), InverseProperty("FiscalYear2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 11), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 13), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 14), InverseProperty("FiscalYear2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 15), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(FiscalYear), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 17), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Budgets", Order = 18)]
        public virtual ICollection<Budget2> Budget2s { get; set; }

        [Display(Name = "Fiscal Year Acquisitions Units", Order = 19), JsonConverter(typeof(ArrayJsonConverter<List<FiscalYearAcquisitionsUnit>, FiscalYearAcquisitionsUnit>), "AcquisitionsUnitId"), JsonProperty("acqUnitIds")]
        public virtual ICollection<FiscalYearAcquisitionsUnit> FiscalYearAcquisitionsUnits { get; set; }

        [Display(Name = "Group Fund Fiscal Years", Order = 20)]
        public virtual ICollection<GroupFundFiscalYear2> GroupFundFiscalYear2s { get; set; }

        [Display(Name = "Ledgers", Order = 21)]
        public virtual ICollection<Ledger2> Ledger2s { get; set; }

        [Display(Name = "Transactions", Order = 22)]
        public virtual ICollection<Transaction2> Transaction2s { get; set; }

        [Display(Name = "Transactions 1", Order = 23)]
        public virtual ICollection<Transaction2> Transaction2s1 { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Code)} = {Code}, {nameof(Currency)} = {Currency}, {nameof(Description)} = {Description}, {nameof(StartDate)} = {StartDate}, {nameof(EndDate)} = {EndDate}, {nameof(Series)} = {Series}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(FiscalYearAcquisitionsUnits)} = {(FiscalYearAcquisitionsUnits != null ? $"{{ {string.Join(", ", FiscalYearAcquisitionsUnits)} }}" : "")} }}";

        public static FiscalYear2 FromJObject(JObject jObject) => jObject != null ? new FiscalYear2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Code = (string)jObject.SelectToken("code"),
            Currency = (string)jObject.SelectToken("currency"),
            Description = (string)jObject.SelectToken("description"),
            StartDate = ((DateTime?)jObject.SelectToken("periodStart"))?.ToLocalTime(),
            EndDate = ((DateTime?)jObject.SelectToken("periodEnd"))?.ToLocalTime(),
            Series = (string)jObject.SelectToken("series"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString(),
            FiscalYearAcquisitionsUnits = jObject.SelectToken("acqUnitIds")?.Where(jt => jt.HasValues).Select(jt => FiscalYearAcquisitionsUnit.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("code", Code),
            new JProperty("currency", Currency),
            new JProperty("description", Description),
            new JProperty("periodStart", StartDate?.ToUniversalTime()),
            new JProperty("periodEnd", EndDate?.ToUniversalTime()),
            new JProperty("series", Series),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("acqUnitIds", FiscalYearAcquisitionsUnits?.Select(fyau => fyau.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
