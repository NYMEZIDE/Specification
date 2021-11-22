using System.Linq.Expressions;

namespace Specification.Internal.Expressions
{
    internal class BinaryExpressionVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _old;
        private readonly ParameterExpression _new;

        public BinaryExpressionVisitor(ParameterExpression old, ParameterExpression @new)
        {
            _old = old;
            _new = @new;
        }

        public override Expression Visit(Expression node) => node == _old ? _new : base.Visit(node);
    }
}