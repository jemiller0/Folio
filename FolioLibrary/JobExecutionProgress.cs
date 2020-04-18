using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("job_execution_progress", Schema = "diku_mod_source_record_manager")]
    public partial class JobExecutionProgress
    {
        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Display(Name = "Job Execution", Order = 3)]
        public virtual JobExecution JobExecution { get; set; }

        [Column("jobexecutionid"), Display(Name = "Job Execution", Order = 4), Editable(false), ForeignKey("JobExecution")]
        public virtual Guid? Jobexecutionid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(Jobexecutionid)} = {Jobexecutionid} }}";
    }
}
