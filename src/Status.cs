using System;
using System.Collections.Generic;
using System.Text;
using Ladon;

namespace Yort.Humm.InStore
{
	public class Status
	{
		private readonly string _Code;
		private readonly string _Status;

		internal Status(string code, string status)
		{
			_Code = code.GuardNullOrEmpty(nameof(code));
			_Status = status.GuardNullOrEmpty(nameof(code));
		}

		public string Code => _Code;

		public string Status => _Code;

	}
}
