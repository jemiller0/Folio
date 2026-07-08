using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.donors -> uc.organizations
    // Donor2 -> Organization2
    [DisplayColumn(nameof(Name)), DisplayName("Donors"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("donors", Schema = "uc")]
    public partial class Donor2
    {
        [Column("id"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("code"), Display(Order = 3), StringLength(1024)]
        public virtual string Code { get; set; }

        [Column("description"), Display(Order = 4), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("status"), Display(Order = 5), StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 6), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 7), InverseProperty("Donor2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 8), Editable(false)]
        public virtual Guid? CreationUserId { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 9), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 10), InverseProperty("Donor2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 11), Editable(false)]
        public virtual Guid? LastWriteUserId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Code)} = {Code}, {nameof(Description)} = {Description}, {nameof(Status)} = {Status}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId} }}";

        public static Donor2 FromJObject(JObject jObject) => jObject != null ? new Donor2
        {
            Id = (Guid?)jObject.SelectToken("id")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id)).RemoveNullAndEmptyProperties();
    }
}
