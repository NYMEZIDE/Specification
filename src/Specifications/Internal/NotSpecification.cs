using System;
using System.Linq.Expressions;
using Specifications.Internal.Expressions;

namespace Specifications.Internal
{
    internal sealed class NotSpecification<T> : AbstractSpec<T>
    {
        public AbstractSpec<T> Inner { get; private set; }

        internal NotSpecification(AbstractSpec<T> inner)
        {
            Inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public override Expression<Func<T, bool>> Expression => Inner.Expression.Not();
    }
}
