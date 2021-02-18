using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;

namespace FolioLibrary
{
    // uc.printers -> uc.configurations
    // Printer -> Configuration2
    [CustomValidation(typeof(Printer), nameof(ValidatePrinter)), DisplayColumn(nameof(ComputerName)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("printers", Schema = "uc")]
    public partial class Printer
    {
        public static ValidationResult ValidatePrinter(Printer printer, ValidationContext context)
        {
            var fsc = (FolioServiceContext)context.ObjectInstance;
            if (printer.ComputerName != null && fsc.Printers($"id <> \"{printer.Id}\"").Any(p => p.ComputerName == printer.ComputerName)) return new ValidationResult("Computer Name already exists");
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateComputerName(string value)
        {
            try
            {
                Dns.GetHostEntry(value);
                return ValidationResult.Success;
            }
            catch (Exception)
            {
                return new ValidationResult("The Computer Name field is invalid.");
            }
        }

        [Column("id"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Column("computer_name"), CustomValidation(typeof(Printer), nameof(ValidateComputerName)), Display(Name = "Computer Name", Order = 2), Required, StringLength(1024)]
        public virtual string ComputerName { get; set; }

        [Column("name"), Display(Order = 3), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("left"), Display(Order = 4), Range(-100, 100), Required]
        public virtual int? Left { get; set; }

        [Column("top"), Display(Order = 5), Range(-100, 100), Required]
        public virtual int? Top { get; set; }

        [Column("width"), Display(Order = 6), Range(25, 1100)]
        public virtual int? Width { get; set; }

        [Column("height"), Display(Order = 7), Range(25, 1100)]
        public virtual int? Height { get; set; }

        [Column("enabled"), Display(Order = 8)]
        public virtual bool? Enabled { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 9), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 10), InverseProperty("Printers")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 11), Editable(false)]
        public virtual Guid? CreationUserId { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 12), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 13), InverseProperty("Printers1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 14), Editable(false)]
        public virtual Guid? LastWriteUserId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ComputerName)} = {ComputerName}, {nameof(Name)} = {Name}, {nameof(Left)} = {Left}, {nameof(Top)} = {Top}, {nameof(Width)} = {Width}, {nameof(Height)} = {Height}, {nameof(Enabled)} = {Enabled}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId} }}";

        public static Printer FromJObject(JObject jObject)
        {
            var jo = JObject.Parse((string)jObject["value"]);
            return new Printer
            {
                Id = (Guid?)jObject.SelectToken("id"),
                Enabled = (bool?)jObject.SelectToken("enabled"),
                ComputerName = (string)jo.SelectToken("computerName"),
                Name = (string)jo.SelectToken("name"),
                Left = (int?)jo.SelectToken("left"),
                Top = (int?)jo.SelectToken("top"),
                Width = (int?)jo.SelectToken("width"),
                Height = (int?)jo.SelectToken("height"),
                CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
                CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
                LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
                LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId")
            };
        }

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("module", "uc"),
            new JProperty("configName", "printers"),
            new JProperty("code", Id),
            new JProperty("enabled", Enabled),
            new JProperty("value", new JObject(
                new JProperty("computerName", ComputerName),
                new JProperty("name", Name),
                new JProperty("left", Left),
                new JProperty("top", Top),
                new JProperty("width", Width),
                new JProperty("height", Height)).ToString()),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("updatedDate", LastWriteTime),
                new JProperty("updatedByUserId", LastWriteUserId)))).RemoveNullAndEmptyProperties();
    }
}
