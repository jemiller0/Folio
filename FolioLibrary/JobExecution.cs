using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    [Table("job_executions", Schema = "diku_mod_source_record_manager")]
    public partial class JobExecution
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.JobExecution.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(JobExecution), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<JobExecutionProgress> JobExecutionProgresses { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<JobExecutionSourceChunk> JobExecutionSourceChunks { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<JobMonitoring> JobMonitorings { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<JournalRecord> JournalRecords { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content} }}";

        public static JobExecution FromJObject(JObject jObject) => jObject != null ? new JobExecution
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = jObject.ToString()
        } : null;

        public JObject ToJObject() => JObject.Parse(Content);
    }
}
