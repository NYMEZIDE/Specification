using System;
using System.Linq.Expressions;

namespace Specification.Internal
{
    internal interface ISpecification<T>
    {
        bool IsSatisfiedBy(T candidate);

        Expression<Func<T, bool>> Expression { get; }
    }
}
