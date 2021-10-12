using System;
using System.Linq.Expressions;

namespace Specifications.Internal
{
    internal interface ISpecification<T>
    {
        bool IsSatisfiedBy(T candidate);

        Expression<Func<T, bool>> Expression { get; }
    }
}
