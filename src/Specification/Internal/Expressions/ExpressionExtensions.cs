using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Specification.Internal.Expressions
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
    }
}
