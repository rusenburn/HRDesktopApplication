namespace HR.Domain.Models.EmployeeModels;

public record EmployeeUpdateModel(int EmployeeId,
                            string FirstName,
                            string LastName,
                            string Email,
                            string PhoneNumber,
                            int? ManagerId);