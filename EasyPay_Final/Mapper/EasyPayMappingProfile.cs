using AutoMapper;
using EasyPay_Final.Models;
using EasyPay_Final.Models.DTO.Audit;
using EasyPay_Final.Models.DTO.Benefit;
using EasyPay_Final.Models.DTO.Compilance;
using EasyPay_Final.Models.DTO.Employee;
using EasyPay_Final.Models.DTO.LeaveRequest;
using EasyPay_Final.Models.DTO.Authentication;
using EasyPay_Final.Models.DTO.Payroll;
using EasyPay_Final.Models.DTO.Timesheet;
using EasyPay_Final.Models.DTO.User;


namespace EasyPay_Final.Mapping
{
    public class EasyPayMappingProfile : Profile
    {
        public EasyPayMappingProfile()
        {
            CreateMap<AuditLog, AuditLogResponseDTO>().ReverseMap();
            CreateMap<Benefit, BenefitResponseDTO>().ReverseMap();
            CreateMap<Benefit, BenefitRequestDTO>().ReverseMap();
            CreateMap<ComplianceReport, ComplianceReportResponseDTO>().ReverseMap();
            CreateMap<Employee, EmployeeResponseDTO>().ReverseMap();
            CreateMap<Employee, EmployeeCreateDTO>().ReverseMap();
            CreateMap<LeaveRequest, LeaveResponseDTO>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestDTO>().ReverseMap();
            CreateMap<Payroll, PayrollResponseDTO>().ReverseMap();
            CreateMap<Payroll, PayrollRequestDTO>().ReverseMap();
            CreateMap<Timesheet, TimesheetResponseDTO>().ReverseMap();
            CreateMap<Timesheet, TimesheetRequestDTO>().ReverseMap();
            CreateMap<User, UserResponseDTO>().ReverseMap();
            CreateMap<User, UserCreateDTO>().ReverseMap();
            CreateMap<Employee, EmployeeResponseDTO>()
    .ForMember(dest => dest.FullName,
               opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        }
    }
}