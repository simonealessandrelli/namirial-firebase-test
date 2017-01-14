namespace Namirial.Firebase.Test.FCM.Firebase {
    public class FirebaseCloudMessagingBuilder {

        public FirebaseCloudMessagingBuilder() { }

        private string _uri;
        private string _key;
        private string _env;

        public FirebaseCloudMessagingBuilder SetUri(string uri) {
            _uri = uri;
            return this;
        }

        public FirebaseCloudMessagingBuilder SetKey(string key) {
            _key = key;
            return this;
        }

        public FirebaseCloudMessagingBuilder SetEnv(string environment) {
            _env = environment;
            return this;
        } 

        public FirebaseCloudMessaging Build() {
            return new FirebaseCloudMessaging(_uri, _key, _env);
        }
    }
}
