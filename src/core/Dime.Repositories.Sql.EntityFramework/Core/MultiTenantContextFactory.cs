﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract partial class MultiTenantContextFactory<TContext> : IMultiTenantDbContextFactory<TContext> where TContext : DbContext, new()
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        protected MultiTenantContextFactory()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connectionString"></param>
        protected MultiTenantContextFactory(string connectionString) : this()
        {
            this.Connection = connectionString;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tenant"></param>
        protected MultiTenantContextFactory(string connectionString, string tenant) : this(connectionString)
        {
            this.Tenant = tenant;
        }

        #endregion Constructor

        #region Properties

        public string Connection { get; set; }
        public string Tenant { get; set; }

        #endregion Properties

        /// <summary>
        /// Creates the instance of <typeparamref name="TContext"/> with the default settings
        /// </summary>
        /// <returns></returns>
        public virtual TContext Create()
        {
            if (!string.IsNullOrEmpty(this.Tenant) && !string.IsNullOrEmpty(this.Connection))
                return this.Create(this.Tenant, this.Connection);
            else
                return this.Create("dbo", this.Connection);
        }

        /// <summary>
        /// Creates the specified connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        public virtual TContext Create(string connection)
        {
            return this.Create("dbo", connection);
        }

        /// <summary>
        /// Creates the specified tenant.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>        
        public TContext Create(string tenant, string connection, string context)
        {
            return this.Create(tenant, connection);
        }

        /// <summary>
        /// Creates the specified tenant.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>      
        public TContext Create(string tenant, string connection)
        {
            return new TContext();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public TContext Create(DbContextFactoryOptions options)
        {
            DbContextOptionsBuilder<TContext> optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseSqlServer(this.Connection);
            return Activator.CreateInstance(typeof(TContext), new object[] { optionsBuilder.Options }) as TContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public TContext CreateDbContext(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}