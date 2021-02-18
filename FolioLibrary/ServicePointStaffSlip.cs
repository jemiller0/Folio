using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.service_point_staff_slips -> diku_mod_inventory_storage.service_point
    // ServicePointStaffSlip -> ServicePoint
    [DisplayColumn(nameof(Id)), DisplayName("Service Point Staff Slips"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("service_point_staff_slips", Schema = "uc")]
    public partial class ServicePointStaffSlip
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Service Point", Order = 2)]
        public virtual ServicePoint2 ServicePoint { get; set; }

        [Column("service_point_id"), Display(Name = "Service Point", Order = 3), Required]
        public virtual Guid? ServicePointId { get; set; }

        [Display(Name = "Staff Slip", Order = 4)]
        public virtual StaffSlip2 StaffSlip { get; set; }

        [Column("staff_slip_id"), Display(Name = "Staff Slip", Order = 5), JsonProperty("id"), Required]
        public virtual Guid? StaffSlipId { get; set; }

        [Column("print_by_default"), Display(Name = "Print By Default", Order = 6), JsonProperty("printByDefault")]
        public virtual bool? PrintByDefault { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ServicePointId)} = {ServicePointId}, {nameof(StaffSlipId)} = {StaffSlipId}, {nameof(PrintByDefault)} = {PrintByDefault} }}";

        public static ServicePointStaffSlip FromJObject(JObject jObject) => jObject != null ? new ServicePointStaffSlip
        {
            StaffSlipId = (Guid?)jObject.SelectToken("id"),
            PrintByDefault = (bool?)jObject.SelectToken("printByDefault")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", StaffSlipId),
            new JProperty("printByDefault", PrintByDefault)).RemoveNullAndEmptyProperties();
    }
}
