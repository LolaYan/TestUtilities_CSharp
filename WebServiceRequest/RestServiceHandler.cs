using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp.Authenticators;
using RestSharp.Deserializers;
using RestSharp.Extensions;
using RestSharp;
using System.IO;
using System.Diagnostics;

namespace TestUtilities.WebServiceRequest
{
    class RestServiceHandler
    {
        public static void DPSCall()
        {
            var client = new RestClient("https://sec.paymentexpress.com/pxmi3/pxfusionauth");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "d82225a5-413c-bd3c-9caf-18bf89ed0411");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "multipart/form-data; boundary=---011000010111000001101001");
            request.AddParameter("multipart/form-data; boundary=---011000010111000001101001", "-----011000010111000001101001\r\nContent-Disposition: form-data; name=\"CardNumber\"\r\n\r\n4111111111111111\r\n-----011000010111000001101001\r\nContent-Disposition: form-data; name=\"ExpiryMonth\"\r\n\r\n12\r\n-----011000010111000001101001\r\nContent-Disposition: form-data; name=\"ExpiryYear\"\r\n\r\n22\r\n-----011000010111000001101001\r\nContent-Disposition: form-data; name=\"SessionId\"\r\n\r\n000003006059808500ae8a5cfd195bcb\r\n-----011000010111000001101001\r\nContent-Disposition: form-data; name=\"Cvc2\"\r\n\r\n123\r\n-----011000010111000001101001\r\nContent-Disposition: form-data; name=\"CardHolderName\"\r\n\r\ntest\r\n-----011000010111000001101001--", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Debug.Print("StatusCode: " + response.StatusCode);
            Debug.Print("RawBytes: " + response.Content);
            Debug.Print("ResponseUri: " + response.ResponseUri);


        }
    }
}
