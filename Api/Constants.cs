using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public static class Constants
    {
        public static readonly TimeSpan DefaultBackoffPeriod = TimeSpan.FromSeconds(30);
        public static readonly int DefaultFailuresBeforeBackoff = 3;
        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
    }
}
