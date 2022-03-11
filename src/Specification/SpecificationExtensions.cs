using System.Collections.Generic;
using System.Linq;
using Specification.Internal;

namespace Specification
{
    public static class SpecificationExtensions
    {
        public static bool Is<T>(this T candidate, AbstractSpec<T> spec) => spec.IsSatisfiedBy(candidate);

        /// <summary>
        /// Result is Spec1 || Spec2 || etc. Any Specs to be enough for True.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="candidate"></param>
        /// <param name="specs"></param>
        /// <returns></returns>
        public static bool IsAny<T>(this T candidate, params AbstractSpec<T>[] specs)
        {
            if (specs == null || !specs.Any())
                return false;
            
            AbstractSpec<T> combineSpec = specs.Aggregate<AbstractSpec<T>, AbstractSpec<T>>(null, (current, spec) 
                => current == null ? spec : current.Or(spec));

            return combineSpec!.IsSatisfiedBy(candidate);
        }

        /// <summary>
        /// Result is Spec1 && Spec2 && etc. All Specs must be True.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="candidate"></param>
        /// <param name="specs"></param>
        /// <returns></returns>
        public static bool IsAll<T>(this T candidate, params AbstractSpec<T>[] specs)
        {
            if (specs == null || !specs.Any())
                return false;
            
            AbstractSpec<T> combineSpec = specs.Aggregate<AbstractSpec<T>, AbstractSpec<T>>(null, (current, spec) 
                => current == null ? spec : current.And(spec));

            return combineSpec.IsSatisfiedBy(candidate);
        }
        

        public static bool AnyIs<T>(this IEnumerable<T> candidates, AbstractSpec<T> spec) => spec.InternalAnyIs(candidates);

        public static bool AllIs<T>(this IEnumerable<T> candidates, AbstractSpec<T> spec) => spec.InternalAllIs(candidates);

        #region Fluent interface
        public static AbstractSpec<T> And<T>(this AbstractSpec<T> left, AbstractSpec<T> right) => new AndSpecification<T>(left, right);

        public static AbstractSpec<T> Or<T>(this AbstractSpec<T> left, AbstractSpec<T> right) => new OrSpecification<T>(left, right);

        public static AbstractSpec<T> Not<T>(this AbstractSpec<T> inner) => new NotSpecification<T>(inner); 
        #endregion
    }
}
