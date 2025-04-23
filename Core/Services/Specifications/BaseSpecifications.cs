using Domain.Contracts;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class BaseSpecifications<T,TKey> : ISpecifications<T, TKey> where T : BaseEntity<TKey>
    {
        public Expression<Func<T, bool>>? Criteria { get; set; }
        public List<Expression<Func<T, object>>> IncludeExpressions { get; set; } = new List<Expression<Func<T, object>>>();  

        public BaseSpecifications(Expression<Func<T, bool>>? criteria)
        {
            Criteria = criteria;
        }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            IncludeExpressions.Add(includeExpression);
        }


    }
}
