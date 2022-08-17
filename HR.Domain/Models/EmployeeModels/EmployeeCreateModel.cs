namespace HR.Domain.Models.EmployeeModels;

public record EmployeeCreateModel(int EmployeeId,
                            string FirstName,
                            string LastName,
                            string Email,
                            string PhoneNumber);
