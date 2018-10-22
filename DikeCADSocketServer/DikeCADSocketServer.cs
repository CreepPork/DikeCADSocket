using System;
using System.Threading;
using CitizenFX.Core;
using RestSharp;

namespace DikeCADSocketServer
{
    public class DikeCADSocketServer : BaseScript
    {
        public DikeCADSocketServer()
        {
            EventHandlers.Add("dike:receiveData", new Action<string>(OnReceiveData));
        }

        private static void OnReceiveData(string json)
        {
            Debug.WriteLine(json);

            new Thread(() =>
            {
                SendPostRequest(json);
            }).Start();
        }

        private static void SendPostRequest(string json)
        {
            RestClient client = new RestClient("http://dikecad.garkaklis.com");
            
            RestRequest request = new RestRequest("map", Method.POST);

            request.AddParameter("text/json", json, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            
            if (! response.IsSuccessful)
                Debug.WriteLine($"Failed to send data! {json}");
        }
    }
}