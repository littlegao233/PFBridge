using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sora.OnebotModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFBridgeForOneBot
{
    public class TimeSpanConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;//objectType == typeof(TimeSpan);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            long r = (long)reader.Value;
            if (long.TryParse(reader.Value.ToString(), out long o)) if (o == 10) r = o * 1000;
            return TimeSpan.FromMilliseconds(r);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is TimeSpan span)
            {
                writer.WriteValue(Convert.ToInt64(span.TotalMilliseconds));
            }
        }
    }
    class ServerConfigModel
    {
        public string Host
        {
            get => host;
            set
            {
                if (value != "127.0.0.1") host = value;
            }
        }
        private string host = "127.0.0.1";
        public string Location
        {
            set
            {
                Host = value;
            }
        }
        public uint Port = 8080;
        public string AccessToken = string.Empty;
        public string UniversalPath = string.Empty;
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan HeartBeatTimeOut = TimeSpan.FromSeconds(10);
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ApiTimeOut = TimeSpan.FromMilliseconds(1000);
        public bool EnableSoraCommandManager = true;
        public ServerConfig GetServerConfig()
        {
            return new ServerConfig()
            {
                EnableSoraCommandManager = EnableSoraCommandManager,
                AccessToken = AccessToken,
                ApiTimeOut = HeartBeatTimeOut,
                Port = Port,
                UniversalPath = UniversalPath,
                Host = Host,
                HeartBeatTimeOut = HeartBeatTimeOut
            };
        }
    }
}
