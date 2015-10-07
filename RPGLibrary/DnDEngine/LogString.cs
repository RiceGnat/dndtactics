namespace DnDEngine
{
	public class LogString : ILoggable
	{
		public string Message { get; private set; }
		public string ShortMessage { get; private set; }

		public LogString(string message, string shortMessage = null)
		{
			Message = message;
			ShortMessage = shortMessage;
		}

		string ILoggable.Inline
		{
			get { return ShortMessage ?? Message; }
		}

		string ILoggable.Full
		{
			get { return Message; }
		}
	}
}
