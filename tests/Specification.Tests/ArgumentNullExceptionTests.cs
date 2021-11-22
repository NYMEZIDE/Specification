using System;
using System.Linq.Expressions;
using Specification.Tests.Entities;
using Xunit;

namespace Specification.Tests
{
    public class ArgumentNullExceptionTests
    {
        [Fact]
        public void When_Left_Spec_And_Null_Then_Throw()
        {
            Expression<Func<Movie, bool>> expression = (m) => m.Rating > 5 && m.MpaaRating == MpaaRating.R;
            var spec = new Spec<Movie>(expression);
            
            Assert.Throws<ArgumentNullException>(() => spec.And(null));
        }
        
        [Fact]
        public void When_Right_Spec_And_Null_Then_Throw()
        {
            Expression<Func<Movie, bool>> expression = (m) => m.Rating > 5 && m.MpaaRating == MpaaRating.R;
            var spec = new Spec<Movie>(expression);
            
            Assert.Throws<ArgumentNullException>(() => (null as Spec<Movie>).And(spec));
        }
        
        [Fact]
        public void When_Left_Spec_Or_Null_Then_Throw()
        {
            Expression<Func<Movie, bool>> expression = (m) => m.Rating > 5 && m.MpaaRating == MpaaRating.R;
            var spec = new Spec<Movie>(expression);
            
            Assert.Throws<ArgumentNullException>(() => spec.Or(null));
        }
        
        [Fact]
        public void When_Right_Spec_Or_Null_Then_Throw()
        {
            Expression<Func<Movie, bool>> expression = (m) => m.Rating > 5 && m.MpaaRating == MpaaRating.R;
            var spec = new Spec<Movie>(expression);
            
            Assert.Throws<ArgumentNullException>(() => (null as Spec<Movie>).Or(spec));
        }
        
        [Fact]
        public void When_Not_Spec_is_Null_Then_Throw()
        {
            Assert.Throws<ArgumentNullException>(() => (null as Spec<Movie>).Not());
        }
    }
}