using Autofac;
using CHC.Common;

namespace CongerHeatingAndCooling
{
    public class ChcAutofacModule : Module
    {
        public string ChcDbConnectionString { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(b => new DefaultDbContextFactory(
                this.ChcDbConnectionString ) )
                .As<IDbContextFactory>()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(DefaultDbContextFactory).Assembly)
                .Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces();
        }
    }
}