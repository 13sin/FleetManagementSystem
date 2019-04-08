using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace fleetAPI.Models.Data
{
    public partial class fleetContext : DbContext
    {
        public fleetContext()
        {
        }

        public fleetContext(DbContextOptions<fleetContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Broker> Broker { get; set; }
        public virtual DbSet<Carrier> Carrier { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Destination> Destination { get; set; }
        public virtual DbSet<Driver> Driver { get; set; }
        public virtual DbSet<Origin> Origin { get; set; }
        public virtual DbSet<Shipment> Shipment { get; set; }
        public virtual DbSet<ShipmentOrder> ShipmentOrder { get; set; }
        public virtual DbSet<Trailer> Trailer { get; set; }
        public virtual DbSet<Truck> Truck { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=fleet.cwwvzanpma0i.us-east-1.rds.amazonaws.com;Initial Catalog=fleet;User ID=tamas;Password=password;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(10);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(50);

                entity.Property(e => e.Postalcode)
                    .HasColumnName("postalcode")
                    .HasMaxLength(50);

                entity.Property(e => e.Province)
                    .HasColumnName("province")
                    .HasMaxLength(50);

                entity.Property(e => e.Streetname)
                    .HasColumnName("streetname")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Broker>(entity =>
            {
                entity.Property(e => e.Mc)
                    .IsRequired()
                    .HasColumnName("MC")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Broker)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Broker_Address");
            });

            modelBuilder.Entity<Carrier>(entity =>
            {
                entity.Property(e => e.Ctpat)
                    .HasColumnName("CTPAT")
                    .HasMaxLength(50);

                entity.Property(e => e.Cvor)
                    .HasColumnName("CVOR")
                    .HasMaxLength(50);

                entity.Property(e => e.Mc)
                    .HasColumnName("MC")
                    .HasMaxLength(50);

                entity.Property(e => e.Usdot)
                    .HasColumnName("USDOT")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Carrier)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Carrier_Address");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_Address");
            });

            modelBuilder.Entity<Destination>(entity =>
            {
                entity.ToTable("destination");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Destination)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_destination_Address");
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.Property(e => e.CarrierId).HasColumnName("carrierId");

                entity.Property(e => e.userId).HasColumnName("userId");

                entity.Property(e => e.LicenseNumber).HasColumnName("licenseNumber");

                entity.Property(e => e.LicenseState)
                    .HasColumnName("licenseState")
                    .HasMaxLength(50);

                entity.Property(e => e.LicenseType)
                    .HasColumnName("licenseType")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Driver)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Driver_Address");

                entity.HasOne(d => d.Carrier)
                    .WithMany(p => p.Driver)
                    .HasForeignKey(d => d.CarrierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Driver_Carrier");
            });

            modelBuilder.Entity<Origin>(entity =>
            {
                entity.ToTable("origin");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Origin)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_origin_Address");
            });

            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.ToTable("shipment");

                entity.Property(e => e.BrokerId).HasColumnName("brokerId");

                entity.Property(e => e.BrokerRate)
                    .HasColumnName("broker_rate")
                    .HasColumnType("money");

                entity.Property(e => e.Commodity).HasColumnName("commodity");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.DestinationApptDatetime).HasColumnName("destination_appt_datetime");

                entity.Property(e => e.DestinationApptNumber).HasColumnName("destination_appt_number");

                entity.Property(e => e.DestinationId).HasColumnName("destinationId");

                entity.Property(e => e.EquipmentType)
                    .HasColumnName("equipmentType")
                    .HasMaxLength(50);

                entity.Property(e => e.FreightType)
                    .HasColumnName("freightType")
                    .HasMaxLength(10);

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.OriginApptDatetime).HasColumnName("origin_appt_datetime");

                entity.Property(e => e.OriginApptNumber).HasColumnName("origin_appt_number");

                entity.Property(e => e.OriginId).HasColumnName("originId");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.Broker)
                    .WithMany(p => p.Shipment)
                    .HasForeignKey(d => d.BrokerId)
                    .HasConstraintName("FK_shipment_Broker");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Shipment)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_shipment_Customer");

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.Shipment)
                    .HasForeignKey(d => d.DestinationId)
                    .HasConstraintName("FK_shipment_destination");

                entity.HasOne(d => d.Origin)
                    .WithMany(p => p.Shipment)
                    .HasForeignKey(d => d.OriginId)
                    .HasConstraintName("FK_shipment_origin");
            });

            modelBuilder.Entity<ShipmentOrder>(entity =>
            {
                entity.ToTable("shipmentOrder");

                entity.Property(e => e.CarrierId).HasColumnName("carrierId");

                entity.Property(e => e.CarrierRate)
                    .HasColumnName("carrier_rate")
                    .HasColumnType("money");

                entity.Property(e => e.DriverId).HasColumnName("driverId");

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.ShipmentId).HasColumnName("shipmentId");

                entity.Property(e => e.TrailerId).HasColumnName("trailerId");

                entity.Property(e => e.TruckId).HasColumnName("truckId");

                entity.HasOne(d => d.Carrier)
                    .WithMany(p => p.ShipmentOrder)
                    .HasForeignKey(d => d.CarrierId)
                    .HasConstraintName("FK_shipmentOrder_Carrier");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.ShipmentOrder)
                    .HasForeignKey(d => d.DriverId)
                    .HasConstraintName("FK_shipmentOrder_Driver");

                entity.HasOne(d => d.Shipment)
                    .WithMany(p => p.ShipmentOrder)
                    .HasForeignKey(d => d.ShipmentId)
                    .HasConstraintName("FK_shipmentOrder_shipment");

                entity.HasOne(d => d.Trailer)
                    .WithMany(p => p.ShipmentOrder)
                    .HasForeignKey(d => d.TrailerId)
                    .HasConstraintName("FK_shipmentOrder_Trailer");

                entity.HasOne(d => d.Truck)
                    .WithMany(p => p.ShipmentOrder)
                    .HasForeignKey(d => d.TruckId)
                    .HasConstraintName("FK_shipmentOrder_Truck");
            });

            modelBuilder.Entity<Trailer>(entity =>
            {
                entity.Property(e => e.CarrierId).HasColumnName("carrierId");

                entity.Property(e => e.LicensePlate)
                    .HasColumnName("license_plate")
                    .HasMaxLength(50);

                entity.Property(e => e.Location).HasColumnName("location");

                entity.Property(e => e.Make)
                    .HasColumnName("make")
                    .HasMaxLength(10);

                entity.Property(e => e.Model)
                    .HasColumnName("model")
                    .HasMaxLength(10);

                entity.Property(e => e.TrailerType).HasMaxLength(10);

                entity.Property(e => e.Vin).HasColumnName("vin");

                entity.Property(e => e.Year)
                    .HasColumnName("year")
                    .HasMaxLength(10);

                entity.HasOne(d => d.Carrier)
                    .WithMany(p => p.Trailer)
                    .HasForeignKey(d => d.CarrierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trailer_Carrier");
            });

            modelBuilder.Entity<Truck>(entity =>
            {
                entity.Property(e => e.CarrierId).HasColumnName("carrierId");

                entity.Property(e => e.LicensePlate)
                    .HasColumnName("license_plate")
                    .HasMaxLength(50);

                entity.Property(e => e.Location).HasColumnName("location");

                entity.Property(e => e.Make)
                    .HasColumnName("make")
                    .HasMaxLength(10);

                entity.Property(e => e.Model)
                    .HasColumnName("model")
                    .HasMaxLength(10);

                entity.Property(e => e.OdometerId).HasColumnName("odometerId");

                entity.Property(e => e.TruckType).HasMaxLength(10);

                entity.Property(e => e.Vin).HasColumnName("vin");

                entity.Property(e => e.Year)
                    .HasColumnName("year")
                    .HasMaxLength(10);

                entity.HasOne(d => d.Carrier)
                    .WithMany(p => p.Truck)
                    .HasForeignKey(d => d.CarrierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Truck_Carrier");
            });
        }
    }
}
