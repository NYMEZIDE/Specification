using Specifications.Tests.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Specifications.Tests
{
    public class AggregateTests
    {
        [Fact]
        public void When_Aggregate_is_specify_activeChild_Then_True()
        {
            Aggregate aggregate = new Aggregate
            {
                Id = Guid.NewGuid(),
                State = State.active,
                Childs = new List<Child>
                {
                    new Child() { Name = "name1", State = State.noActive },
                    new Child() { Name = "name2", State = State.active },
                    new Child() { Name = "name3", State = State.noActive },
                }
            };

            AbstractSpec<Aggregate> spec = new Spec<Aggregate>(x => x.Childs.Any(c => c.State == State.active));

            Assert.True(aggregate.Is(spec));
        }

        [Fact]
        public void When_Aggregate_is_noSpecify_activeChild_Then_False()
        {
            Aggregate aggregate = new Aggregate
            {
                Id = Guid.NewGuid(),
                State = State.active,
                Childs = new List<Child>
                {
                    new Child() { Name = "name1", State = State.noActive },
                    new Child() { Name = "name2", State = State.noActive },
                    new Child() { Name = "name3", State = State.noActive },
                }
            };

            AbstractSpec<Aggregate> spec = new Spec<Aggregate>(x => x.Childs.Any(c => c.State == State.active));

            Assert.False(aggregate.Is(spec));
        }

        [Fact]
        public void When_Aggregate_is_noChilds_Then_False()
        {
            Aggregate aggregate = new Aggregate
            {
                Id = Guid.NewGuid(),
                State = State.active,
                Childs = new List<Child>
                {
                    
                }
            };

            AbstractSpec<Aggregate> spec = new Spec<Aggregate>(x => x.Childs.Any(c => c.State == State.active));

            Assert.False(aggregate.Is(spec));
        }

        [Fact]
        public void When_Aggregate_active_and_haveActiveChild_Then_True()
        {
            Aggregate aggregate = new Aggregate
            {
                Id = Guid.NewGuid(),
                State = State.active,
                Childs = new List<Child>
                {
                    new Child() { Name = "name1", State = State.noActive },
                    new Child() { Name = "name2", State = State.active },
                    new Child() { Name = "name3", State = State.noActive },
                }
            };

            AbstractSpec<Aggregate> spec = new Spec<Aggregate>(x => x.State == State.active && x.Childs.Any(c => c.State == State.active));

            Assert.True(aggregate.Is(spec));
        }

        [Fact]
        public void When_Aggregate_activeSpec_and_haveActiveChildSpec_Then_True()
        {
            Aggregate aggregate = new Aggregate
            {
                Id = Guid.NewGuid(),
                State = State.active,
                Childs = new List<Child>
                {
                    new Child() { Name = "name1", State = State.noActive },
                    new Child() { Name = "name2", State = State.active },
                    new Child() { Name = "name3", State = State.noActive },
                }
            };

            AbstractSpec<Aggregate> spec1 = new Spec<Aggregate>(x => x.State == State.active);
            AbstractSpec<Aggregate> spec2 = new Spec<Aggregate>(x =>x.Childs.Any(c => c.State == State.active));

            Assert.True(aggregate.Is(spec1 & spec2));
        }
    }
}
