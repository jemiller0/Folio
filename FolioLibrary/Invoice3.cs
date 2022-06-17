using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("invoices", Schema = "local")]
    public partial class Invoice3
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual int? Id { get; set; }

        [Column("vendor_id"), ScaffoldColumn(false)]
        public virtual int? VendorId { get; set; }

        [Column("invoice_date"), ScaffoldColumn(false)]
        public virtual DateTime? InvoiceDate { get; set; }

        [Column("vendor_invoice_id"), ScaffoldColumn(false), StringLength(128)]
        public virtual string VendorInvoiceId { get; set; }

        [Column("vendor_invoice_amount"), ScaffoldColumn(false)]
        public virtual decimal? VendorInvoiceAmount { get; set; }

        [Column("invoice_status_id"), ScaffoldColumn(false)]
        public virtual int? InvoiceStatusId { get; set; }

        [Column("fiscal_year"), ScaffoldColumn(false)]
        public virtual int? FiscalYear { get; set; }

        [Column("approve_time"), ScaffoldColumn(false)]
        public virtual DateTime? ApproveTime { get; set; }

        [Column("approve_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string ApproveUsername { get; set; }

        [Column("cancel_time"), ScaffoldColumn(false)]
        public virtual DateTime? CancelTime { get; set; }

        [Column("cancel_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string CancelUsername { get; set; }

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

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(VendorId)} = {VendorId}, {nameof(InvoiceDate)} = {InvoiceDate}, {nameof(VendorInvoiceId)} = {VendorInvoiceId}, {nameof(VendorInvoiceAmount)} = {VendorInvoiceAmount}, {nameof(InvoiceStatusId)} = {InvoiceStatusId}, {nameof(FiscalYear)} = {FiscalYear}, {nameof(ApproveTime)} = {ApproveTime}, {nameof(ApproveUsername)} = {ApproveUsername}, {nameof(CancelTime)} = {CancelTime}, {nameof(CancelUsername)} = {CancelUsername}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUsername)} = {CreationUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUsername)} = {LastWriteUsername}, {nameof(LongId)} = {LongId} }}";
    }
}
