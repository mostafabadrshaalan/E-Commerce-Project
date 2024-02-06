using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BaseSpecifications<TEntity> : ISpecifications<TEntity> where TEntity : BaseEntity
    {
        public BaseSpecifications(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<TEntity, bool>> Criteria { get; set; }

        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new List<Expression<Func<TEntity, object>>>();

        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        public int Take { get;private set; }

        public int Skip { get; private set; }

        public bool isPagingEnabled { get; private set; }

        protected void AddInclude(Expression<Func<TEntity, object>> include)
            => Includes.Add(include);

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderBy)
            => OrderBy = orderBy;
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
           => OrderByDescending = orderByDescending;

        protected void AddPaging(int skip, int take)
        {
            Take = take;
            Skip = skip;
            isPagingEnabled = true;
        }
    }
}
