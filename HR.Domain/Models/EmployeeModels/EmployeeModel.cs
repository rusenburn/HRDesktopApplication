namespace HR.Domain.Models.EmployeeModels;
public record EmployeeModel(int EmployeeId,
                            string FirstName,
                            string LastName,
                            string Email,
                            string PhoneNumber,
                            DateTime? HireDate,
                            int? Salary,
                            int? JobId,
                            int? ManagerId,
                            int? DepartmentId);
