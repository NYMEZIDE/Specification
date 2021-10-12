using System;
using System.Linq.Expressions;
using Specifications.Tests.Entities;

namespace Specifications.Tests.Specs
{
    public class LongestMovieSpec : AbstractSpec<Movie>
    {
        private readonly TimeSpan _durationGreaterThat;

        public LongestMovieSpec(TimeSpan? durationGreaterThat = null)
        {
            _durationGreaterThat = durationGreaterThat ?? TimeSpan.FromHours(2.5);
        }

        public override Expression<Func<Movie, bool>> Expression => m => m.Duration > _durationGreaterThat;
    }
}