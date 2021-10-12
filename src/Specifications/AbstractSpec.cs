using System;
using System.Linq.Expressions;
using Specifications.Internal;
using Specifications.Internal.Expressions;

namespace Specifications
{
    public abstract class AbstractSpec<T> : ISpecification<T>
    {
        private Func<T, bool> _compiledFunc;

        public bool IsSatisfiedBy(T candidate)
        {
            _compiledFunc ??= Expression.Compile();
            return _compiledFunc(candidate);
        }

        public abstract Expression<Func<T, bool>> Expression { get; }

        #region Operators
        public static AbstractSpec<T> operator &(AbstractSpec<T> spec1, AbstractSpec<T> spec2)
        {
            return new AndSpecification<T>(spec1, spec2);
        }

        public static AbstractSpec<T> operator |(AbstractSpec<T> left, AbstractSpec<T> right)
        {
            return new OrSpecification<T>(left, right);
        }

        public static AbstractSpec<T> operator ==(bool value, AbstractSpec<T> spec)
        {
            return value ? spec : !spec;
        }

        public static AbstractSpec<T> operator ==(AbstractSpec<T> spec, bool value)
        {
            return value ? spec : !spec;
        }

        public static AbstractSpec<T> operator !=(bool value, AbstractSpec<T> spec)
        {
            return value ? !spec : spec;
        }

        public static AbstractSpec<T> operator !=(AbstractSpec<T> spec, bool value)
        {
            return value ? !spec : spec;
        }

        public static AbstractSpec<T> operator !(AbstractSpec<T> spec)
        {
            return new NotSpecification<T>(spec);
        }

        public static implicit operator Expression<Func<T, bool>>(AbstractSpec<T> spec)
        {
            return spec.Expression;
        }

        public static implicit operator Func<T, bool>(AbstractSpec<T> spec)
        {
            return spec.IsSatisfiedBy;
        }

        #endregion

        #region Overrides
        public override string ToString()
        {
            return Expression.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            
            if ((obj is AbstractSpec<T>) == false)
                return false;

            return Expression.IsEquals((obj as AbstractSpec<T>).Expression);
        }

        public override int GetHashCode()
        {
            return Expression.GetHashCodeFor();
        }

        #endregion
    }
}
