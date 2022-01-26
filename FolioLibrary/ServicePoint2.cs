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
    // uc.service_points -> diku_mod_inventory_storage.service_point
    // ServicePoint2 -> ServicePoint
    [DisplayColumn(nameof(Name)), DisplayName("Service Points"), JsonConverter(typeof(JsonPathJsonConverter<ServicePoint2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("service_points", Schema = "uc")]
    public partial class ServicePoint2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.ServicePoint.json")))
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

        [Column("discovery_display_name"), Display(Name = "Discovery Display Name", Order = 4), JsonProperty("discoveryDisplayName"), Required, StringLength(1024)]
        public virtual string DiscoveryDisplayName { get; set; }

        [Column("description"), Display(Order = 5), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("shelving_lag_time"), Display(Name = "Shelving Lag Time", Order = 6), JsonProperty("shelvingLagTime")]
        public virtual int? ShelvingLagTime { get; set; }

        [Column("pickup_location"), Display(Name = "Pickup Location", Order = 7), JsonProperty("pickupLocation")]
        public virtual bool? PickupLocation { get; set; }

        [Column("hold_shelf_expiry_period_duration"), Display(Name = "Hold Shelf Expiry Period Duration", Order = 8), JsonProperty("holdShelfExpiryPeriod.duration"), Required]
        public virtual int? HoldShelfExpiryPeriodDuration { get; set; }

        [Column("hold_shelf_expiry_period_interval_id"), Display(Name = "Hold Shelf Expiry Period Interval", Order = 9), JsonProperty("holdShelfExpiryPeriod.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), Required, StringLength(1024)]
        public virtual string HoldShelfExpiryPeriodInterval { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 10), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 11), InverseProperty("ServicePoint2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 12), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 14), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 15), InverseProperty("ServicePoint2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 16), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(ServicePoint), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 18), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Check Ins", Order = 19)]
        public virtual ICollection<CheckIn2> CheckIn2s { get; set; }

        [Display(Name = "Default Service Point Users", Order = 20)]
        public virtual ICollection<ServicePointUser2> DefaultServicePointUsers { get; set; }

        [Display(Name = "Items", Order = 21)]
        public virtual ICollection<Item2> Item2s { get; set; }

        [Display(Name = "Items 1", Order = 22)]
        public virtual ICollection<Item2> Item2s1 { get; set; }

        [Display(Name = "Loans", Order = 23)]
        public virtual ICollection<Loan2> Loan2s { get; set; }

        [Display(Name = "Loans 1", Order = 24)]
        public virtual ICollection<Loan2> Loan2s1 { get; set; }

        [Display(Name = "Locations", Order = 25)]
        public virtual ICollection<Location2> Location2s { get; set; }

        [Display(Name = "Location Service Points", Order = 26)]
        public virtual ICollection<LocationServicePoint> LocationServicePoints { get; set; }

        [Display(Name = "Payments", Order = 27)]
        public virtual ICollection<Payment2> Payment2s { get; set; }

        [Display(Name = "Requests", Order = 28)]
        public virtual ICollection<Request2> Request2s { get; set; }

        [Display(Name = "Service Point Owners", Order = 29)]
        public virtual ICollection<ServicePointOwner> ServicePointOwners { get; set; }

        [Display(Name = "Service Point Staff Slips", Order = 30), JsonProperty("staffSlips")]
        public virtual ICollection<ServicePointStaffSlip> ServicePointStaffSlips { get; set; }

        [Display(Name = "Service Point User Service Points", Order = 31)]
        public virtual ICollection<ServicePointUserServicePoint> ServicePointUserServicePoints { get; set; }

        [Display(Name = "User Request Preferences", Order = 32)]
        public virtual ICollection<UserRequestPreference2> UserRequestPreference2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Code)} = {Code}, {nameof(DiscoveryDisplayName)} = {DiscoveryDisplayName}, {nameof(Description)} = {Description}, {nameof(ShelvingLagTime)} = {ShelvingLagTime}, {nameof(PickupLocation)} = {PickupLocation}, {nameof(HoldShelfExpiryPeriodDuration)} = {HoldShelfExpiryPeriodDuration}, {nameof(HoldShelfExpiryPeriodInterval)} = {HoldShelfExpiryPeriodInterval}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(ServicePointStaffSlips)} = {(ServicePointStaffSlips != null ? $"{{ {string.Join(", ", ServicePointStaffSlips)} }}" : "")} }}";

        public static ServicePoint2 FromJObject(JObject jObject) => jObject != null ? new ServicePoint2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Code = (string)jObject.SelectToken("code"),
            DiscoveryDisplayName = (string)jObject.SelectToken("discoveryDisplayName"),
            Description = (string)jObject.SelectToken("description"),
            ShelvingLagTime = (int?)jObject.SelectToken("shelvingLagTime"),
            PickupLocation = (bool?)jObject.SelectToken("pickupLocation"),
            HoldShelfExpiryPeriodDuration = (int?)jObject.SelectToken("holdShelfExpiryPeriod.duration"),
            HoldShelfExpiryPeriodInterval = (string)jObject.SelectToken("holdShelfExpiryPeriod.intervalId"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            ServicePointStaffSlips = jObject.SelectToken("staffSlips")?.Where(jt => jt.HasValues).Select(jt => ServicePointStaffSlip.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("code", Code),
            new JProperty("discoveryDisplayName", DiscoveryDisplayName),
            new JProperty("description", Description),
            new JProperty("shelvingLagTime", ShelvingLagTime),
            new JProperty("pickupLocation", PickupLocation),
            new JProperty("holdShelfExpiryPeriod", new JObject(
                new JProperty("duration", HoldShelfExpiryPeriodDuration),
                new JProperty("intervalId", HoldShelfExpiryPeriodInterval))),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("staffSlips", ServicePointStaffSlips?.Select(spss => spss.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
