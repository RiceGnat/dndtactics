namespace RPGEngine
{
    /// <summary>
    /// Defines a short and a full version of a message.
    /// </summary>
    public class LoggableString : ILoggable
    {
        private string msg;
        private string fullMsg;

        /// <summary>
        /// Creates a new LoggableString from a string.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="fullMessage">(optional) The full version of the message; if omitted, the message parameter will be used</param>
        public LoggableString(string message, string fullMessage = null)
        {
            msg = message;
            fullMsg = fullMessage;
        }

        /// <summary>
        /// Gets the summary version of the message.
        /// </summary>
        public string Summary
        {
            get { return msg; }
        }

        /// <summary>
        /// Gets the full version of the message;
        /// </summary>
        public string Full
        {
            get { return fullMsg ?? msg; }
        }

        /// <summary>
        /// Gets the summary version of the message.
        /// </summary>
        public override string ToString()
        {
            return Summary;
        }
    }
}
