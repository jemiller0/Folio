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
    // uc.fee_types -> diku_mod_feesfines.feefines
    // FeeType2 -> FeeType
    [DisplayColumn(nameof(Name)), DisplayName("Fee Types"), JsonConverter(typeof(JsonPathJsonConverter<FeeType2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("fee_types", Schema = "uc")]
    public partial class FeeType2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.FeeType.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("automatic"), Display(Order = 2), JsonProperty("automatic")]
        public virtual bool? Automatic { get; set; }

        [Column("fee_fine_type"), Display(Order = 3), JsonProperty("feeFineType"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("default_amount"), DataType(DataType.Currency), Display(Name = "Default Amount", Order = 4), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("defaultAmount")]
        public virtual decimal? DefaultAmount { get; set; }

        [Display(Name = "Charge Notice", Order = 5), InverseProperty("FeeType2s1")]
        public virtual Template2 ChargeNotice { get; set; }

        [Column("charge_notice_id"), Display(Name = "Charge Notice", Order = 6), JsonProperty("chargeNoticeId")]
        public virtual Guid? ChargeNoticeId { get; set; }

        [Display(Name = "Action Notice", Order = 7), InverseProperty("FeeType2s")]
        public virtual Template2 ActionNotice { get; set; }

        [Column("action_notice_id"), Display(Name = "Action Notice", Order = 8), JsonProperty("actionNoticeId")]
        public virtual Guid? ActionNoticeId { get; set; }

        [Display(Order = 9)]
        public virtual Owner2 Owner { get; set; }

        [Column("owner_id"), Display(Name = "Owner", Order = 10), JsonProperty("ownerId")]
        public virtual Guid? OwnerId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 11), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 12), InverseProperty("FeeType2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 13), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 16), InverseProperty("FeeType2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 17), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(FeeType), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 19), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Fees", Order = 20)]
        public virtual ICollection<Fee2> Fee2s { get; set; }

        [Display(Name = "User Summary Open Fees Fines", Order = 21)]
        public virtual ICollection<UserSummaryOpenFeesFine> UserSummaryOpenFeesFines { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Automatic)} = {Automatic}, {nameof(Name)} = {Name}, {nameof(DefaultAmount)} = {DefaultAmount}, {nameof(ChargeNoticeId)} = {ChargeNoticeId}, {nameof(ActionNoticeId)} = {ActionNoticeId}, {nameof(OwnerId)} = {OwnerId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static FeeType2 FromJObject(JObject jObject) => jObject != null ? new FeeType2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Automatic = (bool?)jObject.SelectToken("automatic"),
            Name = (string)jObject.SelectToken("feeFineType"),
            DefaultAmount = (decimal?)jObject.SelectToken("defaultAmount"),
            ChargeNoticeId = (Guid?)jObject.SelectToken("chargeNoticeId"),
            ActionNoticeId = (Guid?)jObject.SelectToken("actionNoticeId"),
            OwnerId = (Guid?)jObject.SelectToken("ownerId"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("automatic", Automatic),
            new JProperty("feeFineType", Name),
            new JProperty("defaultAmount", DefaultAmount),
            new JProperty("chargeNoticeId", ChargeNoticeId),
            new JProperty("actionNoticeId", ActionNoticeId),
            new JProperty("ownerId", OwnerId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
