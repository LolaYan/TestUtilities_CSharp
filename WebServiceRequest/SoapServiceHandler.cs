using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestUtilities.WebServiceRequest
{
    public class SoapServiceHandler
    {
        public static string SoapWebServiceUrl;
        public static string SoapRequestAction;
        public static string SoapRequestMethod;
        public static string SoapRequestHeader;
        public static string SoapRequestBody;
        public static string SoapResponseHeader;
        public static string SoapResponseStatus;
        public static string SoapResponseBody;


        public static void Login()
        {
            string requestFileName = "../../WebServiceRequest/SoapRequestData/LoginRequestBody.xml";
            SoapWebServiceUrl = "http://192.168.100.70/nzl-ws/services/PlayerService/v1";
            SoapRequestAction = "loginCmd";
            SoapRequestMethod = "POST";
            CallWebService(SoapWebServiceUrl, SoapRequestAction, SoapRequestMethod, requestFileName);
            printInfo();
            parseLoginResponseXml(SoapResponseBody);


        }
        public static void CallWebService(string _url, string _action, string method, string requestFileName)
        {
            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(requestFileName);
            HttpWebRequest webRequest = CreateWebRequest(_url, _action, method);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                HttpWebResponse response = (HttpWebResponse)webResponse;
                SoapResponseStatus = response.StatusCode.ToString();
                SoapResponseHeader = webResponse.Headers.ToString();

                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                SoapResponseBody = soapResult;
            }
        }

        private static HttpWebRequest CreateWebRequest(string url, string action, string method)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = method;
            SoapRequestHeader = webRequest.Headers.ToString();
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope(string requestFileName)
        {
            XmlDocument soapEnvelop = new XmlDocument();
            //soapEnvelop.LoadXml(@"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/1999/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/1999/XMLSchema""><SOAP-ENV:Body><HelloWorld xmlns=""http://tempuri.org/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/""><int1 xsi:type=""xsd:integer"">12</int1><int2 xsi:type=""xsd:integer"">32</int2></HelloWorld></SOAP-ENV:Body></SOAP-ENV:Envelope>");
            soapEnvelop.Load(requestFileName);
            SoapRequestBody = ConvertXmltoString(soapEnvelop);
            return soapEnvelop;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        public static void parseLoginResponseXml(string xmlText)
        {
            XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object
            xmlDoc.LoadXml(xmlText); // Load the XML document from the specified file

            // Get elements
            XmlNodeList userId = xmlDoc.GetElementsByTagName("userId");
            XmlNodeList sessionId = xmlDoc.GetElementsByTagName("sessionId");

            // Display the results
            Console.WriteLine("userId: " + userId[0].InnerText);
            Console.WriteLine("sessionId: " + sessionId[0].InnerText);
        }

        public static void printInfo()
        {
            Console.WriteLine("Request Info .............................");
            Console.WriteLine(SoapRequestMethod + " " + SoapWebServiceUrl);
            Console.WriteLine(SoapRequestHeader);
            Console.WriteLine(SoapRequestBody);
            Console.WriteLine("Response Info .............................");
            Console.WriteLine(SoapResponseStatus);
            Console.WriteLine(SoapResponseHeader);
            Console.WriteLine(SoapResponseBody);
        }

        public static string ConvertXmltoString(XmlDocument xmlDoc)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter tx = new XmlTextWriter(sw))
                {
                    xmlDoc.WriteTo(tx);
                    string strXmlText = sw.ToString();
                    return strXmlText;
                }
            }
        }
    }
}
