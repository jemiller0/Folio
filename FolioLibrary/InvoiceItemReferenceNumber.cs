using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.invoice_item_reference_numbers -> uchicago_mod_invoice_storage.invoice_lines
    // InvoiceItemReferenceNumber -> InvoiceItem
    [DisplayColumn(nameof(Id)), DisplayName("Invoice Item Reference Numbers"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("invoice_item_reference_numbers", Schema = "uc")]
    public partial class InvoiceItemReferenceNumber
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Invoice Item", Order = 2)]
        public virtual InvoiceItem2 InvoiceItem { get; set; }

        [Column("invoice_item_id"), Display(Name = "Invoice Item", Order = 3)]
        public virtual Guid? InvoiceItemId { get; set; }

        [Column("ref_number"), Display(Name = "Reference Number", Order = 4), JsonProperty("refNumber"), StringLength(1024)]
        public virtual string ReferenceNumber { get; set; }

        [Column("ref_number_type"), Display(Name = "Reference Number Type", Order = 5), JsonProperty("refNumberType"), StringLength(1024)]
        public virtual string ReferenceNumberType { get; set; }

        [Column("vendor_details_source"), Display(Name = "Vendor Details Source", Order = 6), JsonProperty("vendorDetailsSource"), StringLength(1024)]
        public virtual string VendorDetailsSource { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InvoiceItemId)} = {InvoiceItemId}, {nameof(ReferenceNumber)} = {ReferenceNumber}, {nameof(ReferenceNumberType)} = {ReferenceNumberType}, {nameof(VendorDetailsSource)} = {VendorDetailsSource} }}";

        public static InvoiceItemReferenceNumber FromJObject(JObject jObject) => jObject != null ? new InvoiceItemReferenceNumber
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
