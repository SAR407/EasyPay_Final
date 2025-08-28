using AutoMapper;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Audit;
using EasyPay_Final.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditLogRepository _repository;
        private readonly IMapper _mapper;

        public AuditService(IAuditLogRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task LogActionAsync(string performedBy, string action, string details)
        {
            if (string.IsNullOrWhiteSpace(performedBy))
                throw new ArgumentException("PerformedBy cannot be empty.", nameof(performedBy));

            if (string.IsNullOrWhiteSpace(action))
                throw new ArgumentException("Action cannot be empty.", nameof(action));

            var log = new AuditLog
            {
                Timestamp = DateTime.UtcNow,
                PerformedBy = performedBy,
                Action = action,
                Details = details ?? string.Empty
            };

            await _repository.AddAsync(log);
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync()
        {
            // Returning entities directly as per your IAuditService interface
            // You can map to DTOs here if you want cleaner API responses
            return await _repository.GetAllAsync();
        }

        // Optional helper for DTO mapping
        public async Task<IEnumerable<AuditLogResponseDTO>> GetAuditLogsAsDtoAsync()
        {
            var logs = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<AuditLogResponseDTO>>(logs);
        }
    }
}
