using System;

namespace Namirial.Firebase.Test.FCM.Firebase.Exceptions {
    public class FirebaseSendMessageException :Exception {

        /// <summary>
        /// Constructor with no parameters
        /// </summary>
        public FirebaseSendMessageException() : base() { }

        /// <summary>
        /// Constructor with message parameter
        /// </summary>
        /// <param name="message">Message of exception</param>
        public FirebaseSendMessageException(string message) : base(message) { }

        /// <summary>
        /// Constructor with message and inner exception
        /// </summary>
        /// <param name="message">Message of exception</param>
        /// <param name="innerException">Inner exception</param>
        public FirebaseSendMessageException(string message, Exception innerException) { }
    }

}
