using System.Collections.Generic;
using System.Linq.Expressions;

namespace Specifications.Internal.Expressions
{
    internal interface IValueComparer<in T>
    {
        bool Compare(T x, T y);
    }
    
    internal sealed class ExpressionValueComparer : ExpressionVisitor, IValueComparer<Expression>
    {
        private Queue<Expression> _tracked;
        private Expression _current;

        private bool _eq = true;

        public bool Compare(Expression x, Expression y)
        {
            IExpressionCollection expressionCollection = new ExpressionCollection(y);
            expressionCollection.Fill();

            _tracked = new Queue<Expression>(expressionCollection);

            Visit(x);

            return _eq;
        }

        public override Expression Visit(Expression node)
        {
            if (!_eq)
            {
                return node;
            }

            if (node == null || _tracked.Count == 0)
            {
                _eq = false;
                return node;
            }

            var peeked = _tracked.Peek();

            if (peeked == null || peeked.NodeType != node.NodeType || peeked.Type != node.Type)
            {
                _eq = false;
                return node;
            }

            _current = _tracked.Dequeue();

            return base.Visit(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            var other = (BinaryExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.Method, _ => _.IsLifted, _ => _.IsLiftedToNull);
            return _eq ? base.VisitBinary(node) : node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            var other = (ConstantExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.Value);
            return _eq ? base.VisitConstant(node) : node;
        }

        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            var other = (DebugInfoExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.EndColumn, _ => _.EndLine, _ => _.IsClear, _ => _.StartLine, _ => _.StartColumn);
            return _eq ? base.VisitDebugInfo(node) : node;
        }

        protected override Expression VisitGoto(GotoExpression node)
        {
            var other = (GotoExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.Kind, _ => _.Target);
            return _eq ? base.VisitGoto(node) : node;
        }

        protected override Expression VisitIndex(IndexExpression node)
        {
            var other = (IndexExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.Indexer);
            return _eq ? base.VisitIndex(node) : node;
        }

        protected override Expression VisitLabel(LabelExpression node)
        {
            var other = (LabelExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.Target);
            return _eq ? base.VisitLabel(node) : node;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var other = (LambdaExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.Name, _ => _.TailCall);
            return _eq ? base.VisitLambda(node) : node;
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            var other = (ListInitExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.Initializers);
            return _eq ? base.VisitListInit(node) : node;
        }

        protected override Expression VisitLoop(LoopExpression node)
        {
            var other = (LoopExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.BreakLabel, _ => _.ContinueLabel);
            return _eq ? base.VisitLoop(node) : node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var other = (MemberExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.Member);
            return _eq ? base.VisitMember(node) : node;
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            var other = (MemberInitExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.Bindings);
            return _eq ? base.VisitMemberInit(node) : node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var other = (MethodCallExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.Method);
            return _eq ? base.VisitMethodCall(node) : node;
        }

        protected override Expression VisitNew(NewExpression node)
        {
            var other = (NewExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.Constructor, _ => _.Members);
            return _eq ? base.VisitNew(node) : node;
        }

        protected override Expression VisitSwitch(SwitchExpression node)
        {
            var other = (SwitchExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.Comparison);
            return _eq ? base.VisitSwitch(node) : node;
        }

        protected override Expression VisitTry(TryExpression node)
        {
            var other = (TryExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.Handlers);
            return _eq ? base.VisitTry(node) : node;
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            var other = (TypeBinaryExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.TypeOperand);
            return _eq ? base.VisitTypeBinary(node) : node;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            var other = (UnaryExpression)_current;
            _eq &= node.IsEqualTo(other, _ => _.Method, _ => _.IsLifted, _ => _.IsLiftedToNull);
            return _eq ? base.VisitUnary(node) : node;
        }
    }
}