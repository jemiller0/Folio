using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.tags -> uchicago_mod_tags.tags
    // Tag2 -> Tag
    [DisplayColumn(nameof(Id)), DisplayName("Tags"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("tags", Schema = "uc")]
    public partial class Tag2
    {
        [Column("id"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Column("label"), Display(Order = 2), Required, StringLength(255)]
        public virtual string Label { get; set; }

        [Column("description"), Display(Order = 3), StringLength(255)]
        public virtual string Description { get; set; }

        [Column("creation_time"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 4), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("last_write_time"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 5), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 6)]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by"), Display(Name = "Last Write User", Order = 7), Editable(false)]
        public virtual Guid? LastWriteUserId { get; set; }

        [Display(Name = "Budget Tags", Order = 8)]
        public virtual ICollection<BudgetTag> BudgetTags { get; set; }

        [Display(Name = "Fund Tags", Order = 9)]
        public virtual ICollection<FundTag> FundTags { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Label)} = {Label}, {nameof(Description)} = {Description}, {nameof(CreationTime)} = {CreationTime}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId} }}";

        public static Tag2 FromJObject(JObject jObject) => jObject != null ? new Tag2
        {
            Id = (Guid?)jObject.SelectToken("id")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id)).RemoveNullAndEmptyProperties();
    }
}
