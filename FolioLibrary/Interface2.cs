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
    // uc.interfaces -> diku_mod_organizations_storage.interfaces
    // Interface2 -> Interface
    [DisplayColumn(nameof(Name)), DisplayName("Interfaces"), JsonConverter(typeof(JsonPathJsonConverter<Interface2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("interfaces", Schema = "uc")]
    public partial class Interface2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Interface.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), JsonProperty("name"), StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("uri"), DataType(DataType.Url), Display(Name = "URI", Order = 3), JsonProperty("uri"), StringLength(1024)]
        public virtual string Uri { get; set; }

        [Column("notes"), Display(Order = 4), JsonProperty("notes"), StringLength(1024)]
        public virtual string Notes { get; set; }

        [Column("available"), Display(Order = 5), JsonProperty("available")]
        public virtual bool? Available { get; set; }

        [Column("delivery_method"), Display(Name = "Delivery Method", Order = 6), JsonProperty("deliveryMethod"), RegularExpression(@"^(Online|FTP|Email|Other)$"), StringLength(1024)]
        public virtual string DeliveryMethod { get; set; }

        [Column("statistics_format"), Display(Name = "Statistics Format", Order = 7), JsonProperty("statisticsFormat"), StringLength(1024)]
        public virtual string StatisticsFormat { get; set; }

        [Column("locally_stored"), DataType(DataType.Url), Display(Name = "Locally Stored", Order = 8), JsonProperty("locallyStored"), StringLength(1024)]
        public virtual string LocallyStored { get; set; }

        [Column("online_location"), DataType(DataType.Url), Display(Name = "Online Location", Order = 9), JsonProperty("onlineLocation"), StringLength(1024)]
        public virtual string OnlineLocation { get; set; }

        [Column("statistics_notes"), Display(Name = "Statistics Notes", Order = 10), JsonProperty("statisticsNotes"), StringLength(1024)]
        public virtual string StatisticsNotes { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 11), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 12), InverseProperty("Interface2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 13), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 16), InverseProperty("Interface2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 17), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Interface), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 19), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Interface Credentials", Order = 20)]
        public virtual ICollection<InterfaceCredential2> InterfaceCredential2s { get; set; }

        [Display(Name = "Interface Types", Order = 21), JsonConverter(typeof(ArrayJsonConverter<List<InterfaceType>, InterfaceType>), "Content"), JsonProperty("type")]
        public virtual ICollection<InterfaceType> InterfaceTypes { get; set; }

        [Display(Name = "Organization Interfaces", Order = 22)]
        public virtual ICollection<OrganizationInterface> OrganizationInterfaces { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Uri)} = {Uri}, {nameof(Notes)} = {Notes}, {nameof(Available)} = {Available}, {nameof(DeliveryMethod)} = {DeliveryMethod}, {nameof(StatisticsFormat)} = {StatisticsFormat}, {nameof(LocallyStored)} = {LocallyStored}, {nameof(OnlineLocation)} = {OnlineLocation}, {nameof(StatisticsNotes)} = {StatisticsNotes}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(InterfaceTypes)} = {(InterfaceTypes != null ? $"{{ {string.Join(", ", InterfaceTypes)} }}" : "")} }}";

        public static Interface2 FromJObject(JObject jObject) => jObject != null ? new Interface2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Uri = (string)jObject.SelectToken("uri"),
            Notes = (string)jObject.SelectToken("notes"),
            Available = (bool?)jObject.SelectToken("available"),
            DeliveryMethod = (string)jObject.SelectToken("deliveryMethod"),
            StatisticsFormat = (string)jObject.SelectToken("statisticsFormat"),
            LocallyStored = (string)jObject.SelectToken("locallyStored"),
            OnlineLocation = (string)jObject.SelectToken("onlineLocation"),
            StatisticsNotes = (string)jObject.SelectToken("statisticsNotes"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString(),
            InterfaceTypes = jObject.SelectToken("type")?.Where(jt => jt.HasValues).Select(jt => InterfaceType.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("uri", Uri),
            new JProperty("notes", Notes),
            new JProperty("available", Available),
            new JProperty("deliveryMethod", DeliveryMethod),
            new JProperty("statisticsFormat", StatisticsFormat),
            new JProperty("locallyStored", LocallyStored),
            new JProperty("onlineLocation", OnlineLocation),
            new JProperty("statisticsNotes", StatisticsNotes),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("type", InterfaceTypes?.Select(it => it.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
