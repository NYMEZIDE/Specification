using Specification.Tests.Entities;
using System;
using System.Linq.Expressions;

namespace Specification.Tests.Specs
{
    public class BoringMovieSpec : AbstractSpec<Movie>
    {
        private readonly TimeSpan _durationGreaterThat;
        private readonly double _ratingLessOrEqualThat;

        public BoringMovieSpec(TimeSpan? durationGreaterThat = null, double ratingLessOrEqualThat = 4)
        {
            _durationGreaterThat = durationGreaterThat ?? TimeSpan.FromHours(2.5);
            _ratingLessOrEqualThat = ratingLessOrEqualThat;
        }

        public override Expression<Func<Movie, bool>> Expression 
            => movie => movie.Duration > _durationGreaterThat && movie.Rating <= _ratingLessOrEqualThat; 
    }
}
