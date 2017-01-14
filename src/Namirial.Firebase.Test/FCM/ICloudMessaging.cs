using Namirial.Firebase.Test;
using Namirial.Firebase.Test.FCM.Firebase;

namespace Namirial.Firebase.Test.FCM {
    public interface ICloudMessaging {
        string SendMessageToTopic(string topic, FirebaseMessage message);
        string SendMessageToDevice(string device, FirebaseMessage message);
    }
}
