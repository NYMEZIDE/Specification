﻿using Specification.Tests.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Specification.Tests.OnFalseActionsTests
{
    /// <summary>
    ///               isAny (|)
    ///             spec1   spec2
    ///        
    /// candidate    true    true         When_spec1_true_spec2_true_ActionDontExecute 
    /// candidate    true   false         When_spec1_true_spec2_false_ActionDontExecute    
    /// candidate   false    true         When_spec1_false_spec2_true_ActionDontExecute    
    /// candidate   false   false         When_spec1_false_spec2_false_ActionExecuteForAll 
    /// 
    /// </summary>
    public class Is_Any_Tests
    {
        [Fact]
        public void When_spec1_true_spec2_true_ActionDontExecute()
        {
            Movie movie = new Movie()
            {
                Rating = 4,
                MpaaRating = MpaaRating.G
            };

            var spec1 = new Spec<Movie>(m => m.Rating > 3);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.G);

            var actionValue1 = false;
            var actionValue2 = false;

            spec1.OnFalseAction = (s, c) => actionValue1 = true;
            spec2.OnFalseAction = (s, c) => actionValue2 = true;

            movie.IsAny(spec1, spec2);

            Assert.False(actionValue1);
            Assert.False(actionValue2);
        }

        [Fact]
        public void When_spec1_true_spec2_false_ActionDontExecute()
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

            movie.IsAny(spec1, spec2);

            Assert.False(actionValue1);
            Assert.False(actionValue2);
        }

        [Fact]
        public void When_spec1_false_spec2_true_ActionDontExecute()
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

            movie.IsAny(spec1, spec2);

            Assert.False(actionValue1);
            Assert.False(actionValue2);
        }

        [Fact]
        public void When_spec1_false_spec2_false_ActionExecuteForAll()
        {
            Movie movie = new Movie()
            {
                Rating = 4,
                MpaaRating = MpaaRating.G
            };

            var spec1 = new Spec<Movie>(m => m.Rating > 5);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.R);

            var actionValue1 = false;
            var actionValue2 = false;

            spec1.OnFalseAction = (s, c) => actionValue1 = true;
            spec2.OnFalseAction = (s, c) => actionValue2 = true;

            movie.IsAny(spec1, spec2);

            Assert.True(actionValue1);
            Assert.True(actionValue2);
        }
    }
}
