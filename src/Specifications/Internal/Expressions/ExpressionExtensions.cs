using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Specifications.Internal.Expressions
{
    internal static class ExpressionExtensions
    {
        internal static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            return Combine(left, Expression.AndAlso, right);
        }

        internal static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            return Combine(left, Expression.OrElse, right);
        }

        internal static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(expression.Body), 
                expression.Parameters);
        }

        private static Expression<Func<T, bool>> Combine<T>(
            Expression<Func<T, bool>> left, 
            Func<Expression, Expression, BinaryExpression> operation,
            Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftExpression = new BinaryExpressionVisitor(left.Parameters.Single(), parameter).Visit(left.Body);
            var rightExpression = new BinaryExpressionVisitor(right.Parameters.Single(), parameter).Visit(right.Body);

            return Expression.Lambda<Func<T, bool>>(operation(leftExpression, rightExpression), parameter);
        }

        internal static bool IsEquals<T>(this Expression<Func<T, bool>> x, Expression<Func<T, bool>> y)
        {
            return new ExpressionEqualityComparer().Equals(x, y);
        }
        
        internal static int GetHashCodeFor<T>(this Expression<T> x)
        {
            return new ExpressionHashCodeResolver().GetHashCodeFor(x);
        }

        #region private extensions

        internal static bool IsEqualTo<TExpression, TMember>(this TExpression value, TExpression other, Func<TExpression, TMember> reader)
        {
            return EqualityComparer<TMember>.Default.Equals(reader.Invoke(value), reader.Invoke(other));
        }
        
        internal static bool IsEqualTo<TExpression>(this TExpression value, TExpression other, params Func<TExpression, object>[] reader)
        {
            return reader.All(_ => EqualityComparer<object>.Default.Equals(_.Invoke(value), _.Invoke(other)));
        }

        internal static int GetHashCodeFor<TExpression, TProperty>(this TExpression value, TProperty prop)
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + prop.GetHashCode();
                return hash;
            }
        }
        
        internal static int GetHashCodeFor<TExpression>(this TExpression value, params object[] props)
        {
            unchecked
            {
                return props.Where(prop => prop != null).Aggregate(17, (current, prop) => current * 23 + prop.GetHashCode());
            }
        }

        #endregion
    }
}
