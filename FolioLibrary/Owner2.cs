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
    // uc.owners -> diku_mod_feesfines.owners
    // Owner2 -> Owner
    [DisplayColumn(nameof(Name)), DisplayName("Owners"), JsonConverter(typeof(JsonPathJsonConverter<Owner2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("owners", Schema = "uc")]
    public partial class Owner2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Owner.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("owner"), Display(Order = 2), JsonProperty("owner"), StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("desc"), Display(Order = 3), JsonProperty("desc"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Display(Name = "Default Charge Notice", Order = 4), InverseProperty("Owner2s1")]
        public virtual Template2 DefaultChargeNotice { get; set; }

        [Column("default_charge_notice_id"), Display(Name = "Default Charge Notice", Order = 5), JsonProperty("defaultChargeNoticeId")]
        public virtual Guid? DefaultChargeNoticeId { get; set; }

        [Display(Name = "Default Action Notice", Order = 6), InverseProperty("Owner2s")]
        public virtual Template2 DefaultActionNotice { get; set; }

        [Column("default_action_notice_id"), Display(Name = "Default Action Notice", Order = 7), JsonProperty("defaultActionNoticeId")]
        public virtual Guid? DefaultActionNoticeId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 8), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 9), InverseProperty("Owner2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 10), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 12), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 13), InverseProperty("Owner2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 14), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Owner), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 16), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Fees", Order = 17)]
        public virtual ICollection<Fee2> Fee2s { get; set; }

        [Display(Name = "Fee Types", Order = 18)]
        public virtual ICollection<FeeType2> FeeType2s { get; set; }

        [Display(Name = "Payment Methods", Order = 19)]
        public virtual ICollection<PaymentMethod2> PaymentMethod2s { get; set; }

        [Display(Name = "Service Point Owners", Order = 20), JsonProperty("servicePointOwner")]
        public virtual ICollection<ServicePointOwner> ServicePointOwners { get; set; }

        [Display(Name = "Transfer Accounts", Order = 21)]
        public virtual ICollection<TransferAccount2> TransferAccount2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Description)} = {Description}, {nameof(DefaultChargeNoticeId)} = {DefaultChargeNoticeId}, {nameof(DefaultActionNoticeId)} = {DefaultActionNoticeId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(ServicePointOwners)} = {(ServicePointOwners != null ? $"{{ {string.Join(", ", ServicePointOwners)} }}" : "")} }}";

        public static Owner2 FromJObject(JObject jObject) => jObject != null ? new Owner2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("owner"),
            Description = (string)jObject.SelectToken("desc"),
            DefaultChargeNoticeId = (Guid?)jObject.SelectToken("defaultChargeNoticeId"),
            DefaultActionNoticeId = (Guid?)jObject.SelectToken("defaultActionNoticeId"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            ServicePointOwners = jObject.SelectToken("servicePointOwner")?.Select(jt => ServicePointOwner.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("owner", Name),
            new JProperty("desc", Description),
            new JProperty("defaultChargeNoticeId", DefaultChargeNoticeId),
            new JProperty("defaultActionNoticeId", DefaultActionNoticeId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("servicePointOwner", ServicePointOwners?.Select(spo => spo.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
