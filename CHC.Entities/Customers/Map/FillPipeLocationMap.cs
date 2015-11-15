using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Customers.Map
{
    public class FillPipeLocationMap : EntityTypeConfiguration<FillPipeLocation>
    {
        public FillPipeLocationMap()
        {
            this.HasKey(obj => obj.ID);
            this.Property(obj => obj.ID).HasColumnName("FillPipeLocationID");
            this.ToTable("tblFillPipeLocation");
        }
    }
}
