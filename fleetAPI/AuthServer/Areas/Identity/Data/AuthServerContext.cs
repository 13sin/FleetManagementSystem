using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthServer.Models;

namespace AuthServer.Models
{
    public class AuthServerContext : IdentityDbContext<AuthServerUser>
    {
        public AuthServerContext(DbContextOptions<AuthServerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EmployeeInfo> EmployeeInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=fleet.cwwvzanpma0i.us-east-1.rds.amazonaws.com;Initial Catalog=fleet;User ID=tamas;Password=password;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<EmployeeInfo>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.FirstName).HasColumnName("firstName");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.LastName).HasColumnName("lastName");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("userId");
            });
        }
    }
}
