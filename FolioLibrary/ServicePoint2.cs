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
    // uc.service_points -> uchicago_mod_inventory_storage.service_point
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

        [Column("hold_shelf_closed_library_date_management"), Display(Name = "Hold Shelf Closed Library Date Management", Order = 10), JsonProperty("holdShelfClosedLibraryDateManagement"), RegularExpression(@"^(Keep_the_current_due_date|Move_to_the_end_of_the_previous_open_day|Move_to_the_end_of_the_next_open_day|Keep_the_current_due_date_time|Move_to_end_of_current_service_point_hours|Move_to_beginning_of_next_open_service_point_hours)$"), StringLength(1024)]
        public virtual string HoldShelfClosedLibraryDateManagement { get; set; }

        [Column("default_check_in_action_for_use_at_location"), Display(Name = "Default Check In Action For Use At Location", Order = 11), JsonProperty("defaultCheckInActionForUseAtLocation"), RegularExpression(@"^(Keep_on_hold_shelf|Close_loan_and_return_item|Ask_for_action)$"), StringLength(1024)]
        public virtual string DefaultCheckInActionForUseAtLocation { get; set; }

        [Column("ecs_request_routing"), Display(Name = "Ecs Request Routing", Order = 12), JsonProperty("ecsRequestRouting")]
        public virtual bool? EcsRequestRouting { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 13), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 14), InverseProperty("ServicePoint2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 15), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 17), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 18), InverseProperty("ServicePoint2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 19), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(ServicePoint), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 21), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Check Ins", Order = 22)]
        public virtual ICollection<CheckIn2> CheckIn2s { get; set; }

        [Display(Name = "Default Service Point Users", Order = 23)]
        public virtual ICollection<ServicePointUser2> DefaultServicePointUsers { get; set; }

        [Display(Name = "Items", Order = 24)]
        public virtual ICollection<Item2> Item2s { get; set; }

        [Display(Name = "Items 1", Order = 25)]
        public virtual ICollection<Item2> Item2s1 { get; set; }

        [Display(Name = "Loans", Order = 26)]
        public virtual ICollection<Loan2> Loan2s { get; set; }

        [Display(Name = "Loans 1", Order = 27)]
        public virtual ICollection<Loan2> Loan2s1 { get; set; }

        [Display(Name = "Locations", Order = 28)]
        public virtual ICollection<Location2> Location2s { get; set; }

        [Display(Name = "Location Service Points", Order = 29)]
        public virtual ICollection<LocationServicePoint> LocationServicePoints { get; set; }

        [Display(Name = "Requests", Order = 30)]
        public virtual ICollection<Request2> Request2s { get; set; }

        [Display(Name = "Service Point Owners", Order = 31)]
        public virtual ICollection<ServicePointOwner> ServicePointOwners { get; set; }

        [Display(Name = "Service Point Staff Slips", Order = 32), JsonProperty("staffSlips")]
        public virtual ICollection<ServicePointStaffSlip> ServicePointStaffSlips { get; set; }

        [Display(Name = "Service Point User Service Points", Order = 33)]
        public virtual ICollection<ServicePointUserServicePoint> ServicePointUserServicePoints { get; set; }

        [Display(Name = "User Request Preferences", Order = 34)]
        public virtual ICollection<UserRequestPreference2> UserRequestPreference2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Code)} = {Code}, {nameof(DiscoveryDisplayName)} = {DiscoveryDisplayName}, {nameof(Description)} = {Description}, {nameof(ShelvingLagTime)} = {ShelvingLagTime}, {nameof(PickupLocation)} = {PickupLocation}, {nameof(HoldShelfExpiryPeriodDuration)} = {HoldShelfExpiryPeriodDuration}, {nameof(HoldShelfExpiryPeriodInterval)} = {HoldShelfExpiryPeriodInterval}, {nameof(HoldShelfClosedLibraryDateManagement)} = {HoldShelfClosedLibraryDateManagement}, {nameof(DefaultCheckInActionForUseAtLocation)} = {DefaultCheckInActionForUseAtLocation}, {nameof(EcsRequestRouting)} = {EcsRequestRouting}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(ServicePointStaffSlips)} = {(ServicePointStaffSlips != null ? $"{{ {string.Join(", ", ServicePointStaffSlips)} }}" : "")} }}";

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
            HoldShelfClosedLibraryDateManagement = (string)jObject.SelectToken("holdShelfClosedLibraryDateManagement"),
            DefaultCheckInActionForUseAtLocation = (string)jObject.SelectToken("defaultCheckInActionForUseAtLocation"),
            EcsRequestRouting = (bool?)jObject.SelectToken("ecsRequestRouting"),
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
            new JProperty("holdShelfClosedLibraryDateManagement", HoldShelfClosedLibraryDateManagement),
            new JProperty("defaultCheckInActionForUseAtLocation", DefaultCheckInActionForUseAtLocation),
            new JProperty("ecsRequestRouting", EcsRequestRouting),
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
