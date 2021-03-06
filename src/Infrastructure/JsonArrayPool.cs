﻿using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Yort.Humm.InStore.Infrastructure
{
	internal sealed class JsonArrayPool : IArrayPool<char>
	{
		public static readonly JsonArrayPool Instance = new JsonArrayPool();

		public char[] Rent(int minimumLength)
		{
			// get char array from System.Buffers shared pool
			return ArrayPool<char>.Shared.Rent(minimumLength);
		}

		public void Return(char[] array)
		{
			// return char array to System.Buffers shared pool
			ArrayPool<char>.Shared.Return(array);
		}
	}
}
