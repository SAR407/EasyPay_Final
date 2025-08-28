using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Repositories;
using System;
using System.Threading.Tasks;

namespace EasyPay_Final.Services
{
    public class PayrollConfigService : IPayrollConfigService
    {
        private readonly IRepository<PayrollPolicy> _policyRepository;

        public PayrollConfigService(IRepository<PayrollPolicy> policyRepository)
        {
            _policyRepository = policyRepository;
        }

        public async Task ConfigurePayrollRulesAsync(PayrollPolicy config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            // Check if a payroll policy already exists
            var existingPolicies = await _policyRepository.GetAllAsync();

            if (existingPolicies != null && existingPolicies.Any())
            {
                var existing = existingPolicies.First();
                existing.TaxRate = config.TaxRate;
                existing.Allowances = config.Allowances;
                existing.Deductions = config.Deductions;

                await _policyRepository.UpdateAsync(existing);
            }
            else
            {
                await _policyRepository.AddAsync(config);
            }
        }

        public async Task<PayrollPolicy> GetPayrollConfigAsync()
        {
            var configs = await _policyRepository.GetAllAsync();
            return configs.FirstOrDefault(); // Assuming only one config exists
        }
    }
}
