using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;
using System.IO;
using System.Xml;

namespace AndroidHTTPExample
{
    [Activity(Label = "XmlHTTP")]
    public class XmlHTTP : Activity
    {

        Button btnGetDataSetXmlV1, btnGetDataSetXmlV2, btnGetTimeLayout;
        TextView tvLongitude, tvLatitude;
        TextView tvResponse;
        TextView tvAltitude;
        TextView tvOutputXml;

        HttpWebResponse httpResponse = null;

        // We will work with URL for the entire page.
        string currentURL = @"http://forecast.weather.gov/MapClick.php?lat=39.318&lon=-120.356&unit=0&lg=english&FcstType=dwml";

        // 
        // In this example, I do things much differently than in the SimpleHTTP example. First, 
        // I use the HTTPWebRequest class (belongs to System.Net. This is a .NET class, rather 
        // than a Java (URL) class.)
        //
        // However, the URL Java techniques will work. There are different classes though.
        //  

        // Wire the event handlers as usual.
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.XmlHTTPLayout);

            btnGetDataSetXmlV1 = (Button)FindViewById(Resource.Id.btnGetDatasetXmlV1);
            btnGetDataSetXmlV1.Click += btnGetDataSetXmlV1_Click;

            btnGetDataSetXmlV2 = (Button)FindViewById(Resource.Id.btnGetDatasetXmlV2);
            btnGetDataSetXmlV2.Click += btnGetDataSetXmlV1_Click;

            btnGetTimeLayout = (Button)FindViewById(Resource.Id.btnGetTimeLayout);
            btnGetTimeLayout.Click += btnGetTimeLayout_Click;
            tvLongitude = (TextView)FindViewById(Resource.Id.tvLongitude);
            tvLatitude = (TextView)FindViewById(Resource.Id.tvLatitude);
            tvAltitude = (TextView)FindViewById(Resource.Id.tvAltitude);
            tvResponse = (TextView)FindViewById(Resource.Id.tvResponse);
            tvOutputXml = (TextView)FindViewById(Resource.Id.tvOutputXML);
        }

        // Technique 1 to get XML data.
        protected void btnGetDataSetXmlV1_Click(object sender, EventArgs args)
        {
            try
            {
                // XmlDocument beloongs to the System.XML namespace.
                XmlDocument currentDocument;

                // By rights, we should check that the network is available before making the 
                // following call. It will fail if not connected.

                // The following is a procedure that I wrote to make the requests and load
                // the XML document.
                currentDocument = GetXmlFromURL(currentURL);

                // GetElementsByTagName gets a list of nodes (NodeList). Here there are two <data> 
                // nodes. They are children of <dwml>.
                System.Xml.XmlNodeList nlData = currentDocument.GetElementsByTagName("data");

                // We know from the XML that there are 2 data nodes having attributes named 
                // "forecast", and "current observation".

                foreach (System.Xml.XmlNode nData in nlData)
                {
                    if (nData.Attributes["type"].Value == "forecast")
                    {
                        // <data type="forecast">

                        string s1;
                        // <data><location><point latitude="39.31".
                        s1 = nData.ChildNodes[0].ChildNodes[1].Attributes["latitude"].Value;

                        // <data><location><point longitude="-120.34".
                        string s2;
                        s2 = nData.ChildNodes[0].ChildNodes[1].Attributes["longitude"].Value;

                        // <data><location><height>element content.
                        // Element content (text) is a child of an element node. Just how XML works.
                        string s3;
                        s3 = nData.ChildNodes[0].ChildNodes[3].ChildNodes[0].Value;

                        // Display the values.
                        tvLatitude.Text = "Latitude: " + s1;
                        tvLongitude.Text = "Longitude: " + s2;
                        tvAltitude.Text = "Altitude MSL: " + s3;
                    }

                }
            }

            catch (Exception ex)
            {
                HttpStatusCode i = httpResponse.StatusCode;

            }
        }

        // 
        protected void btnGetDataSetXmlV2_Click(object sender, EventArgs args)
        {
            XmlDocument d1 = new XmlDocument();
            XmlDocument dLocation = new XmlDocument();
            System.Xml.XmlNode nPoint;
            System.Xml.XmlNode nHeight;

            // The following is a procedure that I wrote to make the requests and load
            // the XML document.
            d1 = GetXmlFromURL(currentURL);

            // GetElementsByTagName gets a list of nodes (NodeList). Here there are two <data> 
            // nodes. They are children of <dwml>.
            System.Xml.XmlNodeList nlData = d1.GetElementsByTagName("data");

            // We know from the XML that there are 2 data nodes having attributes named "forecast", and 
            // "current observation".
            foreach (System.Xml.XmlNode nData in nlData)
            {
                if (nData.Attributes["type"].Value == "forecast")
                {
                    // <data><location><point latitude="39.31".
                    // <data><location><point longitude="-120.34".

                    // Here I do something very different than i did in the last example. Here, i create
                    // and XML subdocuemnt containing only the nodes that are children of the current
                    // node. So d1.InnerXml contains all of the children of the current <location> node.
                    // InnerXml is a string, so we parse this string just as we did before. But this time.
                    // the document tree is just a partial document.
                    dLocation.LoadXml(d1.InnerXml);
                    nPoint = dLocation.GetElementsByTagName("point")[0];
                    nHeight = dLocation.GetElementsByTagName("height")[0];

                    tvLatitude.Text = "Latitude: " + nPoint.Attributes["latitude"].Value;
                    tvLongitude.Text = "Longitude: " + nPoint.Attributes["latitude"].Value;

                    // <data><location><height>element content.
                    // Element content (text) is a child of an element node. Just how XML works.
                    tvAltitude.Text = "Altitude MSL: " + nHeight.ChildNodes[0].Value;

                }

            }
        }

        protected void btnGetTimeLayout_Click(object sender, EventArgs args)
        {
            XmlDocument d1 = new XmlDocument();
            XmlDocument dLocation = new XmlDocument();
            System.Xml.XmlNode nPoint;
            System.Xml.XmlNode nHeight;

            d1 = GetXmlFromURL(currentURL);

            // GetElementsByTagName gets a list of nodes (NodeList). Here there are two <data> 
            // nodes. They are children of <dwml>.
            System.Xml.XmlNodeList nlData = d1.GetElementsByTagName("data");

            // We know from the XML that there are 2 data nodes having attributes named "forecast", and 
            // "current observation".
            foreach (System.Xml.XmlNode nData in nlData)
            {
                if (nData.Attributes["type"].Value == "forecast")
                {
                    dLocation.LoadXml(nData.OuterXml);
                    System.Xml.XmlNodeList nlTimeLayout = dLocation.GetElementsByTagName("time-layout");
                    foreach (System.Xml.XmlNode nTimeLayout in nlData)
                    {
                        if (nTimeLayout.ChildNodes[0].ChildNodes[0].Value == "k-p12h-n13-1")
                        {
                            // Get and process the periods. 
                        }
                    }
                }
            }
        }

        protected XmlDocument GetXmlFromURL(string argURL)
        {
            // Create the XmlDocument object.
            XmlDocument d = new XmlDocument();
            try
            {
                // System.Net.HttpWebRequest.
                //
                // We are doing a GET not a POST because that's what the service expects.
                // The mimetype. I don't think this is necessary.
                // Accept = "text/html" tells the request and response that we will accept html.
                // UserAgent is necessary for this web site. We are fooling the server a bit here. 
                // 
                HttpWebRequest request = HttpWebRequest.CreateHttp(argURL);

                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "text/html";
                
                // for the NWS, this is necessary. Otherwise, the server will give us a Forbidden error.
                request.UserAgent = @"Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)";

                // Here we make the request and prepare the response object.
                httpResponse = (HttpWebResponse)request.GetResponse();

                // The server has responded. Get the HTTP status code and display it.
                HttpStatusCode i = httpResponse.StatusCode;
                tvResponse.Text = "HTTP response code: " + i.ToString();

                // Connect the HTTP response stream to our .NET StreamReader; 
                Stream s = httpResponse.GetResponseStream();
                StreamReader sr = new StreamReader(s);

                // And read the entire stream.
                // At this point, the entire XML document is in "resultString".
                string resultString = sr.ReadToEnd();


                // The following loads the XML document into a DOM object. From here, it's just a matter of 
                // undertanding the structure of the XML and extracting the contents. In the code that follows, 
                // I'll spell things out in detail for those of you who are not familiar with XML 
                // parsing or have forgot.
                d.LoadXml(resultString);
            }

            catch (Exception ex)
            {
                HttpStatusCode i = httpResponse.StatusCode;
            }
            // Return the XML document.
            return d;
        }
    }
}
// 240
