using Specification.Tests.Entities;
using Specification.Tests.Specs;
using System;
using Xunit;

namespace Specification.Tests
{
    public class CustomSpecificationTests
    {
        [Fact]
        public void When_Spec_is_class()
        {
            Movie movie = new Movie()
            {
                Duration = TimeSpan.FromHours(3),
                Rating = 3
            };

            AbstractSpec<Movie> boring = new BoringMovieSpec(TimeSpan.FromHours(2.5), 3);

            Assert.True(movie.Is(boring));
            Assert.True(boring.IsSatisfiedBy(movie));
        }
        
        [Fact]
        public void When_TwoSpec_is_class_and_isTrue()
        {
            Movie movie = new Movie()
            {
                Duration = TimeSpan.FromHours(3),
                Rating = 3
            };

            AbstractSpec<Movie> longest = new LongestMovieSpec(TimeSpan.FromHours(2.5));
            AbstractSpec<Movie> ratingLess = new RatingLessMovieSpec(3);

            AbstractSpec<Movie> compositeSpecs = longest.And(ratingLess);
            
            Assert.True(movie.Is(compositeSpecs));
            Assert.True(compositeSpecs.IsSatisfiedBy(movie));
        }
        
        [Fact]
        public void When_TwoSpec_is_class_and_isFalse()
        {
            Movie movie = new Movie()
            {
                Duration = TimeSpan.FromHours(3),
                Rating = 4
            };

            AbstractSpec<Movie> longest = new LongestMovieSpec(TimeSpan.FromHours(2.5));
            AbstractSpec<Movie> ratingLess = new RatingLessMovieSpec(3);

            AbstractSpec<Movie> compositeSpecs = longest.And(ratingLess);
            
            Assert.False(movie.Is(compositeSpecs));
            Assert.False(compositeSpecs.IsSatisfiedBy(movie));
        }
    }
}
