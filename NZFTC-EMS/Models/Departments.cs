// Models/Department.cs
namespace NZFTC_EMS.Models;
public class Department
{
    public int Id { get; set; }                  // PK
    public string Name { get; set; } = "";
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
