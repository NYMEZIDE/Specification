using Specification.Tests.Entities;
using System;
using System.Linq.Expressions;
using Xunit;

namespace Specification.Tests
{
    public class SpecTests
    {
        [Fact]
        public void When_Spec_set_Expression_Then_CorrectEqual()
        {
            Expression<Func<Movie, bool>> expression = (m) => m.Rating > 5 && m.MpaaRating == MpaaRating.R;
            var spec = new Spec<Movie>(expression);
            
            Assert.Equal(spec.Expression, expression);
        }
        
        [Fact]
        public void When_Spec_ToString_Then_EqualExpressionToString()
        {
            Expression<Func<Movie, bool>> expression = (m) => m.Rating > 5 && m.MpaaRating == MpaaRating.R;
            var spec = new Spec<Movie>(expression);
            
            Assert.Equal(spec.ToString(), expression.ToString());
        }

        [Fact]
        public void When_Spec_Expression_ToString_Then_Equal()
        {
            Expression<Func<Movie, bool>> expression = (m) => m.Rating > 5 && m.MpaaRating == MpaaRating.R;
            var spec = new Spec<Movie>(expression);

            Assert.Equal(spec.Expression.ToString(), expression.ToString());
        }
        
        [Fact]
        public void When_Specs_Equal_Null_Then_False()
        {
            var spec = new Spec<Movie>((m) => m.Rating > 5 && m.MpaaRating == MpaaRating.PG13);

            Assert.False(spec.Equals(null));
        }
        
        [Fact]
        public void When_TwoSpecs_different_Specs_Then_False()
        {
            var spec1 = new Spec<Movie>((m) => m.MpaaRating == MpaaRating.PG13);
            var spec2 = new Spec<MpaaRating>((m) => m == MpaaRating.PG13);
            
            Assert.False(spec1.Equals(spec2));
            Assert.NotEqual(spec1.GetHashCode(), spec2.GetHashCode());
        }
    }
}
