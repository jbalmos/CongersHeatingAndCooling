using System.Data.Entity;
using CHC.Entities.Customers.Map;
using CHC.Entities.Services.OilDelivery.Map;

namespace CHC.Common
{
	public class ChcDbContext : DbContext
	{
		static ChcDbContext()
		{
			// This tells EF that we are working with a pre-existing database and are not using the migrations 
			// feature.
			Database.SetInitializer<ChcDbContext>(null);
		}

		public ChcDbContext(string connectionString)
		 : base(connectionString)
		{ }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			/* Customer models */
			modelBuilder.Configurations.Add(new CustomerMap());
			modelBuilder.Configurations.Add(new AccountMap());
			modelBuilder.Configurations.Add(new AddressMap());
			modelBuilder.Configurations.Add(new OilTankMap());
			modelBuilder.Configurations.Add(new FillPipeLocationMap());

			/* Oil Delivery Service Models*/
			modelBuilder.Configurations.Add(new ServiceAreaMap());
			modelBuilder.Configurations.Add(new ServiceAreaTownMap());
			modelBuilder.Configurations.Add(new PricingTierMap());
			modelBuilder.Configurations.Add(new PriceLevelMap());
			modelBuilder.Configurations.Add(new PriceLevelFeeMap());
			modelBuilder.Configurations.Add(new DeliveryRequestMap());
			modelBuilder.Configurations.Add(new DeliveryRequestFeeMap());

			modelBuilder.Configurations.Add(new ContactRequestMap());
		}
	}
}
