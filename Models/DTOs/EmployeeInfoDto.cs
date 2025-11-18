namespace GravyFoodsApi.DTOs
{
    public class EmployeeInfoDto
    {
        public string EmpID { get; set; } = string.Empty;
        public string EmpName { get; set; } = string.Empty;

        public string EmpCode { get; set; } = string.Empty;

        public string? EmpAddress { get; set; }

        public DateTime? EmpDateOfBirth { get; set; }

        public string? EmpGender { get; set; }

        public string? EmpPhone { get; set; }

        public string? EmpDescription { get; set; }

        public string? EmpRoleId { get; set; }

        public bool IsActive { get; set; }

        public string? EmpEmail { get; set; }

        public string? EmpBloodGroup { get; set; }

        public DateTime? EmpJoiningDate { get; set; }

        public string? EmpUserId { get; set; }

        public string? EmpNationalID { get; set; }

        public DateTime? EmpDateOfResignation { get; set; }

        public DateTime? EmpDateOfLeft { get; set; }

        public string? BranchId { get; set; }

        public string? CompanyId { get; set; }
    }
}
