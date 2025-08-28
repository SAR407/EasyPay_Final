using EasyPay_Final.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace EasyPay_Final.Context
{
    public class EasypayDbContext : DbContext
    {
        public EasypayDbContext(DbContextOptions<EasypayDbContext> options) : base(options) { }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<ComplianceReport> ComplianceReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ===== Master Tables =====
            modelBuilder.Entity<Role>().HasKey(r => r.RoleId).HasName("PK_Role");
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "PayrollProcessor" },
                new Role { RoleId = 3, RoleName = "Employee" },
                new Role { RoleId = 4, RoleName = "Manager" }
            );

            // ===== User Table =====
            modelBuilder.Entity<User>().HasKey(u => u.UserId).HasName("PK_User");
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .HasConstraintName("FK_User_Role")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<Employee>(e => e.UserId)
                .HasConstraintName("FK_Employee_User")
                .OnDelete(DeleteBehavior.Cascade);

            // ===== Employee Table =====
            modelBuilder.Entity<Employee>().HasKey(e => e.EmployeeId).HasName("PK_Employee");

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Payrolls)
                .WithOne(p => p.Employee)
                .HasForeignKey(p => p.EmployeeId)
                .HasConstraintName("FK_Payroll_Employee")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.LeaveRequests)
                .WithOne(l => l.Employee)
                .HasForeignKey(l => l.EmployeeId)
                .HasConstraintName("FK_Leave_Employee")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Timesheets)
                .WithOne(t => t.Employee)
                .HasForeignKey(t => t.EmployeeId)
                .HasConstraintName("FK_Timesheet_Employee")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Benefits)
                .WithOne(b => b.Employee)
                .HasForeignKey(b => b.EmployeeId)
                .HasConstraintName("FK_Benefit_Employee")
                .OnDelete(DeleteBehavior.Cascade);

            // ===== Payroll Table =====
            modelBuilder.Entity<Payroll>().HasKey(p => p.PayrollId).HasName("PK_Payroll");

            // ===== LeaveRequest Table =====
            modelBuilder.Entity<LeaveRequest>().HasKey(l => l.LeaveRequestId).HasName("PK_LeaveRequest");

            // ===== Timesheet Table =====
            modelBuilder.Entity<Timesheet>().HasKey(t => t.TimesheetId).HasName("PK_Timesheet");

            // ===== Benefit Table =====
            modelBuilder.Entity<Benefit>().HasKey(b => b.BenefitId).HasName("PK_Benefit");

            // ===== AuditLog Table =====
            modelBuilder.Entity<AuditLog>().HasKey(a => a.AuditLogId).HasName("PK_AuditLog");

            // ===== ComplianceReport Table =====
            modelBuilder.Entity<ComplianceReport>().HasKey(c => c.ReportId).HasName("PK_ComplianceReport");
        }
    }
}
