
namespace CHC.Common
{
    public class DefaultDbContextFactory : IDbContextFactory
    {
        public string chcDbConnectionString;

        public DefaultDbContextFactory(
            string chcDbConnectionString)

        {
            this.chcDbConnectionString = chcDbConnectionString;
        }


        public ChcDbContext CreateChcDbContext()
        {
            return new ChcDbContext(this.chcDbConnectionString);
		}

    }
}
