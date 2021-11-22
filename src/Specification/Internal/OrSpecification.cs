using System;
using System.Linq.Expressions;
using Specification.Internal.Expressions;

namespace Specification.Internal
{
    internal sealed class OrSpecification<T> : AbstractSpec<T>
    {
        public AbstractSpec<T> Left { get; private set; }

        public AbstractSpec<T> Right { get; private set; }

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

            if (Left.OnFalseAction != null)
                base.UpActions(Left);

            if (Right.OnFalseAction != null)
                base.UpActions(Right);

            return false;

            //return Left.IsSatisfiedBy(candidate) || Right.IsSatisfiedBy(candidate);
        }
    }
}
