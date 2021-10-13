using System;
using System.Collections.Generic;
using System.Text;

namespace Specifications.Tests.Entities
{
    public class Aggregate
    {
        public Guid Id { get; set; }

        public ICollection<Child> Childs { get; set; }

        public State State { get; set; }
    }

    public class Child
    {
        public State State { get; set; }

        public string Name { get; set; }
    }

    public enum State
    {
        noActive,
        active
    }
}
