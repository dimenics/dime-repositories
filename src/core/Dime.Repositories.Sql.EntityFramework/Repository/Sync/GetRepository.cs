﻿using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <history>
    /// [HB] 19/05/2016 - Review AsNoTracking() is enabled everywhere
    /// </history>
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists(Expression<Func<TEntity, bool>> where)
        {
            using (TContext ctx = this.Context)
            {
                return ctx.Set<TEntity>().Any(where);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity FindById(long id)
        {
            return this.Context.Set<TEntity>().Find(id);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> where)
        {
            if (where == null)
            {
                return this.FindAll().FirstOrDefault();
            }
            else
            {
                return this.FindAll(where).FirstOrDefault();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> where, params string[] includes)
        {
            if (where == null)
            {
                return this.FindAll().FirstOrDefault();
            }
            else
            {
                return this.FindAll(where, true, includes).FirstOrDefault();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where, int? page, int? pageSize, string[] includes)
        {
            if (includes == null)
            {
                return this.FindAll(where);
            }
            else
            {
                using (TContext ctx = this.Context)
                {
                    IQueryable<TEntity> query = ctx.Set<TEntity>().With(where).With(page, pageSize).With(pageSize).AsNoTracking();
                    foreach (var include in includes)
                        query = query.Include(include);

                    return query.ToList().AsQueryable();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
        {
            if (includes == null)
            {
                return this.FindAll(where);
            }
            else
            {
                using (TContext ctx = this.Context)
                {
                    IQueryable<TEntity> query = where == null ?
                    ctx.Set<TEntity>().AsNoTracking() :
                    ctx.Set<TEntity>().Where(where).AsNoTracking();

                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }

                    return query.ToList().AsQueryable();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where = null)
        {
            using (TContext ctx = this.Context)
            {
                IEnumerable<TEntity> query = where == null ?
                ctx.Set<TEntity>().AsNoTracking() :
                ctx.Set<TEntity>().Where(where).AsNoTracking();

                //MetadataWorkspace workspace = ((IObjectContextAdapter)ctx).ObjectContext.MetadataWorkspace;
                //ObjectItemCollection itemCollection = (ObjectItemCollection)(workspace.GetItemCollection(DataSpace.OSpace));
                //EntityType entityType = itemCollection.OfType<EntityType>().Single(e => itemCollection.GetClrType(e) == typeof(TEntity));

                //object _lock = new object();
                //Parallel.ForEach(entityType.NavigationProperties, x =>
                //{
                //    lock (_lock)
                //    {
                //        query = query.Include(x.Name);
                //    }
                //});

                return query.ToList().AsQueryable();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where = null, params string[] includes)
        {
            using (TContext ctx = this.Context)
            {
                IEnumerable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where);

                return query.ToList().AsQueryable();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="includeAll"></param>
        /// <param name=""></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 09/05/2016 - Create
        /// </history>
        public virtual IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where, bool includeAll, params string[] includes)
        {
            using (TContext ctx = this.Context)
            {
                IQueryable<TEntity> query = where == null ?
                    ctx.Set<TEntity>().AsNoTracking() :
                    ctx.Set<TEntity>().Where(where).AsNoTracking();

                //if (includeAll)
                //{
                //    MetadataWorkspace workspace = ((IObjectContextAdapter)ctx).ObjectContext.MetadataWorkspace;
                //    ObjectItemCollection itemCollection = (ObjectItemCollection)(workspace.GetItemCollection(DataSpace.OSpace));
                //    EntityType entityType = itemCollection.OfType<EntityType>().Single(e => itemCollection.GetClrType(e) == typeof(TEntity));

                //    object _lock = new object();
                //    Parallel.ForEach(entityType.NavigationProperties, x =>
                //    {
                //        lock (_lock)
                //        {
                //            query = query.Include(x.Name);
                //        }
                //    });
                //}

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return query.ToList().AsQueryable();
            }
        }
    }
}