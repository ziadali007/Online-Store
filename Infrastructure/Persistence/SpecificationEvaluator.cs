﻿using Domain.Contracts;
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

            if (Spec.OrderBy is not null)
                query = query.OrderBy(Spec.OrderBy);
            else if (Spec.OrderByDescending is not null)
                query = query.OrderByDescending(Spec.OrderByDescending);

            if (Spec.IsPagination)
                query = query.Skip(Spec.Skip).Take(Spec.Take);


            query =Spec.IncludeExpressions.Aggregate(query,(CurrentQuery,includeExpression)=> CurrentQuery.Include(includeExpression));


            return query;
        }
    }
}
