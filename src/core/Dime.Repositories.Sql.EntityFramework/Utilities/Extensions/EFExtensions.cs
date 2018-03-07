﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

using System.Linq;

namespace Dime.Repositories
{
    internal static class EFExtensions
    {
        //private static MetadataWorkspace _workspace;
        //private static ObjectItemCollection _itemCollection;

        //private static EntityType GetEntityType<T>(DbContext context)
        //{
        //    if (_workspace == default(MetadataWorkspace))
        //        _workspace = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

        //    if (_itemCollection == default(ObjectItemCollection))
        //        _itemCollection = (ObjectItemCollection)(_workspace.GetItemCollection(DataSpace.OSpace));

        //    EntityType entityType = _itemCollection.OfType<EntityType>().FirstOrDefault(e => _itemCollection.GetClrType(e) == typeof(T));
        //    return entityType;
        //}

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        internal static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> query, DbContext context, params string[] includes) where TEntity : class
        {
            // Do a safety check first
            if (includes == null)
                return query;

            List<string> includeList = new List<string>();
            if (includes.Count() == 0)
            {
                //ReadOnlyMetadataCollection<NavigationProperty> navigationProperties = GetEntityType<TEntity>(context)?.NavigationProperties;
                //if (navigationProperties != null)
                //{
                //    foreach (NavigationProperty navigationProperty in navigationProperties)
                //    {
                //        if (!includeList.Contains(navigationProperty.Name))
                //        {
                //            includeList.Add(navigationProperty.Name);
                //            query = query.Include(navigationProperty.Name);
                //        }
                //    }
                //}

                return query;
            }
            else
            {
                foreach (string include in includes.Where(x => !string.IsNullOrEmpty(x) && !includeList.Contains(x)))
                {
                    query = query.Include(include);
                }

                return query;
            }
        }
    }
}