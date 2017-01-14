using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Namirial.Firebase.Test.FCM;
using Namirial.Firebase.Test.FCM.Firebase;
using Namirial.Firebase.Test.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Namirial.Firebase.Test.Controllers {
    [Route("api/[controller]")]
    public class FirebaseController :Controller {

        private readonly ICloudMessaging _cloudMessaging;
        private readonly IConfiguration _configuration;

        public FirebaseController(ICloudMessaging cloudMessaging, IConfiguration configuration) {
            _cloudMessaging = cloudMessaging;
            _configuration = configuration;
        }

        public object JsconConvert { get; private set; }

        // POST api/firebase
        [HttpPost]
        public IActionResult Post([FromBody]NotifyTestParam param) {

            if (param == null || string.IsNullOrEmpty(param.Type)) {
                return BadRequest($"No device type specified");
            }

            try {
                //Get Url
                string firebaseApp = _configuration.GetValue<string>("FirebaseAppUrl");

                //Get list of devices
                string testUrl = $"{firebaseApp}/{param.Type}/tokens/{param.Token}.json";

                using (var client = new HttpClient()) {

                    for (int attempt = 0; attempt <= param.Attempts; attempt++) {

                        //Get time
                        int time = new DataConverterHelper(DateTime.UtcNow).GetUnixEpoch();

                        //Get new guid
                        string messageId = Guid.NewGuid().ToString();

                        //Prepare POST 
                        StringContent body = new StringContent($"{{ \"{messageId}\": {{ \"sentTime\": {time}}} }}", Encoding.UTF8, ContentTypes.ApplicationJson);

                        //Get response
                        var response = client.PatchAsync(testUrl, body);
                        var responseString = response.Result.Content.ReadAsStringAsync().Result;
                    }
                }
            } catch (Exception e) {


            }

            return Ok();
        }

        private string sendNotification(NotifyTestParam param, string token, int attempt, string messageId) {
            return _cloudMessaging.SendMessageToDevice(token, FirebaseCloudMessages.NotificationTest(param.Type, attempt, messageId));
        }
    }

    public static class HttpExtension {

        public const string MimeJson = "application/json";

        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content) {
            HttpRequestMessage request = new HttpRequestMessage {
                Method = new HttpMethod("PATCH"),
                RequestUri = new Uri(client.BaseAddress + requestUri),
                Content = content
            };

            return client.SendAsync(request);
        }

        public static Task<HttpResponseMessage> PostJsonAsync(this HttpClient client, string requestUri, Type type, object value) {
            return client.PostAsync(requestUri, new ObjectContent(type, value, new JsonMediaTypeFormatter(), MimeJson));
        }

        public static Task<HttpResponseMessage> PutJsonAsync(this HttpClient client, string requestUri, Type type, object value) {
            return client.PutAsync(requestUri, new ObjectContent(type, value, new JsonMediaTypeFormatter(), MimeJson));
        }

        public static Task<HttpResponseMessage> PatchJsonAsync(this HttpClient client, string requestUri, Type type, object value) {
            return client.PatchAsync(requestUri, new ObjectContent(type, value, new JsonMediaTypeFormatter(), MimeJson));
        }
    }
}

[DataContract]
public class Root {
    [DataMember(Name = "tokens")]
    public List<Token> tokens { get; set; }
}

[DataContract]
public class Token {
    public Token() { }
    //[DataMember(Name = "id")]
    //public string Id { get; set; }

    //[DataMember(Name = "messages")]
    //public List<Message> Messages { get; set; }
}

[DataContract]
public class Message {
    public Message() { }
    [DataMember(Name = "messageId")]
    public string MessageId { get; set; }
    [DataMember(Name = "sendTime")]
    public int sendTime { get; set; }
}

