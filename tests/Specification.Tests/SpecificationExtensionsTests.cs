using Specification.Tests.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Specification.Tests
{
    public class SpecificationExtensionsTests
    {
        [Fact]
        public void When_Is_True()
        {
            Movie movie = new Movie()
            {
                Rating = 3
            };

            var spec = new Spec<Movie>(m => m.Rating == 3);

            Assert.True(movie.Is(spec));
        }

        [Fact]
        public void When_Is_False()
        {
            Movie movie = new Movie()
            {
                Rating = 3
            };

            var spec = new Spec<Movie>(m => m.Rating == 5);

            Assert.False(movie.Is(spec));
        }

        [Fact]
        public void When_IsAny_True()
        {
            Movie movie = new Movie()
            {
                Rating = 3,
                
            };

            var spec1 = new Spec<Movie>(m => m.Rating == 2);
            var spec2 = new Spec<Movie>(m => m.Rating == 3);
            var spec3 = new Spec<Movie>(m => m.Rating == 4);

            Assert.True(movie.IsAny(spec1, spec2, spec3));
        }

        [Fact]
        public void When_IsAny_False()
        {
            Movie movie = new Movie()
            {
                Rating = 5
            };

            var spec1 = new Spec<Movie>(m => m.Rating == 2);
            var spec2 = new Spec<Movie>(m => m.Rating == 3);
            var spec3 = new Spec<Movie>(m => m.Rating == 4);

            Assert.False(movie.IsAny(spec1, spec2, spec3));
        }

        [Fact]
        public void When_IsAll_True()
        {
            Movie movie = new Movie()
            {
                Rating = 3,
                MpaaRating = MpaaRating.R,
                Duration = TimeSpan.FromHours(2.5)
            };

            var spec1 = new Spec<Movie>(m => m.Rating == 3);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.R);
            var spec3 = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(2));

            Assert.True(movie.IsAll(spec1, spec2, spec3));
        }

        [Fact]
        public void When_IsAll_False()
        {
            Movie movie = new Movie()
            {
                Rating = 3,
                MpaaRating = MpaaRating.R,
                Duration = TimeSpan.FromHours(2.5)
            };

            var spec1 = new Spec<Movie>(m => m.Rating == 3);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.PG13);
            var spec3 = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(2));

            Assert.False(movie.IsAll(spec1, spec2, spec3));
        }

        [Fact]
        public void When_AnyIs_True()
        {
            ICollection<Movie> movies = new List<Movie>
            {
                new Movie { Rating = 2 },
                new Movie { Rating = 3 },
                new Movie { Rating = 4 },
                new Movie { Rating = 5 },
                new Movie { Rating = 6 },
            };

            var spec = new Spec<Movie>(m => m.Rating > 5);

            Assert.True(movies.AnyIs(spec));
        }

        [Fact]
        public void When_AnyIs_False()
        {
            ICollection<Movie> movies = new List<Movie>
            {
                new Movie { Rating = 2 },
                new Movie { Rating = 3 },
                new Movie { Rating = 4 },
                new Movie { Rating = 5 },
                new Movie { Rating = 6 },
            };

            var spec = new Spec<Movie>(m => m.Rating > 7);

            Assert.False(movies.AnyIs(spec));
        }

        [Fact]
        public void When_AllIs_True()
        {
            ICollection<Movie> movies = new List<Movie>
            {
                new Movie { Rating = 2 },
                new Movie { Rating = 3 },
                new Movie { Rating = 4 },
                new Movie { Rating = 5 },
                new Movie { Rating = 6 },
            };

            var spec = new Spec<Movie>(m => m.Rating >= 2);

            Assert.True(movies.AllIs(spec));
        }

        [Fact]
        public void When_AllIs_False()
        {
            ICollection<Movie> movies = new List<Movie>
            {
                new Movie { Rating = 2 },
                new Movie { Rating = 3 },
                new Movie { Rating = 4 },
                new Movie { Rating = 5 },
                new Movie { Rating = 6 },
            };

            var spec = new Spec<Movie>(m => m.Rating >= 4);

            Assert.False(movies.AllIs(spec));
        }

        [Fact]
        public void When_And_True()
        {
            Movie movie = new Movie()
            {
                Rating = 3,
                MpaaRating = MpaaRating.R,
                Duration = TimeSpan.FromHours(2.5)
            };

            var spec1 = new Spec<Movie>(m => m.Rating == 3);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.R);
            var spec3 = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(2));

            Assert.True(spec1.And(spec2).And(spec3).IsSatisfiedBy(movie));
        }

        [Fact]
        public void When_And_False()
        {
            Movie movie = new Movie()
            {
                Rating = 3,
                MpaaRating = MpaaRating.R,
                Duration = TimeSpan.FromHours(2.5)
            };

            var spec1 = new Spec<Movie>(m => m.Rating == 3);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.PG13);
            var spec3 = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(2));

            Assert.False(spec1.And(spec2).And(spec3).IsSatisfiedBy(movie));
        }

        [Fact]
        public void When_Or_True()
        {
            Movie movie = new Movie()
            {
                Rating = 3,
                MpaaRating = MpaaRating.R,
                Duration = TimeSpan.FromHours(2.5)
            };

            var spec1 = new Spec<Movie>(m => m.Rating > 5);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.R);
            var spec3 = new Spec<Movie>(m => m.Duration < TimeSpan.FromHours(2));

            Assert.True(spec1.Or(spec2).Or(spec3).IsSatisfiedBy(movie));
        }

        [Fact]
        public void When_Or_False()
        {
            Movie movie = new Movie()
            {
                Rating = 3,
                MpaaRating = MpaaRating.R,
                Duration = TimeSpan.FromHours(2.5)
            };

            var spec1 = new Spec<Movie>(m => m.Rating > 5);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.PG13);
            var spec3 = new Spec<Movie>(m => m.Duration < TimeSpan.FromHours(2));

            Assert.False(spec1.Or(spec2).Or(spec3).IsSatisfiedBy(movie));
        }

        [Fact]
        public void When_Not_True()
        {
            Movie movie = new Movie()
            {
                Rating = 3
            };

            var spec = new Spec<Movie>(m => m.Rating > 5);

            Assert.True(spec.Not().IsSatisfiedBy(movie));
        }

        [Fact]
        public void When_Not_False()
        {
            Movie movie = new Movie()
            {
                Rating = 3
            };

            var spec = new Spec<Movie>(m => m.Rating < 5);

            Assert.False(spec.Not().IsSatisfiedBy(movie));
        }
    }
}
