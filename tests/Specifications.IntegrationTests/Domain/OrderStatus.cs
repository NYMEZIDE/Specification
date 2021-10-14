using System;
using System.Collections.Generic;
using System.Text;

namespace Specifications.IntegrationTests.Domain
{
    public enum OrderStatus
    {
        created,
        accepted,
        cooked,
        packed,
        delivered,
        completed,
        rejected
    }
}
