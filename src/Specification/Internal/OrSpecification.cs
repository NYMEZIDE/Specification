﻿using System;
using System.Linq.Expressions;
using Specification.Internal.Expressions;

namespace Specification.Internal
{
    internal sealed class OrSpecification<T> : AbstractSpec<T>
    {
        private AbstractSpec<T> Left { get; }

        private AbstractSpec<T> Right { get; }

        internal OrSpecification(AbstractSpec<T> left, AbstractSpec<T> right)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
        }

        public override Expression<Func<T, bool>> Expression => Left.Expression.Or(Right.Expression);

        internal override bool InternalIsSatisfiedBy(T candidate)
        {
            if (Left.InternalIsSatisfiedBy(candidate) || Right.InternalIsSatisfiedBy(candidate))
                return true;

            base.UpActions(Left);

            base.UpActions(Right);

            return false;
        }
    }
}
