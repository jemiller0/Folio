using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("vouchers", Schema = "local")]
    public partial class Voucher3
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual int? Id { get; set; }

        [Column("invoice_date"), ScaffoldColumn(false)]
        public virtual DateTime? InvoiceDate { get; set; }

        [Column("vendor_invoice_id"), ScaffoldColumn(false), StringLength(25)]
        public virtual string VendorInvoiceId { get; set; }

        [Column("vendor_invoice_amount"), ScaffoldColumn(false)]
        public virtual decimal? VendorInvoiceAmount { get; set; }

        [Column("vendor_number"), ScaffoldColumn(false), StringLength(10)]
        public virtual string VendorNumber { get; set; }

        [Column("number"), ScaffoldColumn(false), StringLength(128)]
        public virtual string Number { get; set; }

        [Column("payment_type_id"), ScaffoldColumn(false)]
        public virtual int? PaymentTypeId { get; set; }

        [Column("enclosure"), ScaffoldColumn(false)]
        public virtual bool? Enclosure { get; set; }

        [Column("manual"), ScaffoldColumn(false)]
        public virtual bool? Manual { get; set; }

        [Column("voucher_status_id"), ScaffoldColumn(false)]
        public virtual int? VoucherStatusId { get; set; }

        [Column("notes"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string Notes { get; set; }

        [Column("review_time"), ScaffoldColumn(false)]
        public virtual DateTime? ReviewTime { get; set; }

        [Column("review_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string ReviewUsername { get; set; }

        [Column("approve_time"), ScaffoldColumn(false)]
        public virtual DateTime? ApproveTime { get; set; }

        [Column("approve_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string ApproveUsername { get; set; }

        [Column("cancel_time"), ScaffoldColumn(false)]
        public virtual DateTime? CancelTime { get; set; }

        [Column("cancel_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string CancelUsername { get; set; }

        [Column("export_id"), ScaffoldColumn(false)]
        public virtual int? ExportId { get; set; }

        [Column("export_time"), ScaffoldColumn(false)]
        public virtual DateTime? ExportTime { get; set; }

        [Column("check_number"), ScaffoldColumn(false), StringLength(128)]
        public virtual string CheckNumber { get; set; }

        [Column("completion_time"), ScaffoldColumn(false)]
        public virtual DateTime? CompletionTime { get; set; }

        [Column("completion_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string CompletionUsername { get; set; }

        [Column("mail_time"), ScaffoldColumn(false)]
        public virtual DateTime? MailTime { get; set; }

        [Column("creation_time"), ScaffoldColumn(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("creation_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string CreationUsername { get; set; }

        [Column("last_write_time"), ScaffoldColumn(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Column("last_write_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string LastWriteUsername { get; set; }

        [Column("long_id"), ScaffoldColumn(false)]
        public virtual Guid? LongId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InvoiceDate)} = {InvoiceDate}, {nameof(VendorInvoiceId)} = {VendorInvoiceId}, {nameof(VendorInvoiceAmount)} = {VendorInvoiceAmount}, {nameof(VendorNumber)} = {VendorNumber}, {nameof(Number)} = {Number}, {nameof(PaymentTypeId)} = {PaymentTypeId}, {nameof(Enclosure)} = {Enclosure}, {nameof(Manual)} = {Manual}, {nameof(VoucherStatusId)} = {VoucherStatusId}, {nameof(Notes)} = {Notes}, {nameof(ReviewTime)} = {ReviewTime}, {nameof(ReviewUsername)} = {ReviewUsername}, {nameof(ApproveTime)} = {ApproveTime}, {nameof(ApproveUsername)} = {ApproveUsername}, {nameof(CancelTime)} = {CancelTime}, {nameof(CancelUsername)} = {CancelUsername}, {nameof(ExportId)} = {ExportId}, {nameof(ExportTime)} = {ExportTime}, {nameof(CheckNumber)} = {CheckNumber}, {nameof(CompletionTime)} = {CompletionTime}, {nameof(CompletionUsername)} = {CompletionUsername}, {nameof(MailTime)} = {MailTime}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUsername)} = {CreationUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUsername)} = {LastWriteUsername}, {nameof(LongId)} = {LongId} }}";
    }
}
