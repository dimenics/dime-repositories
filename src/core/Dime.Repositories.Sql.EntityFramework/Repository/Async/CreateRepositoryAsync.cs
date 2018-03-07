﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        /// Save a new item to the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <returns>The connected entity</returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            using (TContext ctx = this.Context)
            {
                ctx.Entry(entity).State = EntityState.Added;
                EntityEntry<TEntity> createdItem = ctx.Set<TEntity>().Add(entity);
                await this.SaveChangesAsync(ctx);

                return createdItem.Entity;
            }
        }

        /// <summary>
        /// Save a new item to the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <param name="predicate">The predicate to validate before creating the entity</param>
        /// <returns>The connected entity</returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate)
        {
            using (TContext ctx = this.Context)
            {
                ctx.Entry(entity).State = EntityState.Added;
                TEntity createdItem = ctx.Set<TEntity>().AddIfNotExists(entity, predicate);
                await this.SaveChangesAsync(ctx);

                return createdItem;
            }
        }

        /// <summary>
        /// Save a new item to the data store and provide the chance to execute additional logic before saving
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <param name="beforeSaveAction">The Func to execute before anything is done</param>
        /// <returns>The connected entity</returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity, Func<TEntity, TContext, Task> beforeSaveAction)
        {
            using (TContext ctx = this.Context)
            {
                await beforeSaveAction(entity, ctx);

                ctx.Entry(entity).State = EntityState.Added;
                EntityEntry<TEntity> createdItem = ctx.Set<TEntity>().Add(entity);
                await this.SaveChangesAsync(ctx);

                return createdItem.Entity;
            }
        }

        /// <summary>
        /// Save a new item to the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <param name="commit">Indicates whether or not SaveChangesAsync should be executed</param>
        /// <returns>The connected entity</returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity, bool commit)
        {
            using (TContext ctx = this.Context)
            {
                ctx.Entry(entity).State = EntityState.Added;
                EntityEntry<TEntity> createdItem = ctx.Set<TEntity>().Add(entity);

                if (commit)
                    await this.SaveChangesAsync(ctx);

                return createdItem.Entity;
            }
        }

        /// <summary>
        /// Save new items to the data store
        /// </summary>
        /// <param name="entities">The disconnected entities to store</param>
        /// <returns>The connected entity</returns>
        public virtual async Task<IQueryable<TEntity>> CreateAsync(IQueryable<TEntity> entities)
        {
            using (TContext ctx = this.Context)
            {
                foreach (TEntity entity in entities)
                {
                    ctx.Entry(entity).State = EntityState.Added;
                    ctx.Set<TEntity>().Add(entity);
                }

                await this.SaveChangesAsync(ctx);

                return entities;
            }
        }
    }
}