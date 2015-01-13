using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bond.PerformanceTest
{
    public static class MeasurementEx
    {
        public static TimeSpan[] Call(this Action action, int times, int warmUp = 5)
        {
            return Enumerable
                .Range(0, times + warmUp)
                .Select(_ => Measure(action))
                .Skip(warmUp)
                .ToArray();
        }

        public static TimeSpan Average(this IEnumerable<TimeSpan> times)
        {
            double avgTicks = times.Average(t => t.Ticks);
            return TimeSpan.FromTicks((long) avgTicks);
        }

        public static TimeSpan Measure(Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            action();

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
