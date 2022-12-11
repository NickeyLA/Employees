using Microsoft.AspNetCore.Http;

namespace Employees_Test.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public string department { get; set; }
        public string Phone { get; set; }
        public IFormFile Image { get; set; }
    }
}
