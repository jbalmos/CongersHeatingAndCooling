using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using EntityFramework.Caching;
using CHC.Common;

namespace CHC.Common.Repositories
{
    public abstract class AbstractDbRepository<T> : IDisposable where T : class
    {
        protected readonly ChcDbContext dbContext;

        public AbstractDbRepository(IDbContextFactory factory)
		{
			dbContext = factory.CreateChcDbContext();
		}

		#region Add

		/// <summary>
		///	Adds the specfied entity to the database.
		/// </summary>
		/// <typeparam name="TDbEntity">Type of the entity to add to the database.</typeparam>
		/// <param name="entity">Entity to add to the database.</param>
		public virtual bool Add( T entity )
		{
            Guard.NotNull(entity, "entity");

			this.dbContext.Set<T>().Add( entity );
			this.dbContext.SaveChanges();

            return true;
		}

		/// <summary>
		///	Adds the specfied entity to the database.
		/// </summary>
		/// <typeparam name="TDbEntity">Type of the entity to add to the database.</typeparam>
		/// <param name="entity">Entity to add to the database.</param>
		public virtual bool AddMany( List<T> entities )
		{
			Guard.NotNull( entities, "entities" );

			this.dbContext.Set<T>().AddRange( entities );
			this.dbContext.SaveChanges();

            return true;
		}

		#endregion

		#region Update

		/// <summary>
		///	Updates the specified entity in the database.
		/// </summary>
		/// <typeparam name="TDbEntity">Type of the entity to update.</typeparam>
		/// <param name="entity">Entity to update.</param>
		public virtual bool Update( T entity )
		{
			Guard.NotNull( entity, "entity" );

			var entry = this.dbContext.Entry( entity );
			entry.State = EntityState.Modified;

			try {
				this.dbContext.SaveChanges();
				return true;
			}
			catch ( DbUpdateConcurrencyException ) {
				return false;
			}
		}

		#endregion

		#region Delete

		public virtual bool Delete( T entity )
		{
			Guard.NotNull( entity, "entity" );

			var entry = this.dbContext.Entry( entity );
			entry.State = EntityState.Deleted;
			this.dbContext.SaveChanges();

            return true;
		}

		/// <summary>
		///	Adds the specfied entity to the database.
		/// </summary>
		/// <typeparam name="TDbEntity">Type of the entity to add to the database.</typeparam>
		/// <param name="entity">Entity to add to the database.</param>
		public virtual bool DeleteMany( List<T> entities )
		{
			Guard.NotNull( entities, "entities" );

			this.dbContext.Set<T>().RemoveRange( entities );
			this.dbContext.SaveChanges();

            return true;
		}

		#endregion

		#region Get

		public virtual IQueryable<T> Query()
		{
			return this.dbContext.Set<T>();
		}

		public virtual T Get( int primaryKey )
		{
			return this.dbContext.Set<T>().Find( primaryKey );
		}

		#endregion

		#region Cache

		public virtual void ClearTableCache( string tableName )
		{
			CacheManager.Current.Expire( tableName );
		}

		public virtual object GetTableCache( string tableName )
		{
			return CacheManager.Current.Get( tableName );
		}

		#endregion

		/// <summary>
		///	See <see cref="IDisposable.Dispose"/>.
		/// </summary>
		public void Dispose()
		{
			this.dbContext.Dispose();
		}
    }
}
