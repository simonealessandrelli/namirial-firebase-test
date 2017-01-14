namespace Namirial.Firebase.Test.FCM.Firebase {
    public static class FirebaseCloudMessages {
        public static FirebaseMessage NotificationTest(string value, int attempt, string messageId) {
            return new FirebaseMessage() {
                Body = $"[{value}]: Notifica di prova",
                Data = new {
                    messageId = messageId,
                    message = $"Notifica {attempt} al dispositivo"
                }
            };
        }
    }
}
