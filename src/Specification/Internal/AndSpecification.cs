using System;
using System.Linq.Expressions;
using Specification.Internal.Expressions;

namespace Specification.Internal
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

        internal override bool InternalIsSatisfiedBy(T candidate)
        {
            var leftResult = Left.InternalIsSatisfiedBy(candidate);

            if (leftResult == false && Left.OnFalseAction != null)
                base.UpActions(Left);

            var rightResult = Right.InternalIsSatisfiedBy(candidate);

            if (rightResult == false && Right.OnFalseAction != null)
                base.UpActions(Right);

            return leftResult && rightResult;
        }
    }
}
