using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("tags", Schema = "uchicago_mod_tags")]
    public partial class Tag
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 2), Editable(false)]
        public virtual Guid? CreationUserId { get; set; }

        [Column("label"), Display(Order = 3), Required, StringLength(255)]
        public virtual string Label { get; set; }

        [Column("description"), Display(Order = 4), StringLength(255)]
        public virtual string Description { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 5), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 6), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Column("updated_by"), Display(Name = "Updated By", Order = 7)]
        public virtual Guid? UpdatedBy { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Label)} = {Label}, {nameof(Description)} = {Description}, {nameof(CreationTime)} = {CreationTime}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(UpdatedBy)} = {UpdatedBy} }}";
    }
}
