using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IMapper _mapper;

        public TimesheetService(ITimesheetRepository timesheetRepository, IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }

        public async Task<Timesheet> AddTimesheetEntryAsync(Timesheet timesheet)
        {
            if (timesheet == null)
                throw new ArgumentNullException(nameof(timesheet));

            timesheet.Status = "Pending"; // default status for new timesheet entries

            await _timesheetRepository.AddAsync(timesheet);
            return timesheet;
        }

        public async Task<IEnumerable<Timesheet>> GetTimesheetsByEmployeeAsync(int employeeId)
        {
            if (employeeId <= 0)
                throw new ArgumentException("Invalid Employee ID", nameof(employeeId));

            return await _timesheetRepository.GetByEmployeeIdAsync(employeeId);
        }


        public async Task<bool> ApproveTimesheetAsync(int timesheetId, int managerId)
        {
            var existingTimesheet = await _timesheetRepository.GetByIdAsync(timesheetId);
            if (existingTimesheet == null)
                return false;

            // Business rule: only approve if currently pending
            if (existingTimesheet.Status != "Pending")
                return false;

            existingTimesheet.Status = "Approved";
            await _timesheetRepository.UpdateAsync(existingTimesheet);
            return true;
        }
    }
}
