using System;
using System.Linq.Expressions;
using Specification.Tests.Entities;
using Xunit;

namespace Specification.Tests
{
    public class OperatorsTests
    {
        [Fact]
        public void Operator_BitwiseAnd_Test()
        {   
            Movie movie = new Movie()
            {
                Duration = TimeSpan.FromHours(3),
                Rating = 3
            };

            AbstractSpec<Movie> longest = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(2.5));
            AbstractSpec<Movie> ratingLess = new Spec<Movie>(m => m.Rating <= 3);
            
            Assert.True(movie.Is(longest & ratingLess));
        }
        
        [Fact]
        public void Operator_BitwiseOr_Test()
        {   
            Movie movie = new Movie()
            {
                Duration = TimeSpan.FromHours(3),
                Rating = 3
            };

            AbstractSpec<Movie> longest = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(2.5));
            AbstractSpec<Movie> ratingLess = new Spec<Movie>(m => m.Rating <= 3);
            
            Assert.True(movie.Is(longest | ratingLess));
        }
        
        [Fact]
        public void Operator_LeftEquality_Test()
        {   
            Movie movie = new Movie()
            {
                Duration = TimeSpan.FromHours(3),
            };

            AbstractSpec<Movie> longest = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(2.5));
            
            Assert.True(movie.Is(longest == true));
        }

        [Fact]
        public void Operator_LeftEqualityFalse_Test()
        {
            Movie movie = new Movie()
            {
                Duration = TimeSpan.FromHours(3),
            };

            AbstractSpec<Movie> longest = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(3.5));

            Assert.True(movie.Is(longest == false));
        }

        [Fact]
        public void Operator_RightEquality_Test()
        {   
            Movie movie = new Movie()
            {
                Duration = TimeSpan.FromHours(3),
            };

            AbstractSpec<Movie> longest = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(2.5));
            
            Assert.True(movie.Is(true == longest));
        }

        [Fact]
        public void Operator_RightEqualityFalse_Test()
        {
            Movie movie = new Movie()
            {
                Duration = TimeSpan.FromHours(3),
            };

            AbstractSpec<Movie> longest = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(3.5));

            Assert.True(movie.Is(false == longest));
        }

        [Fact]
        public void Operator_LeftInequality_Test()
        {   
            Movie movie = new Movie()
            {
                Duration = TimeSpan.FromHours(2),
            };

            AbstractSpec<Movie> longest = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(2.5));
            
            Assert.True(movie.Is(longest != true));
        }

        [Fact]
        public void Operator_LeftInequalityFalse_Test()
        {
            Movie movie = new Movie()
            {
                Duration = TimeSpan.FromHours(2),
            };

            AbstractSpec<Movie> longest = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(1.5));

            Assert.True(movie.Is(longest != false));
        }

        [Fact]
        public void Operator_RightInequality_Test()
        {   
            Movie movie = new Movie()
            {
                Duration = TimeSpan.FromHours(2),
            };

            AbstractSpec<Movie> longest = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(2.5));
            
            Assert.True(movie.Is(true != longest));
        }

        [Fact]
        public void Operator_RightInequalityFalse_Test()
        {
            Movie movie = new Movie()
            {
                Duration = TimeSpan.FromHours(2),
            };

            AbstractSpec<Movie> longest = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(1.5));

            Assert.True(movie.Is(false != longest));
        }

        [Fact]
        public void Operator_Implicit_Expression_Func_T_Bool()
        {
            Expression<Func<Movie, bool>> expression;
            Movie movie = new Movie()
            {
                MpaaRating = MpaaRating.R,
                Rating = 7
            };

            AbstractSpec<Movie> spec = new Spec<Movie>(m => m.Rating > 5 && m.MpaaRating == MpaaRating.R);
            expression = spec;
            
            Assert.True(expression.Compile().Invoke(movie));
        }
        
        [Fact]
        public void Operator_Implicit_Func_T_Bool()
        {
            Func<Movie, bool> func;
            Movie movie = new Movie()
            {
                MpaaRating = MpaaRating.R,
                Rating = 7
            };

            AbstractSpec<Movie> spec = new Spec<Movie>(m => m.Rating > 5 && m.MpaaRating == MpaaRating.R);
            func = spec;
            
            Assert.True(func.Invoke(movie));
        }
    }
}