using Specification.Tests.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Specification.Tests.OnFalseActionsTests
{
    public class Is_Tests
    {
        [Fact]
        public void When_OneSpec_Then_ActionExecute()
        {
            Movie movie = new Movie()
            {
                Rating = 4,
                MpaaRating = MpaaRating.G
            };

            var spec = new Spec<Movie>(m => m.Rating > 5 && m.MpaaRating == MpaaRating.R);
            var actionValue = false;
            spec.OnFalseAction = (s, c) => actionValue = true;

            movie.Is(spec);

            Assert.True(actionValue);
        }

        [Fact]
        public void When_FirstSpec_Is_True_Then_ActionDontExecute()
        {
            Movie movie = new Movie()
            {
                Rating = 4,
                MpaaRating = MpaaRating.G
            };

            var spec1 = new Spec<Movie>(m => m.Rating > 3);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.R);

            var actionValue1 = false;
            var actionValue2 = false;

            spec1.OnFalseAction = (s, c) => actionValue1 = true;
            spec2.OnFalseAction = (s, c) => actionValue2 = true;

            movie.Is(spec1 | spec2);

            Assert.False(actionValue1);
            Assert.False(actionValue2);
        }

        [Fact]
        public void When_SecondSpec_Is_True_Then_ActionDontExecute()
        {
            Movie movie = new Movie()
            {
                Rating = 4,
                MpaaRating = MpaaRating.G
            };

            var spec1 = new Spec<Movie>(m => m.Rating > 5);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.G);

            var actionValue1 = false;
            var actionValue2 = false;

            spec1.OnFalseAction = (s, c) => actionValue1 = true;
            spec2.OnFalseAction = (s, c) => actionValue2 = true;

            movie.Is(spec1 | spec2);

            Assert.False(actionValue1);
            Assert.False(actionValue2);
        }

        [Fact]
        public void When_specCascade_isFalse_ActionExecute()
        {
            Movie movie = new Movie()
            {
                Rating = 4,
                MpaaRating = MpaaRating.G,
                Duration = TimeSpan.FromHours(2.5) 
            };

            var ratingSpecActionResult = false;
            var mpaaRatingSpecActionResult = false;
            var durationSpecActionResult = false;

            var ratingSpec = new Spec<Movie>(m => m.Rating > 5)
            {
                OnFalseAction = (s, c) => ratingSpecActionResult = true
            };
            var mpaaRatingSpec = new Spec<Movie>(m => m.MpaaRating == MpaaRating.PG13)
            {
                OnFalseAction = (s, c) => mpaaRatingSpecActionResult = true
            };
            var durationSpec = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(2));

            var compositeRatingsSpecs = ratingSpec.Or(mpaaRatingSpec);
            var compositeSpecs = durationSpec.And(compositeRatingsSpecs);

            Assert.False(movie.Is(compositeSpecs));

            Assert.True(ratingSpecActionResult);
            Assert.True(mpaaRatingSpecActionResult);
            Assert.False(durationSpecActionResult);
        }
    }
}
