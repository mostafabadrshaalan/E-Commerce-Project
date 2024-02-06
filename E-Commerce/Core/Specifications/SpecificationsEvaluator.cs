using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public static class SpecificationsEvaluator<TEntity>where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> specifications)
        {
            var query = inputQuery;
            if(specifications.Criteria!=null)
                query =query.Where(specifications.Criteria);


            if(specifications.OrderBy!=null)
                query=query.OrderBy(specifications.OrderBy);

            if(specifications.OrderByDescending!=null)
                query=query.OrderByDescending(specifications.OrderByDescending);

            if(specifications.isPagingEnabled)
                query=query.Skip(specifications.Skip).Take(specifications.Take);

            foreach (var include in specifications.Includes)
                query = query.Include(include);

            return query;
        }
    }
}
