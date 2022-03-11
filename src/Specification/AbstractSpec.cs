using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Specification.Internal;
#pragma warning disable 660,661

namespace Specification
{
    public abstract class AbstractSpec<T> : ISpecification<T>
    {
        private Func<T, bool> _compiledFunc;

        private ICollection<Action<AbstractSpec<T>, T>> _onFalseActions;

        protected void UpActions(AbstractSpec<T> spec)
        {
            _onFalseActions ??= new Collection<Action<AbstractSpec<T>, T>>();

            if (spec._onFalseActions != null)
            {
                foreach (var action in spec._onFalseActions)
                {
                    _onFalseActions.Add(action);
                }
            }

            if (spec.OnFalseAction != null)
                _onFalseActions.Add(spec.OnFalseAction);
        }

        public bool IsSatisfiedBy(T candidate)
        {
            var result = candidate != null && InternalIsSatisfiedBy(candidate);
        
            if (result is false)
            {
                if (_onFalseActions != null)
                {
                    foreach (var action in _onFalseActions)
                        action(this, candidate);
                }

                OnFalseAction?.Invoke(this, candidate);
            }

            return result;
        }

        internal bool InternalAnyIs(IEnumerable<T> candidates)
        {
            ICollection<Tuple<Action<AbstractSpec<T>, T>, T>> actions = new Collection<Tuple<Action<AbstractSpec<T>, T>, T>>();
            
            foreach (var candidate in candidates)
            {
                var result = InternalIsSatisfiedBy(candidate);

                if (result)
                    return true;
                
                if (_onFalseActions != null)
                {
                    foreach (var action in _onFalseActions)
                        actions.Add(new Tuple<Action<AbstractSpec<T>, T>, T>(action, candidate));
                }

                if (OnFalseAction != null)
                {
                    actions.Add(new Tuple<Action<AbstractSpec<T>, T>, T>(OnFalseAction, candidate));
                }
            }

            foreach (var action in actions)
            {
                action.Item1(this, action.Item2);
            }

            return false;
        }

        internal bool InternalAllIs(IEnumerable<T> candidates)
        {
            ICollection<Tuple<Action<AbstractSpec<T>, T>, T>> actions = new Collection<Tuple<Action<AbstractSpec<T>, T>, T>>();
            bool result = true;

            foreach (var candidate in candidates)
            {
                if (InternalIsSatisfiedBy(candidate))
                    continue;

                result = false;

                if (_onFalseActions != null)
                {
                    foreach (var action in _onFalseActions)
                        actions.Add(new Tuple<Action<AbstractSpec<T>, T>, T>(action, candidate));
                }

                if (OnFalseAction != null)
                {
                    actions.Add(new Tuple<Action<AbstractSpec<T>, T>, T>(OnFalseAction, candidate));
                }
            }

            foreach (var action in actions)
            {
                action.Item1(this, action.Item2);
            }

            return result;
        }

        internal virtual bool InternalIsSatisfiedBy(T candidate)
        {
            _compiledFunc ??= Expression.Compile();
            var result = _compiledFunc(candidate);

            if (result)
                _onFalseActions = null;

            return result;
        }

        public abstract Expression<Func<T, bool>> Expression { get; }

        public Action<AbstractSpec<T>, T> OnFalseAction { get; set; }

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

        #endregion
    }
}
