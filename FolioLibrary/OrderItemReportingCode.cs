using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_item_reporting_codes -> uchicago_mod_orders_storage.po_line
    // OrderItemReportingCode -> OrderItem
    [DisplayColumn(nameof(Id)), DisplayName("Order Item Reporting Codes"), Table("order_item_reporting_codes", Schema = "uc")]
    public partial class OrderItemReportingCode
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Order Item", Order = 2)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 3), Required]
        public virtual Guid? OrderItemId { get; set; }

        [Display(Name = "Reporting Code", Order = 4)]
        public virtual ReportingCode2 ReportingCode { get; set; }

        [Column("reporting_code_id"), Display(Name = "Reporting Code", Order = 5), Required]
        public virtual Guid? ReportingCodeId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(ReportingCodeId)} = {ReportingCodeId} }}";

        public static OrderItemReportingCode FromJObject(JValue jObject) => jObject != null ? new OrderItemReportingCode
        {
            ReportingCodeId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(ReportingCodeId);
    }
}
