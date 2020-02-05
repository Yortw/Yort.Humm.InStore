using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Humm.InStore.Infrastructure
{
	internal static class MemoryStreamFactory
	{
		private static readonly Microsoft.IO.RecyclableMemoryStreamManager _StreamManager = new Microsoft.IO.RecyclableMemoryStreamManager();

		public static System.IO.Stream CreateStream()
		{
			return _StreamManager.GetStream();
		}
	}
}
