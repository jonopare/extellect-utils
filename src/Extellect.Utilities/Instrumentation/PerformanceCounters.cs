using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
#if NETFULL
public static class PerformanceCounters
{
	private static Dictionary<int, PerformanceCounter> counters = new Dictionary<int, PerformanceCounter>();

	public static PerformanceCounter PercentProcessorTime
	{
		get 
		{
			PerformanceCounter counter = null;
			if (!counters.TryGetValue(1, out counter))
			{
				counter = new PerformanceCounter(@"Processor", @"% Processor Time", @"_Total");
				counters[1] = counter;
			}
			return counter;
		}
	}
}
#endif
