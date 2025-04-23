using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T,TKey> (ISpecifications<T,TKey> Spec , IQueryable<T> InputQuery) where T : BaseEntity<TKey>
        {
            var query = InputQuery;

            if(Spec.Criteria is not null)
                query= query.Where(Spec.Criteria);

            query=Spec.IncludeExpressions.Aggregate(query,(CurrentQuery,includeExpression)=> CurrentQuery.Include(includeExpression));


            return query;
        }
    }
}
