using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("audit_loan", Schema = "diku_mod_circulation_storage")]
    public partial class AuditLoan
    {
        [Column("_id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("orig_id"), Display(Name = "Orig Id", Order = 2), Editable(false)]
        public virtual Guid? OrigId { get; set; }

        [Column("operation"), Display(Order = 3), Editable(false), StringLength(1)]
        public virtual string Operation { get; set; }

        [Column("jsonb"), DataType(DataType.MultilineText), Display(Order = 4)]
        public virtual string Content { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 5), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrigId)} = {OrigId}, {nameof(Operation)} = {Operation}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime} }}";
    }
}
