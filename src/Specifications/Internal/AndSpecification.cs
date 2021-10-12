using System;
using System.Linq.Expressions;
using Specifications.Internal.Expressions;

namespace Specifications.Internal
{
    internal sealed class AndSpecification<T> : AbstractSpec<T>
    {
        public AbstractSpec<T> Left { get; private set; }

        public AbstractSpec<T> Right { get; private set; }

        internal AndSpecification(AbstractSpec<T> left, AbstractSpec<T> right)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
        }

        public override Expression<Func<T, bool>> Expression => Left.Expression.And(Right.Expression);
    }
}
