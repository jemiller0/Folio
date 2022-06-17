using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("donors", Schema = "local")]
    public partial class Donor
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual int? Id { get; set; }

        [Column("name"), ScaffoldColumn(false), StringLength(256)]
        public virtual string Name { get; set; }

        [Column("code"), ScaffoldColumn(false), StringLength(128)]
        public virtual string Code { get; set; }

        [Column("amount"), ScaffoldColumn(false)]
        public virtual decimal? Amount { get; set; }

        [Column("report"), ScaffoldColumn(false), StringLength(1)]
        public virtual string Report { get; set; }

        [Column("creation_time"), ScaffoldColumn(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("creation_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string CreationUsername { get; set; }

        [Column("last_write_time"), ScaffoldColumn(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Column("last_write_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string LastWriteUsername { get; set; }

        [Column("enabled"), ScaffoldColumn(false)]
        public virtual bool? Enabled { get; set; }

        [Column("public_display"), ScaffoldColumn(false), StringLength(256)]
        public virtual string PublicDisplay { get; set; }

        [Column("notes"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string Notes { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Code)} = {Code}, {nameof(Amount)} = {Amount}, {nameof(Report)} = {Report}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUsername)} = {CreationUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUsername)} = {LastWriteUsername}, {nameof(Enabled)} = {Enabled}, {nameof(PublicDisplay)} = {PublicDisplay}, {nameof(Notes)} = {Notes} }}";
    }
}
