﻿using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Customers.Map
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
		{
            this.HasKey(obj => obj.ID);
            this.Property(obj => obj.ID).HasColumnName("CustomerID");
            this.HasMany(obj => obj.Addresses).WithOptional().HasForeignKey(a => a.CustomerID);
            this.ToTable("tblCustomer");
		}
    }
}
