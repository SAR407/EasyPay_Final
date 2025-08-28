using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.LeaveRequest;
using EasyPay_Final.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRequestRepository _repository;
        private readonly IMapper _mapper;

        public LeaveService(ILeaveRequestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LeaveRequest> SubmitLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            if (leaveRequest == null)
                throw new ArgumentNullException(nameof(leaveRequest));

            leaveRequest.Status = "Pending"; // default status
            await _repository.AddAsync(leaveRequest);
            return leaveRequest;
        }

        public async Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByEmployeeAsync(int employeeId)
        {
            return await _repository.GetByEmployeeIdAsync(employeeId);
        }

        public async Task<bool> ApproveLeaveAsync(int leaveRequestId, int managerId)
        {
            var request = await _repository.GetByIdAsync(leaveRequestId);
            if (request == null)
                return false;

            request.Status = "Approved";
            await _repository.UpdateAsync(request);
            return true;
        }

        public async Task<bool> RejectLeaveAsync(int leaveRequestId, int managerId)
        {
            var request = await _repository.GetByIdAsync(leaveRequestId);
            if (request == null)
                return false;

            request.Status = "Rejected";
            await _repository.UpdateAsync(request);
            return true;
        }
    }
}
