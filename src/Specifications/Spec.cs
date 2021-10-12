﻿using System;
using System.Linq.Expressions;

namespace Specifications
{
    public sealed class Spec<T> : AbstractSpec<T>
    {
        public Spec(Expression<Func<T, bool>> expression)
        {
            Expression = expression;
        }

        public override Expression<Func<T, bool>> Expression { get; }
    }
}
