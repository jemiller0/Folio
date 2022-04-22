using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [DisplayColumn(nameof(Name)), DisplayName("Payment Types"), Table("payment_types", Schema = "uc")]
    public partial class PaymentType
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual int? Id { get; set; }

        [Column("code"), Display(Order = 2), Required, StringLength(128)]
        public virtual string Code { get; set; }

        [Column("name"), Display(Order = 3), Required, StringLength(128)]
        public virtual string Name { get; set; }

        [Column("creation_time"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 4), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("creation_username"), Display(Name = "Creation Username", Order = 5), Required, StringLength(128)]
        public virtual string CreationUsername { get; set; }

        [Column("last_write_time"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 6), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Column("last_write_username"), Display(Name = "Last Write Username", Order = 7), Editable(false), StringLength(128)]
        public virtual string LastWriteUsername { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Code)} = {Code}, {nameof(Name)} = {Name}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUsername)} = {CreationUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUsername)} = {LastWriteUsername} }}";
    }
}
