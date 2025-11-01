namespace FeroTech.Infrastructure.Repositories
{
    public class EmployeeDto
    {
        public string FullName { get; internal set; }
        public string Email { get; internal set; }
        public string Phone { get; internal set; }
        public string Department { get; internal set; }
        public string JobTitle { get; internal set; }
        public bool IsActive { get; internal set; }
    }
}