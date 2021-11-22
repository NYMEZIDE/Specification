using System;

namespace Specification.Tests.Entities
{
    public class Movie
    {
        public double Rating { get; set; }

        public TimeSpan Duration { get; set; }

        public MpaaRating MpaaRating { get; set; }
    }

    public enum MpaaRating
    {
        G = 1,
        PG13 = 2,
        R = 3
    }
}
