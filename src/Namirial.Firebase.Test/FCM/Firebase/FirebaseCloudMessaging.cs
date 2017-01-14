using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json.Serialization;

namespace Namirial.Firebase.Test.FCM.Firebase {
    public class FirebaseCloudMessaging : ICloudMessaging{

        private readonly string _uri;
        private readonly string _key;
        private readonly string _env;
        private readonly JsonSerializerSettings _settings;

        public FirebaseCloudMessaging() {
            _settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
        public FirebaseCloudMessaging(string uri, string key, string env) :base(){
            _uri = uri;
            _key = key;
            _env = env;
        }

        public string SendMessageToTopic(string topic, FirebaseMessage message) {
            using (var client = new HttpClient()) {
                //Create a new Json for FCM
                var topicName = $"{topic}-{_env}";
                var jsonMessage = new FirebaseMessageJson(topicName, message);
                //Prepare POST 
                string postBody = JsonConvert.SerializeObject(jsonMessage, _settings).ToString();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string SERVER_KEY = $"key={_key}";
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", SERVER_KEY);

                var response = client.PostAsync(_uri, new StringContent(postBody, Encoding.UTF8, "application/json"));
                var responseString = response.Result.Content.ReadAsStringAsync().Result;

                return responseString;
            }
        }

        public string SendMessageToDevice(string device, FirebaseMessage message) {
            using (var client = new HttpClient()) {
                //Create a new Json for FCM
                var jsonMessage = new FirebaseMessageJson(device, message);
                //Prepare POST 
                string postBody = JsonConvert.SerializeObject(jsonMessage, _settings).ToString();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string SERVER_KEY = $"key={_key}";
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", SERVER_KEY);

                var response = client.PostAsync(_uri, new StringContent(postBody, Encoding.UTF8, "application/json"));
                var responseString = response.Result.Content.ReadAsStringAsync().Result;

                return responseString;
            }
        }
    }

    [DataContract]
    public class FirebaseMessageJson {
        [DataMember(Name = "to")]
        public string To { get; set; }
        [DataMember(Name = "data")]
        public object Data { get; set; }
        [DataMember(Name = "notification")]
        public FirebaseNotificationSectionJson Notification { get; set; }
      
        public FirebaseMessageJson(string to, FirebaseMessage message) {
            if (string.IsNullOrEmpty(to)) {
                throw new ArgumentNullException(nameof(to), "Invalid topic name");
            }
            if (message == null) {
                throw new ArgumentNullException(nameof(message), "Invalid message");
            }
            To = to;
            Data = message.Data;
            Notification = new FirebaseNotificationSectionJson(message.Title, message.Body);
        }
    }

    [DataContract(Name = "notification")]
    public class FirebaseNotificationSectionJson {
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "body")]
        public string Body { get; set; }
        public FirebaseNotificationSectionJson(string title, string body) {
            Title = title;
            Body = body;
        }
    }

    [DataContract]
    public class FirebaseTopicsResponse {
        [DataMember(Name = "message_id")]
        public string MessageId { get; set; }
        [DataMember(Name = "error")]
        public string Error { get; set; }
    }
}
