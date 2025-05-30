﻿using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ISpecifications<T,TKey> where T : BaseEntity<TKey>
    {
        Expression<Func<T, bool>>? Criteria { get; set; }

        List<Expression<Func<T,object>>> IncludeExpressions { get; set; }

        Expression<Func<T, object>>? OrderBy { get; set; }

        Expression<Func<T, object>>? OrderByDescending { get; set; }


        int Skip { get; set; }

        int Take { get; set; }

        bool IsPagination { get; set; }
    }
}
