﻿using System;
using System.Linq.Expressions;
using Specification.Internal.Expressions;

namespace Specification.Internal
{
    internal sealed class NotSpecification<T> : AbstractSpec<T>
    {
        private AbstractSpec<T> Inner { get; }

        internal NotSpecification(AbstractSpec<T> inner)
        {
            Inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public override Expression<Func<T, bool>> Expression => Inner.Expression.Not();

        internal override bool InternalIsSatisfiedBy(T candidate)
        {
            var innerResult = !Inner.InternalIsSatisfiedBy(candidate);

            if (innerResult is false)
                base.UpActions(Inner);

            return innerResult;
        }
    }
}
