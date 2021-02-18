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
    // uc.overdue_fine_policies -> diku_mod_feesfines.overdue_fine_policy
    // OverdueFinePolicy2 -> OverdueFinePolicy
    [DisplayColumn(nameof(Name)), DisplayName("Overdue Fine Policies"), JsonConverter(typeof(JsonPathJsonConverter<OverdueFinePolicy2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("overdue_fine_policies", Schema = "uc")]
    public partial class OverdueFinePolicy2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.OverdueFinePolicy.json")))
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

        [Column("description"), Display(Order = 3), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("overdue_fine_quantity"), Display(Name = "Overdue Fine Quantity", Order = 4), JsonProperty("overdueFine.quantity")]
        public virtual decimal? OverdueFineQuantity { get; set; }

        [Column("overdue_fine_interval_id"), Display(Name = "Overdue Fine Interval", Order = 5), JsonProperty("overdueFine.intervalId"), RegularExpression(@"^(minute|hour|day|week|month|year)$"), StringLength(1024)]
        public virtual string OverdueFineInterval { get; set; }

        [Column("count_closed"), Display(Name = "Count Closed", Order = 6), JsonProperty("countClosed")]
        public virtual bool? CountClosed { get; set; }

        [Column("max_overdue_fine"), Display(Name = "Max Overdue Fine", Order = 7), JsonProperty("maxOverdueFine")]
        public virtual decimal? MaxOverdueFine { get; set; }

        [Column("forgive_overdue_fine"), Display(Name = "Forgive Overdue Fine", Order = 8), JsonProperty("forgiveOverdueFine")]
        public virtual bool? ForgiveOverdueFine { get; set; }

        [Column("overdue_recall_fine_quantity"), Display(Name = "Overdue Recall Fine Quantity", Order = 9), JsonProperty("overdueRecallFine.quantity")]
        public virtual decimal? OverdueRecallFineQuantity { get; set; }

        [Column("overdue_recall_fine_interval_id"), Display(Name = "Overdue Recall Fine Interval", Order = 10), JsonProperty("overdueRecallFine.intervalId"), RegularExpression(@"^(minute|hour|day|week|month|year)$"), StringLength(1024)]
        public virtual string OverdueRecallFineInterval { get; set; }

        [Column("grace_period_recall"), Display(Name = "Grace Period Recall", Order = 11), JsonProperty("gracePeriodRecall")]
        public virtual bool? GracePeriodRecall { get; set; }

        [Column("max_overdue_recall_fine"), Display(Name = "Max Overdue Recall Fine", Order = 12), JsonProperty("maxOverdueRecallFine")]
        public virtual decimal? MaxOverdueRecallFine { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 13), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 14), InverseProperty("OverdueFinePolicy2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 15), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 17), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 18), InverseProperty("OverdueFinePolicy2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 19), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(OverdueFinePolicy), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 21), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Loans", Order = 22)]
        public virtual ICollection<Loan2> Loan2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Description)} = {Description}, {nameof(OverdueFineQuantity)} = {OverdueFineQuantity}, {nameof(OverdueFineInterval)} = {OverdueFineInterval}, {nameof(CountClosed)} = {CountClosed}, {nameof(MaxOverdueFine)} = {MaxOverdueFine}, {nameof(ForgiveOverdueFine)} = {ForgiveOverdueFine}, {nameof(OverdueRecallFineQuantity)} = {OverdueRecallFineQuantity}, {nameof(OverdueRecallFineInterval)} = {OverdueRecallFineInterval}, {nameof(GracePeriodRecall)} = {GracePeriodRecall}, {nameof(MaxOverdueRecallFine)} = {MaxOverdueRecallFine}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static OverdueFinePolicy2 FromJObject(JObject jObject) => jObject != null ? new OverdueFinePolicy2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Description = (string)jObject.SelectToken("description"),
            OverdueFineQuantity = (decimal?)jObject.SelectToken("overdueFine.quantity"),
            OverdueFineInterval = (string)jObject.SelectToken("overdueFine.intervalId"),
            CountClosed = (bool?)jObject.SelectToken("countClosed"),
            MaxOverdueFine = (decimal?)jObject.SelectToken("maxOverdueFine"),
            ForgiveOverdueFine = (bool?)jObject.SelectToken("forgiveOverdueFine"),
            OverdueRecallFineQuantity = (decimal?)jObject.SelectToken("overdueRecallFine.quantity"),
            OverdueRecallFineInterval = (string)jObject.SelectToken("overdueRecallFine.intervalId"),
            GracePeriodRecall = (bool?)jObject.SelectToken("gracePeriodRecall"),
            MaxOverdueRecallFine = (decimal?)jObject.SelectToken("maxOverdueRecallFine"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("description", Description),
            new JProperty("overdueFine", new JObject(
                new JProperty("quantity", OverdueFineQuantity),
                new JProperty("intervalId", OverdueFineInterval))),
            new JProperty("countClosed", CountClosed),
            new JProperty("maxOverdueFine", MaxOverdueFine),
            new JProperty("forgiveOverdueFine", ForgiveOverdueFine),
            new JProperty("overdueRecallFine", new JObject(
                new JProperty("quantity", OverdueRecallFineQuantity),
                new JProperty("intervalId", OverdueRecallFineInterval))),
            new JProperty("gracePeriodRecall", GracePeriodRecall),
            new JProperty("maxOverdueRecallFine", MaxOverdueRecallFine),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
