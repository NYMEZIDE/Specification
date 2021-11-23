using Specification.Tests.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Specification.Tests.OnFalseActionsTests
{
    /// <summary>
    ///                    
    ///                             spec   
    /// 
    /// candidate1 candidate2   true    true        When_candidate1_true_candidate2_true_ActionDontExecute
    /// candidate1 candidate2   true    false       When_candidate1_true_candidate2_false_ActionExecute    
    /// candidate1 candidate2   false   true        When_candidate1_false_candidate2_true_ActionExecute
    /// candidate1 candidate2   false   false       When_candidate1_false_candidate2_false_ActionExecute
    /// 
    /// 
    ///                             And (&)         
    ///                         spec1   spec2      
    ///                                            
    /// candidate1 candidate2   t  t    t   f     When_c1s1_c2s2_c1s2_true_and_c2s2_false_ActionExecuteForSpec2 
    /// candidate1 candidate2   t  t    f   t     When_c1s1_c2s2_c2s2_true_and_c1s2_false_ActionExecuteForSpec2
    /// 
    /// candidate1 candidate2   t  f    t   t     When_c1s1_c1s2_c2s2_true_and_c2s1_false_ActionExecuteForSpec1
    /// candidate1 candidate2   f  t    t   t     When_c2s1_c1s1_c2s2_true_and_c1s1_false_ActionExecuteForSpec1
    /// 
    /// candidate1 candidate2   t  f    f   t     When_c1s1_c2s2_true_and_c2s1_c1s2_false_ActionExecuteForAll
    /// 
    ///                             Or (|)         
    ///                         spec1   spec2      
    ///                                            
    /// candidate1 candidate2   t  t    t   f     When_c1s1_c2s2_c1s2_true_or_c2s2_false_ActionDontExecute 
    /// candidate1 candidate2   t  t    f   t     When_c1s1_c2s2_c2s2_true_or_c1s2_false_ActionDontExecute 
    /// 
    /// candidate1 candidate2   t  f    t   t     When_c1s1_c1s2_c2s2_true_or_c2s1_false_ActionDontExecute
    /// candidate1 candidate2   f  t    t   t     When_c2s1_c1s1_c2s2_true_or_c1s1_false_ActionDontExecute
    /// 
    /// candidate1 candidate2   t  f    f   t     When_c1s1_c2s2_true_or_c2s1_c1s2_false_ActionDontExecute
    /// 
    /// </summary>
    public class All_Is_Tests
    {
        [Fact]
        public void When_candidate1_true_candidate2_true_ActionDontExecute()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 5
                },
                new Movie()
                {
                    Rating = 6
                }
            };

            var spec = new Spec<Movie>(m => m.Rating > 4);

            var actionValue1 = false;

            spec.OnFalseAction = (s, c) => actionValue1 = true;

            Assert.True(movies.AllIs(spec));

            Assert.False(actionValue1);
        }

        [Fact]
        public void When_candidate1_true_candidate2_false_ActionExecute()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 5
                },
                new Movie()
                {
                    Rating = 6
                }
            };

            var spec = new Spec<Movie>(m => m.Rating == 5);

            var actionValue1 = false;

            spec.OnFalseAction = (s, c) => actionValue1 = true;

            Assert.False(movies.AllIs(spec));

            Assert.True(actionValue1);
        }

        [Fact]
        public void When_candidate1_false_candidate2_true_ActionExecute()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 5
                },
                new Movie()
                {
                    Rating = 6
                }
            };

            var spec = new Spec<Movie>(m => m.Rating == 6);

            var actionValue1 = false;

            spec.OnFalseAction = (s, c) => actionValue1 = true;

            Assert.False(movies.AllIs(spec));

            Assert.True(actionValue1);
        }

        [Fact]
        public void When_candidate1_false_candidate2_false_Action()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 5
                },
                new Movie()
                {
                    Rating = 6
                }
            };

            var spec = new Spec<Movie>(m => m.Rating > 7);

            var actionValue1 = false;

            spec.OnFalseAction = (s, c) => actionValue1 = true;

            Assert.False(movies.AllIs(spec));

            Assert.True(actionValue1);
        }

        [Fact]
        public void When_c1s1_c2s2_c1s2_true_and_c2s2_false_ActionExecuteForSpec2()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 5,
                    MpaaRating = MpaaRating.G
                },
                new Movie()
                {
                    Rating = 6,
                    MpaaRating = MpaaRating.R
                }
            };

            var spec1 = new Spec<Movie>(m => m.Rating > 4);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.G);

            var actionValue1 = false;
            var actionValue2 = false;

            spec1.OnFalseAction = (s, c) => actionValue1 = true;
            spec2.OnFalseAction = (s, c) => actionValue2 = true;

            Assert.False(movies.AllIs(spec1 & spec2));

            Assert.False(actionValue1);
            Assert.True(actionValue2);
        }

        [Fact]
        public void When_c1s1_c2s2_c2s2_true_and_c1s2_false_ActionExecuteForSpec2()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 5,
                    MpaaRating = MpaaRating.G
                },
                new Movie()
                {
                    Rating = 6,
                    MpaaRating = MpaaRating.R
                }
            };

            var spec1 = new Spec<Movie>(m => m.Rating > 4);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.R);

            var actionValue1 = false;
            var actionValue2 = false;

            spec1.OnFalseAction = (s, c) => actionValue1 = true;
            spec2.OnFalseAction = (s, c) => actionValue2 = true;

            Assert.False(movies.AllIs(spec1 & spec2));

            Assert.False(actionValue1);
            Assert.True(actionValue2);
        }

        [Fact]
        public void When_c1s1_c1s2_c2s2_true_and_c2s1_false_ActionExecuteForSpec1()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 5,
                    MpaaRating = MpaaRating.G
                },
                new Movie()
                {
                    Rating = 6,
                    MpaaRating = MpaaRating.G
                }
            };

            var spec1 = new Spec<Movie>(m => m.Rating == 5);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.G);

            var actionValue1 = false;
            var actionValue2 = false;

            spec1.OnFalseAction = (s, c) => actionValue1 = true;
            spec2.OnFalseAction = (s, c) => actionValue2 = true;

            Assert.False(movies.AllIs(spec1 & spec2));

            Assert.True(actionValue1);
            Assert.False(actionValue2);
        }

        [Fact]
        public void When_c2s1_c1s1_c2s2_true_and_c1s1_false_ActionExecuteForSpec1()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 5,
                    MpaaRating = MpaaRating.G
                },
                new Movie()
                {
                    Rating = 6,
                    MpaaRating = MpaaRating.G
                }
            };

            var spec1 = new Spec<Movie>(m => m.Rating == 6);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.G);

            var actionValue1 = false;
            var actionValue2 = false;

            spec1.OnFalseAction = (s, c) => actionValue1 = true;
            spec2.OnFalseAction = (s, c) => actionValue2 = true;

            Assert.False(movies.AllIs(spec1 & spec2));

            Assert.True(actionValue1);
            Assert.False(actionValue2);
        }

        [Fact]
        public void When_c1s1_c2s2_true_and_c2s1_c1s2_false_ActionExecuteForAll()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 5,
                    MpaaRating = MpaaRating.G
                },
                new Movie()
                {
                    Rating = 6,
                    MpaaRating = MpaaRating.R
                }
            };

            var spec1 = new Spec<Movie>(m => m.Rating == 5);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.R);

            var actionValue1 = false;
            var actionValue2 = false;

            spec1.OnFalseAction = (s, c) => actionValue1 = true;
            spec2.OnFalseAction = (s, c) => actionValue2 = true;

            Assert.False(movies.AllIs(spec1 & spec2));

            Assert.True(actionValue1);
            Assert.True(actionValue2);
        }


        //
        [Fact]
        public void When_c1s1_c2s2_c1s2_true_or_c2s2_false_ActionDontExecute()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 5,
                    MpaaRating = MpaaRating.G
                },
                new Movie()
                {
                    Rating = 6,
                    MpaaRating = MpaaRating.R
                }
            };

            var spec1 = new Spec<Movie>(m => m.Rating > 4);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.G);

            var actionValue1 = false;
            var actionValue2 = false;

            spec1.OnFalseAction = (s, c) => actionValue1 = true;
            spec2.OnFalseAction = (s, c) => actionValue2 = true;

            Assert.True(movies.AllIs(spec1 | spec2));

            Assert.False(actionValue1);
            Assert.False(actionValue2);
        }

        [Fact]
        public void When_c1s1_c2s2_c2s2_true_or_c1s2_false_ActionDontExecute()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 5,
                    MpaaRating = MpaaRating.G
                },
                new Movie()
                {
                    Rating = 6,
                    MpaaRating = MpaaRating.R
                }
            };

            var spec1 = new Spec<Movie>(m => m.Rating > 4);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.R);

            var actionValue1 = false;
            var actionValue2 = false;

            spec1.OnFalseAction = (s, c) => actionValue1 = true;
            spec2.OnFalseAction = (s, c) => actionValue2 = true;

            Assert.True(movies.AllIs(spec1 | spec2));

            Assert.False(actionValue1);
            Assert.False(actionValue2);
        }

        [Fact]
        public void When_c1s1_c1s2_c2s2_true_or_c2s1_false_ActionDontExecute()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 5,
                    MpaaRating = MpaaRating.G
                },
                new Movie()
                {
                    Rating = 6,
                    MpaaRating = MpaaRating.G
                }
            };

            var spec1 = new Spec<Movie>(m => m.Rating == 5);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.G);

            var actionValue1 = false;
            var actionValue2 = false;

            spec1.OnFalseAction = (s, c) => actionValue1 = true;
            spec2.OnFalseAction = (s, c) => actionValue2 = true;

            Assert.True(movies.AllIs(spec1 | spec2));

            Assert.False(actionValue1);
            Assert.False(actionValue2);
        }

        [Fact]
        public void When_c2s1_c1s1_c2s2_true_or_c1s1_false_ActionDontExecute()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 5,
                    MpaaRating = MpaaRating.G
                },
                new Movie()
                {
                    Rating = 6,
                    MpaaRating = MpaaRating.G
                }
            };

            var spec1 = new Spec<Movie>(m => m.Rating == 6);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.G);

            var actionValue1 = false;
            var actionValue2 = false;

            spec1.OnFalseAction = (s, c) => actionValue1 = true;
            spec2.OnFalseAction = (s, c) => actionValue2 = true;

            Assert.True(movies.AllIs(spec1 | spec2));

            Assert.False(actionValue1);
            Assert.False(actionValue2);
        }

        [Fact]
        public void When_c1s1_c2s2_true_or_c2s1_c1s2_false_ActionDontExecute()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 5,
                    MpaaRating = MpaaRating.G
                },
                new Movie()
                {
                    Rating = 6,
                    MpaaRating = MpaaRating.R
                }
            };

            var spec1 = new Spec<Movie>(m => m.Rating == 5);
            var spec2 = new Spec<Movie>(m => m.MpaaRating == MpaaRating.R);

            var actionValue1 = false;
            var actionValue2 = false;

            spec1.OnFalseAction = (s, c) => actionValue1 = true;
            spec2.OnFalseAction = (s, c) => actionValue2 = true;

            Assert.True(movies.AllIs(spec1 | spec2));

            Assert.False(actionValue1);
            Assert.False(actionValue2);
        }

        [Fact]
        public void CounterByOnFalseActions()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 1
                },
                new Movie()
                {
                    Rating = 2
                },
                new Movie()
                {
                    Rating = 3
                },
                new Movie()
                {
                    Rating = 4
                },
                new Movie()
                {
                    Rating = 5
                },
                new Movie()
                {
                    Rating = 6
                },
                new Movie()
                {
                    Rating = 7
                },
                new Movie()
                {
                    Rating = 8
                },
                new Movie()
                {
                    Rating = 9
                },
                new Movie()
                {
                    Rating = 10
                },
            };

            var counter = 0;
            var spec = new Spec<Movie>(m => m.Rating % 2 == 0)
            {
                OnFalseAction = (s, c) => counter++
            };

            movies.AllIs(spec);

            Assert.Equal(5, counter);
        }

        [Fact] 
        public void When_notSpec_isFalse_ActionExecute()
        {
            IEnumerable<Movie> movies = new List<Movie>
            {
                new Movie()
                {
                    Rating = 5
                },
                new Movie()
                {
                    Rating = 6
                }
            };

            var actionValue1 = false;
            var spec = new Spec<Movie>(m => m.Rating == 5)
            {
                OnFalseAction = (s, c) => actionValue1 = true
            };
            var notSpec = spec.Not();

            Assert.False(movies.AllIs(notSpec));

            Assert.True(actionValue1);
        }
    }
}
