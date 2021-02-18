using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FolioLibrary
{
    // uc.settings -> uc.configurations
    // Setting -> Configuration2
    [CustomValidation(typeof(Setting), nameof(ValidateSetting)), DisplayColumn(nameof(Name)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("settings", Schema = "uc")]
    public partial class Setting
    {
        public static ValidationResult ValidateSetting(Setting setting, ValidationContext context)
        {
            var fsc = (FolioServiceContext)context.ObjectInstance;
            if (setting.Name != null && fsc.Settings($"id <> \"{setting.Id}\"").Any(s => s.Name == setting.Name)) return new ValidationResult("Name already exists");
            return ValidationResult.Success;
        }

        [Column("id"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("orientation"), Display(Order = 3), Required]
        public virtual Orientation? Orientation { get; set; }

        [Column("font_family"), Display(Name = "Font Family", Order = 4), RegularExpression(@"^(Agency FB|Algerian|Arial|Arial Black|Arial Narrow|Arial Rounded MT Bold|Arial Unicode MS|Bahnschrift|Bahnschrift Condensed|Bahnschrift Light|Bahnschrift Light Condensed|Bahnschrift Light SemiCondensed|Bahnschrift SemiBold|Bahnschrift SemiBold Condensed|Bahnschrift SemiBold SemiConden|Bahnschrift SemiCondensed|Bahnschrift SemiLight|Bahnschrift SemiLight Condensed|Bahnschrift SemiLight SemiConde|BarCode|Baskerville Old Face|Bauhaus 93|Bell MT|Berlin Sans FB|Berlin Sans FB Demi|Bernard MT Condensed|Blackadder ITC|Bodoni MT|Bodoni MT Black|Bodoni MT Condensed|Bodoni MT Poster Compressed|Book Antiqua|Bookman Old Style|Bookshelf Symbol 7|Bradley Hand ITC|Britannic Bold|Broadway|Brush Script MT|Calibri|Calibri Light|Californian FB|Calisto MT|Cambria|Cambria Math|Candara|Candara Light|Cascadia Code|Cascadia Code ExtraLight|Cascadia Code Light|Cascadia Code SemiBold|Cascadia Code SemiLight|Cascadia Mono|Cascadia Mono ExtraLight|Cascadia Mono Light|Cascadia Mono SemiBold|Cascadia Mono SemiLight|Castellar|Centaur|Century|Century Gothic|Century Schoolbook|Chiller|Colonna MT|Comic Sans MS|Consolas|Constantia|Cooper Black|Copperplate Gothic Bold|Copperplate Gothic Light|Corbel|Corbel Light|Courier New|Curlz MT|Dubai|Dubai Light|Dubai Medium|Ebrima|Edwardian Script ITC|Elephant|Engravers MT|Eras Bold ITC|Eras Demi ITC|Eras Light ITC|Eras Medium ITC|Felix Titling|Footlight MT Light|Forte|Franklin Gothic Book|Franklin Gothic Demi|Franklin Gothic Demi Cond|Franklin Gothic Heavy|Franklin Gothic Medium|Franklin Gothic Medium Cond|Freestyle Script|French Script MT|Gabriola|Gadugi|Garamond|Georgia|Gigi|Gill Sans MT|Gill Sans MT Condensed|Gill Sans MT Ext Condensed Bold|Gill Sans Ultra Bold|Gill Sans Ultra Bold Condensed|Gloucester MT Extra Condensed|Goudy Old Style|Goudy Stout|Haettenschweiler|Harlow Solid Italic|Harrington|High Tower Text|HoloLens MDL2 Assets|Impact|Imprint MT Shadow|Informal Roman|Ink Free|Javanese Text|Jokerman|Juice ITC|Kristen ITC|Kunstler Script|Leelawadee UI|Leelawadee UI Semilight|Lucida Bright|Lucida Calligraphy|Lucida Console|Lucida Fax|Lucida Handwriting|Lucida Sans|Lucida Sans Typewriter|Lucida Sans Unicode|Magneto|Maiandra GD|Malgun Gothic|Malgun Gothic Semilight|Marlett|Matura MT Script Capitals|Microsoft Himalaya|Microsoft JhengHei|Microsoft JhengHei Light|Microsoft JhengHei UI|Microsoft JhengHei UI Light|Microsoft New Tai Lue|Microsoft PhagsPa|Microsoft Sans Serif|Microsoft Tai Le|Microsoft YaHei|Microsoft YaHei Light|Microsoft YaHei UI|Microsoft YaHei UI Light|Microsoft Yi Baiti|MingLiU-ExtB|MingLiU_HKSCS-ExtB|Mistral|Modern No. 20|Mongolian Baiti|Monotype Corsiva|MS Gothic|MS Outlook|MS PGothic|MS Reference Sans Serif|MS Reference Specialty|MS UI Gothic|MT Extra|MV Boli|Myanmar Text|Niagara Engraved|Niagara Solid|Nirmala UI|Nirmala UI Semilight|NSimSun|OCR A Extended|Old English Text MT|Onyx|Palace Script MT|Palatino Linotype|Papyrus|Parchment|Perpetua|Perpetua Titling MT|Playbill|PMingLiU-ExtB|Poor Richard|Pristina|Rage Italic|Ravie|Rockwell|Rockwell Condensed|Rockwell Extra Bold|Script MT Bold|Segoe MDL2 Assets|Segoe Print|Segoe Script|Segoe UI|Segoe UI Black|Segoe UI Emoji|Segoe UI Historic|Segoe UI Light|Segoe UI Semibold|Segoe UI Semilight|Segoe UI Symbol|Showcard Gothic|SimSun|SimSun-ExtB|Sitka Banner|Sitka Display|Sitka Heading|Sitka Small|Sitka Subheading|Sitka Text|Snap ITC|Stencil|Sylfaen|Symbol|Tahoma|Tempus Sans ITC|Times New Roman|Trebuchet MS|Tw Cen MT|Tw Cen MT Condensed|Tw Cen MT Condensed Extra Bold|Verdana|Viner Hand ITC|Vivaldi|Vladimir Script|Webdings|Wide Latin|Wingdings|Wingdings 2|Wingdings 3|Yu Gothic|Yu Gothic Light|Yu Gothic Medium|Yu Gothic UI|Yu Gothic UI Light|Yu Gothic UI Semibold|Yu Gothic UI Semilight)$"), Required, StringLength(1024)]
        public virtual string FontFamily { get; set; }

        [Column("font_size"), Display(Name = "Font Size", Order = 5), Required]
        public virtual int? FontSize { get; set; }

        [Column("font_weight"), Display(Name = "Font Weight", Order = 6), Required]
        public virtual FontWeight? FontWeight { get; set; }

        [Column("enabled"), Display(Order = 7)]
        public virtual bool? Enabled { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 8), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 9), InverseProperty("Settings")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 10), Editable(false)]
        public virtual Guid? CreationUserId { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 11), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 12), InverseProperty("Settings1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 13), Editable(false)]
        public virtual Guid? LastWriteUserId { get; set; }

        [Display(Name = "Location Settings", Order = 14)]
        public virtual ICollection<LocationSetting> LocationSettings { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Orientation)} = {Orientation}, {nameof(FontFamily)} = {FontFamily}, {nameof(FontSize)} = {FontSize}, {nameof(FontWeight)} = {FontWeight}, {nameof(Enabled)} = {Enabled}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId} }}";

        public static Setting FromJObject(JObject jObject)
        {
            var jo = JObject.Parse((string)jObject["value"]);
            return new Setting
            {
                Id = (Guid?)jObject.SelectToken("id"),
                Enabled = (bool?)jObject.SelectToken("enabled"),
                Name = (string)jo.SelectToken("name"),
                Orientation = (Orientation?)(int?)jo.SelectToken("orientation"),
                FontFamily = (string)jo.SelectToken("fontFamily"),
                FontSize = (int?)jo.SelectToken("fontSize"),
                FontWeight = (FontWeight)(int?)jo.SelectToken("fontWeight"),
                CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
                CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
                LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
                LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId")
            };
        }

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("module", "uc"),
            new JProperty("configName", "settings"),
            new JProperty("code", Id),
            new JProperty("enabled", Enabled),
            new JProperty("value", new JObject(
                new JProperty("name", Name),
                new JProperty("orientation", Orientation),
                new JProperty("fontFamily", FontFamily),
                new JProperty("fontSize", FontSize),
                new JProperty("fontWeight", FontWeight)).ToString()),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("updatedDate", LastWriteTime),
                new JProperty("updatedByUserId", LastWriteUserId)))).RemoveNullAndEmptyProperties();
    }
}
