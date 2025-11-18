using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class EmployeeInfo
    {
        [Key]
        [Required]
        [StringLength(100)]
        public string EmpID { get; set; } = string.Empty;   // Primary Key

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AutoId { get; set; }

        [Required]
        [StringLength(200)]
        public string EmpName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string EmpCode { get; set; } = string.Empty;

        [StringLength(255)]
        public string? EmpAddress { get; set; }

        public DateTime? EmpDateOfBirth { get; set; }

        [StringLength(50)]
        public string? EmpGender { get; set; }

        [StringLength(50)]
        public string? EmpPhone { get; set; }

        [StringLength(255)]
        public string? EmpDescription { get; set; }

        [StringLength(100)]
        public string? EmpRoleId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [StringLength(50)]
        public string? EmpEmail { get; set; }

        [StringLength(50)]
        public string? EmpBloodGroup { get; set; }

        public DateTime? EmpJoiningDate { get; set; }

        [StringLength(50)]
        public string? EmpUserId { get; set; }

        [StringLength(50)]
        public string? EmpNationalID { get; set; }

        public DateTime? EmpDateOfResignation { get; set; }

        public DateTime? EmpDateOfLeft { get; set; }

        [StringLength(255)]
        [Required]
        public string BranchId { get; set; }


        [StringLength(255)]
        [Required]
        public string CompanyId { get; set; }
    }
}
