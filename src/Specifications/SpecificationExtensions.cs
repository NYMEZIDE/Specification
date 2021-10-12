using System.Collections.Generic;
using System.Linq;
using Specifications.Internal;

namespace Specifications
{
    public static class SpecificationExtensions
    {
        public static bool Is<T>(this T candidate, AbstractSpec<T> spec) => spec.IsSatisfiedBy(candidate);

        public static bool IsAny<T>(this T candidate, params AbstractSpec<T>[] specs) => specs.Any(c => c.IsSatisfiedBy(candidate));

        public static bool IsAll<T>(this T candidate, params AbstractSpec<T>[] specs) => specs.All(c => c.IsSatisfiedBy(candidate));

        public static bool AnyIs<T>(this IEnumerable<T> candidates, AbstractSpec<T> spec) => candidates.Any(spec.IsSatisfiedBy);

        public static bool AllIs<T>(this IEnumerable<T> candidates, AbstractSpec<T> spec) => candidates.All(spec.IsSatisfiedBy);

        #region Fluent interface
        public static AbstractSpec<T> And<T>(this AbstractSpec<T> left, AbstractSpec<T> right) => new AndSpecification<T>(left, right);

        public static AbstractSpec<T> Or<T>(this AbstractSpec<T> left, AbstractSpec<T> right) => new OrSpecification<T>(left, right);

        public static AbstractSpec<T> Not<T>(this AbstractSpec<T> inner) => new NotSpecification<T>(inner); 
        #endregion
    }
}
