using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Employee;
using EasyPay_Final.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeResponseDTO>> GetAllEmployeesAsync()
        {
            var employees = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeResponseDTO>>(employees);
        }

        public async Task<EmployeeResponseDTO> GetEmployeeByIdAsync(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
                throw new Exception($"Employee with ID {id} not found.");

            return _mapper.Map<EmployeeResponseDTO>(employee);
        }


        //public async Task<EmployeeResponseDTO> CreateEmployeeAsync(EmployeeCreateDTO createDTO)
        //{
        //    if (createDTO == null)
        //        throw new ArgumentNullException(nameof(createDTO));

        //    var res=await _repository.AddAsync(createDTO);
        //    return res;
        //}
        public async Task<EmployeeResponseDTO> CreateEmployeeAsync(EmployeeCreateDTO createDTO)
        {
            if (createDTO == null)
                throw new ArgumentNullException(nameof(createDTO));

            // Map DTO to Entity
            var employee = _mapper.Map<Employee>(createDTO);

            // Save entity
            var savedEmployee = await _repository.AddAsync(employee);

            // Map saved entity to Response DTO
            return _mapper.Map<EmployeeResponseDTO>(savedEmployee);
        }



        public async Task<Employee> UpdateEmployeeAsync(int id, Employee employee)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return null;

            // Map updated fields
            existing.FirstName = employee.FirstName;
            existing.LastName = employee.LastName;
            existing.Email = employee.Email;
            existing.BasicSalary = employee.BasicSalary;
            // ... map other existing fields

            // ... map other fields as needed

            await _repository.UpdateAsync(existing);
            return existing;
        }


        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var existingEmployee = await _repository.GetByIdAsync(id);
            if (existingEmployee == null)
                return false;

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
