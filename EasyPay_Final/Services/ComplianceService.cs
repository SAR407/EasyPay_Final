using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Compilance;
using EasyPay_Final.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Services
{
    public class ComplianceService : IComplianceService
    {
        private readonly IComplianceReportRepository _repository;
        private readonly IMapper _mapper;

        public ComplianceService(IComplianceReportRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ComplianceReport>> GenerateTaxReportsAsync(DateTime startDate, DateTime endDate)
        {
            // In real cases, you might run calculations here or query payroll data
            var taxReports = await _repository.GetByTypeAsync("Tax");

            // Filter by date range
            var filteredReports = new List<ComplianceReport>();
            foreach (var report in taxReports)
            {
                if (report.StartDate >= startDate && report.EndDate <= endDate)
                    filteredReports.Add(report);
            }

            return filteredReports;
        }

        public async Task<IEnumerable<ComplianceReport>> GenerateStatutoryReportsAsync()
        {
            // Just fetch all statutory reports
            return await _repository.GetByTypeAsync("Statutory");
        }

        // Optional helper method if you want DTOs directly
        public async Task<IEnumerable<ComplianceReportResponseDTO>> GenerateTaxReportsDtoAsync(DateTime startDate, DateTime endDate)
        {
            var reports = await GenerateTaxReportsAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<ComplianceReportResponseDTO>>(reports);
        }

        public async Task<IEnumerable<ComplianceReportResponseDTO>> GenerateStatutoryReportsDtoAsync()
        {
            var reports = await GenerateStatutoryReportsAsync();
            return _mapper.Map<IEnumerable<ComplianceReportResponseDTO>>(reports);
        }
    }
}
