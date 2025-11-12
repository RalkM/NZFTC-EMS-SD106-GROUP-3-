
// Models/Employee.cs
namespace NZFTC_EMS.Models;
public class Employee
{
    public int Id { get; set; }                  // PK
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public DateTime HiredAt { get; set; } = DateTime.UtcNow;

    public int DepartmentId { get; set; }        // FK
    public Department? Department { get; set; }
}