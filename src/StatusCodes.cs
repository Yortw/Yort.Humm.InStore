using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Humm.InStore
{
	public static class StatusCodes
	{
		private static readonly List<Status> _Codes = new List<Status>();

		public StatusCodes()
		{
			AddCode("SCRK01", RequestStates.Success, "Success");
			AddCode("SINV01", RequestStates.Success, "Success");
			AddCode("SPRA01", RequestStates.Success, "Approved");
			AddCode("SPSA01", RequestStates.Success, "Approved");
			AddCode("SPAR01", RequestStates.Success, "Approved");
			AddCode("SSER01", RequestStates.Success, "Success");

			AddCode("SPND01", RequestStates.Pending, "Pending");

			AddCode("FCNL01", RequestStates.Cancelled, "Cancelled");

			AddCode("FPRA01", RequestStates.Failed, "Declined due to internal risk assessment against the customer");
			AddCode("FPRA02", RequestStates.Failed, "Declined due to insufficient funds for the deposit");
			AddCode("FPRA03", RequestStates.Failed, "Declined as communication to the bank is currently unavailable");
			AddCode("FPRA04", RequestStates.Failed, "");
			AddCode("FPRA05", RequestStates.Failed);
			AddCode("FPRA06", RequestStates.Failed);
			AddCode("FPRA07", RequestStates.Failed);
			AddCode("FPRA08", RequestStates.Failed);
			AddCode("FPRA09", RequestStates.Failed);
			AddCode("FPRA21", RequestStates.Failed);
			AddCode("FPRA22", RequestStates.Failed);
			AddCode("FPRA23", RequestStates.Failed);
			AddCode("FPRA24", RequestStates.Failed);
			AddCode("FPRA99", RequestStates.Failed);
			AddCode("FPSA01", RequestStates.Failed);
			AddCode("FPSA02", RequestStates.Failed);
			AddCode("FPSA03", RequestStates.Failed);
			AddCode("FPSA04", RequestStates.Failed);
			AddCode("FPSA05", RequestStates.Failed);
			AddCode("FPSA06", RequestStates.Failed);
			AddCode("FPSA07", RequestStates.Failed);
			AddCode("FPSA08", RequestStates.Failed);
			AddCode("FPSA09", RequestStates.Failed);
			AddCode("FPAR01", RequestStates.Failed);
			AddCode("FPAR02", RequestStates.Failed);
			AddCode("FPAR03", RequestStates.Failed);
			AddCode("FPAR05", RequestStates.Failed);
			AddCode("FPAR06", RequestStates.Failed);
			AddCode("FPAR07", RequestStates.Failed);
			AddCode("FPAR08", RequestStates.Failed);
			AddCode("FSER01", RequestStates.Failed);
			AddCode("FSER02", RequestStates.Failed);
			AddCode("FCRK01", RequestStates.Failed);
			AddCode("FCRK02", RequestStates.Failed);
			AddCode("EVAL01", RequestStates.Failed);
			AddCode("EVAL02", RequestStates.Failed);
			AddCode("EAUT01", RequestStates.Failed);
			AddCode("ESIG01", RequestStates.Failed);
			AddCode("EISE01", RequestStates.Failed);
		}
	}
}