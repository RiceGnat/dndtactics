namespace RPGEngine
{
    public class LoggableString : ILoggable
    {
        private string msg;
        private string fullMsg;

        public LoggableString(string message, string fullMessage = null)
        {
            msg = message;
            fullMsg = fullMessage;
        }

        public string Summary
        {
            get { return msg; }
        }

        public string Full
        {
            get { return fullMsg ?? msg; }
        }

        public override string ToString()
        {
            return Summary;
        }
    }
}
