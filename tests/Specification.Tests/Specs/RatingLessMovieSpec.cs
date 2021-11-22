using System;
using System.Linq.Expressions;
using Specification.Tests.Entities;

namespace Specification.Tests.Specs
{
    public class RatingLessMovieSpec : AbstractSpec<Movie>
    {
        private readonly double _ratingLessOrEqualThat;

        public RatingLessMovieSpec(double ratingLessOrEqualThat = 4)
        {
            _ratingLessOrEqualThat = ratingLessOrEqualThat;
        }

        public override Expression<Func<Movie, bool>> Expression => m => m.Rating <= _ratingLessOrEqualThat;
    }
}