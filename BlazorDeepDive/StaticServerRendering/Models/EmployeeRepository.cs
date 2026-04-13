namespace StaticServerRendering.Models
{
    public static class EmployeeRepository
    {
        private static List<Employee> employees = new List<Employee>()
        {
            new Employee { Id = 1, EmployeeId = 1001, Name = "John Doe", Position = "Software Engineer" },
            new Employee { Id = 2, EmployeeId = 1002, Name = "Jane Smith", Position = "Senior Developer" },
            new Employee { Id = 3, EmployeeId = 1003, Name = "Bob Johnson", Position = "Tech Lead" },
            new Employee { Id = 4, EmployeeId = 1004, Name = "Alice Brown", Position = "Software Engineer" },
            new Employee { Id = 5, EmployeeId = 1005, Name = "Charlie Wilson", Position = "QA Engineer" },
            new Employee { Id = 6, EmployeeId = 1006, Name = "Diana Lee", Position = "DevOps Engineer" },
            new Employee { Id = 7, EmployeeId = 1007, Name = "Edward Davis", Position = "Software Engineer" },
            new Employee { Id = 8, EmployeeId = 1008, Name = "Fiona Martinez", Position = "Senior Developer" },
            new Employee { Id = 9, EmployeeId = 1009, Name = "George Taylor", Position = "Project Manager" },
            new Employee { Id = 10, EmployeeId = 1010, Name = "Hannah Clark", Position = "UX Designer" },
            new Employee { Id = 11, EmployeeId = 1011, Name = "Ian Robinson", Position = "Software Engineer" },
            new Employee { Id = 12, EmployeeId = 1012, Name = "Julia White", Position = "QA Engineer" },
            new Employee { Id = 13, EmployeeId = 1013, Name = "Kevin Hall", Position = "Tech Lead" },
        };

        public static void AddEmployee(Employee employee)
        {
            var maxId = employees.Max(e => e.Id);
            employee.Id = maxId + 1;
            employees.Add(employee);
        }

        public static List<Employee> GetEmployees() => employees;

        public static List<Employee> GetEmployeesByPosition(string position)
        {
            return employees.Where(e => e.Position.Equals(position, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static Employee? GetEmployeeById(int id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                return new Employee
                {
                    Id = employee.Id,
                    EmployeeId = employee.EmployeeId,
                    Name = employee.Name,
                    Position = employee.Position
                };
            }

            return null;
        }

        public static void UpdateEmployee(int employeeId, Employee employee)
        {
            if (employeeId != employee.Id) return;

            var employeeToUpdate = employees.FirstOrDefault(e => e.Id == employeeId);
            if (employeeToUpdate != null)
            {
                employeeToUpdate.EmployeeId = employee.EmployeeId;
                employeeToUpdate.Name = employee.Name;
                employeeToUpdate.Position = employee.Position;
            }
        }

        public static void DeleteEmployee(int employeeId)
        {
            var employee = employees.FirstOrDefault(e => e.Id == employeeId);
            if (employee != null)
            {
                employees.Remove(employee);
            }
        }

        public static List<Employee> SearchEmployees(string employeeFilter)
        {
            return employees.Where(e => e.Name.Contains(employeeFilter, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
