using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Employee;

namespace EasyPay_Final.Interfaces
{
    public interface IEmployeeService
    {
        //Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<IEnumerable<EmployeeResponseDTO>> GetAllEmployeesAsync();

        //Task<Employee> GetEmployeeByIdAsync(int id);
        Task<EmployeeResponseDTO> GetEmployeeByIdAsync(int id);
        Task<EmployeeResponseDTO> CreateEmployeeAsync(EmployeeCreateDTO dto);
        Task<Employee> UpdateEmployeeAsync(int id, Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
    }

}
