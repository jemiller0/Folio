using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.user_departments -> uchicago_mod_users.users
    // UserDepartment -> User
    [DisplayColumn(nameof(Id)), DisplayName("User Departments"), Table("user_departments", Schema = "uc")]
    public partial class UserDepartment
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 3), Required]
        public virtual Guid? UserId { get; set; }

        [Display(Order = 4)]
        public virtual Department2 Department { get; set; }

        [Column("department_id"), Display(Name = "Department", Order = 5)]
        public virtual Guid? DepartmentId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(UserId)} = {UserId}, {nameof(DepartmentId)} = {DepartmentId} }}";

        public static UserDepartment FromJObject(JValue jObject) => jObject != null ? new UserDepartment
        {
            DepartmentId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(DepartmentId);
    }
}
