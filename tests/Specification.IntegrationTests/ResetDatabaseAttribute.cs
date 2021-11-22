using Specification.IntegrationTests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;
using Xunit.Sdk;

namespace Specification.IntegrationTests
{
    public class ResetDatabaseAttribute : BeforeAfterTestAttribute
    {
        public override void After(MethodInfo methodUnderTest)
        {
            using var db = new OrdersDbContext();
            db.Database.EnsureDeleted();

            base.After(methodUnderTest);
        }
    }
}
