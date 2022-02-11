using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.batch_voucher_export_config_weekdays -> uchicago_mod_invoice_storage.batch_voucher_export_configs
    // BatchVoucherExportConfigWeekday -> BatchVoucherExportConfig
    [DisplayColumn(nameof(Content)), DisplayName("Batch Voucher Export Config Weekdays"), Table("batch_voucher_export_config_weekdays", Schema = "uc")]
    public partial class BatchVoucherExportConfigWeekday
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Batch Voucher Export Config", Order = 2)]
        public virtual BatchVoucherExportConfig2 BatchVoucherExportConfig { get; set; }

        [Column("batch_voucher_export_config_id"), Display(Name = "Batch Voucher Export Config", Order = 3), Required]
        public virtual Guid? BatchVoucherExportConfigId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(BatchVoucherExportConfigId)} = {BatchVoucherExportConfigId}, {nameof(Content)} = {Content} }}";

        public static BatchVoucherExportConfigWeekday FromJObject(JValue jObject) => jObject != null ? new BatchVoucherExportConfigWeekday
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
