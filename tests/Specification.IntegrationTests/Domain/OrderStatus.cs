using System;
using System.Collections.Generic;
using System.Text;

namespace Specification.IntegrationTests.Domain
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
