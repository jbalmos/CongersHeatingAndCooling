namespace CHC.Common
{
    public interface IDbContextFactory
    {
        ChcDbContext CreateChcDbContext();
    }
}
