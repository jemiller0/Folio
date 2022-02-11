using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_item_reference_numbers -> uchicago_mod_orders_storage.po_line
    // OrderItemReferenceNumber -> OrderItem
    [DisplayColumn(nameof(Id)), DisplayName("Order Item Reference Numbers"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("order_item_reference_numbers", Schema = "uc")]
    public partial class OrderItemReferenceNumber
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Order Item", Order = 2)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 3)]
        public virtual Guid? OrderItemId { get; set; }

        [Column("ref_number"), Display(Name = "Reference Number", Order = 4), JsonProperty("refNumber"), StringLength(1024)]
        public virtual string ReferenceNumber { get; set; }

        [Column("ref_number_type"), Display(Name = "Reference Number Type", Order = 5), JsonProperty("refNumberType"), StringLength(1024)]
        public virtual string ReferenceNumberType { get; set; }

        [Column("vendor_details_source"), Display(Name = "Vendor Details Source", Order = 6), JsonProperty("vendorDetailsSource"), StringLength(1024)]
        public virtual string VendorDetailsSource { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(ReferenceNumber)} = {ReferenceNumber}, {nameof(ReferenceNumberType)} = {ReferenceNumberType}, {nameof(VendorDetailsSource)} = {VendorDetailsSource} }}";

        public static OrderItemReferenceNumber FromJObject(JObject jObject) => jObject != null ? new OrderItemReferenceNumber
        {
            ReferenceNumber = (string)jObject.SelectToken("refNumber"),
            ReferenceNumberType = (string)jObject.SelectToken("refNumberType"),
            VendorDetailsSource = (string)jObject.SelectToken("vendorDetailsSource")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("refNumber", ReferenceNumber),
            new JProperty("refNumberType", ReferenceNumberType),
            new JProperty("vendorDetailsSource", VendorDetailsSource)).RemoveNullAndEmptyProperties();
    }
}
