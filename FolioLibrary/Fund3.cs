using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("funds", Schema = "local")]
    public partial class Fund3
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual int? Id { get; set; }

        [Column("name"), ScaffoldColumn(false), StringLength(128)]
        public virtual string Name { get; set; }

        [Column("number"), ScaffoldColumn(false), StringLength(10)]
        public virtual string Number { get; set; }

        [Column("code"), ScaffoldColumn(false), StringLength(128)]
        public virtual string Code { get; set; }

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

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Number)} = {Number}, {nameof(Code)} = {Code}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUsername)} = {CreationUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUsername)} = {LastWriteUsername}, {nameof(LongId)} = {LongId} }}";
    }
}
