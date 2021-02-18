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
    // uc.locations -> diku_mod_inventory_storage.location
    // Location2 -> Location
    [DisplayColumn(nameof(Name)), DisplayName("Locations"), JsonConverter(typeof(JsonPathJsonConverter<Location2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("locations", Schema = "uc")]
    public partial class Location2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Location.json")))
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

        [Column("description"), Display(Order = 4), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("discovery_display_name"), Display(Name = "Discovery Display Name", Order = 5), JsonProperty("discoveryDisplayName"), StringLength(1024)]
        public virtual string DiscoveryDisplayName { get; set; }

        [Column("is_active"), Display(Name = "Is Active", Order = 6), JsonProperty("isActive")]
        public virtual bool? IsActive { get; set; }

        [Display(Order = 7)]
        public virtual Institution2 Institution { get; set; }

        [Column("institution_id"), Display(Name = "Institution", Order = 8), JsonProperty("institutionId"), Required]
        public virtual Guid? InstitutionId { get; set; }

        [Display(Order = 9)]
        public virtual Campus2 Campus { get; set; }

        [Column("campus_id"), Display(Name = "Campus", Order = 10), JsonProperty("campusId"), Required]
        public virtual Guid? CampusId { get; set; }

        [Display(Order = 11)]
        public virtual Library2 Library { get; set; }

        [Column("library_id"), Display(Name = "Library", Order = 12), JsonProperty("libraryId"), Required]
        public virtual Guid? LibraryId { get; set; }

        [Display(Name = "Primary Service Point", Order = 13)]
        public virtual ServicePoint2 PrimaryServicePoint { get; set; }

        [Column("primary_service_point_id"), Display(Name = "Primary Service Point", Order = 14), JsonProperty("primaryServicePoint"), Required]
        public virtual Guid? PrimaryServicePointId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 16), InverseProperty("Location2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 17), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 19), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 20), InverseProperty("Location2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 21), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Location), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 23), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Check Ins", Order = 24)]
        public virtual ICollection<CheckIn2> CheckIn2s { get; set; }

        [Display(Name = "Holdings", Order = 25)]
        public virtual ICollection<Holding2> Holding2s { get; set; }

        [Display(Name = "Holdings 1", Order = 26)]
        public virtual ICollection<Holding2> Holding2s1 { get; set; }

        [Display(Name = "Items", Order = 27)]
        public virtual ICollection<Item2> Item2s { get; set; }

        [Display(Name = "Items 1", Order = 28)]
        public virtual ICollection<Item2> Item2s1 { get; set; }

        [Display(Name = "Items 2", Order = 29)]
        public virtual ICollection<Item2> Item2s2 { get; set; }

        [Display(Name = "Loans", Order = 30)]
        public virtual ICollection<Loan2> Loan2s { get; set; }

        [Display(Name = "Location Service Points", Order = 31), JsonConverter(typeof(ArrayJsonConverter<List<LocationServicePoint>, LocationServicePoint>), "ServicePointId"), JsonProperty("servicePointIds")]
        public virtual ICollection<LocationServicePoint> LocationServicePoints { get; set; }

        [Display(Name = "Location Settings", Order = 32)]
        public virtual ICollection<LocationSetting> LocationSettings { get; set; }

        [Display(Name = "Order Item Locations", Order = 33)]
        public virtual ICollection<OrderItemLocation2> OrderItemLocation2s { get; set; }

        [Display(Name = "Receivings", Order = 34)]
        public virtual ICollection<Receiving2> Receiving2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Code)} = {Code}, {nameof(Description)} = {Description}, {nameof(DiscoveryDisplayName)} = {DiscoveryDisplayName}, {nameof(IsActive)} = {IsActive}, {nameof(InstitutionId)} = {InstitutionId}, {nameof(CampusId)} = {CampusId}, {nameof(LibraryId)} = {LibraryId}, {nameof(PrimaryServicePointId)} = {PrimaryServicePointId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(LocationServicePoints)} = {(LocationServicePoints != null ? $"{{ {string.Join(", ", LocationServicePoints)} }}" : "")} }}";

        public static Location2 FromJObject(JObject jObject) => jObject != null ? new Location2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Code = (string)jObject.SelectToken("code"),
            Description = (string)jObject.SelectToken("description"),
            DiscoveryDisplayName = (string)jObject.SelectToken("discoveryDisplayName"),
            IsActive = (bool?)jObject.SelectToken("isActive"),
            InstitutionId = (Guid?)jObject.SelectToken("institutionId"),
            CampusId = (Guid?)jObject.SelectToken("campusId"),
            LibraryId = (Guid?)jObject.SelectToken("libraryId"),
            PrimaryServicePointId = (Guid?)jObject.SelectToken("primaryServicePoint"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString(),
            LocationServicePoints = jObject.SelectToken("servicePointIds")?.Where(jt => jt.HasValues).Select(jt => LocationServicePoint.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("code", Code),
            new JProperty("description", Description),
            new JProperty("discoveryDisplayName", DiscoveryDisplayName),
            new JProperty("isActive", IsActive),
            new JProperty("institutionId", InstitutionId),
            new JProperty("campusId", CampusId),
            new JProperty("libraryId", LibraryId),
            new JProperty("primaryServicePoint", PrimaryServicePointId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("servicePointIds", LocationServicePoints?.Select(lsp => lsp.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
