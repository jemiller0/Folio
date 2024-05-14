using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [DisplayColumn(nameof(Name)), DisplayName("User Categories"), Table("user_categories", Schema = "uc")]
    public partial class UserCategory
    {
        [Column("id"), ScaffoldColumn(false), StringLength(7)]
        public virtual string Id { get; set; }

        [Column("name"), Display(Order = 2), Required, StringLength(128)]
        public virtual string Name { get; set; }

        [Column("code"), Display(Order = 3), Required, StringLength(128)]
        public virtual string Code { get; set; }

        [Column("creation_time"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 4), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("creation_username"), Display(Name = "Creation Username", Order = 5), StringLength(128)]
        public virtual string CreationUsername { get; set; }

        [Column("last_write_time"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 6), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Column("last_write_username"), Display(Name = "Last Write Username", Order = 7), Editable(false), StringLength(128)]
        public virtual string LastWriteUsername { get; set; }

        [Display(Name = "Users", Order = 8)]
        public virtual ICollection<User2> User2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Code)} = {Code}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUsername)} = {CreationUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUsername)} = {LastWriteUsername} }}";
    }
}
