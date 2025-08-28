using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Benefit;
using EasyPay_Final.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Services
{
    public class BenefitService : IBenefitService
    {
        private readonly IBenefitRepository _repository;
        private readonly IMapper _mapper;

        public BenefitService(IBenefitRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Benefit> AddBenefitAsync(Benefit benefit)
        {
            if (benefit == null)
                throw new ArgumentNullException(nameof(benefit));

            await _repository.AddAsync(benefit);
            return benefit;
        }

        public async Task<IEnumerable<Benefit>> GetBenefitsByEmployeeAsync(int employeeId)
        {
            if (employeeId <= 0)
                throw new ArgumentException("Invalid employee ID.", nameof(employeeId));

            return await _repository.GetByEmployeeIdAsync(employeeId);
        }

        public async Task<bool> UpdateBenefitAsync(int benefitId, Benefit benefit)
        {
            if (benefit == null)
                throw new ArgumentNullException(nameof(benefit));

            var existingBenefit = await _repository.GetByIdAsync(benefitId);
            if (existingBenefit == null)
                return false;

            // Update fields
            existingBenefit.BenefitType = benefit.BenefitType;
            existingBenefit.Amount = benefit.Amount;
            existingBenefit.EmployeeId = benefit.EmployeeId;

            await _repository.UpdateAsync(existingBenefit);
            return true;
        }

        // Optional helper if you want to directly work with DTOs in controllers
        public async Task<IEnumerable<BenefitResponseDTO>> GetBenefitsByEmployeeAsDtoAsync(int employeeId)
        {
            var benefits = await GetBenefitsByEmployeeAsync(employeeId);
            return _mapper.Map<IEnumerable<BenefitResponseDTO>>(benefits);
        }
    }
}
