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
using System.Json;
using Android.Locations;

namespace AndroidHTTPExample
{
    //
    // This activity shows how to make an HTTPRequest from a RESTful Web service.
    // and parse the resulting JSON code.
    //
    // NOTE: We should h ave implmeneted this as an AsyncTask. However, I have 
    // kept the example simple for brevity.
    [Activity(Label = "JsonHTTP")]
    public class JsonHTTP : Activity
    {
        private TextView tvOutputJson;
        
        // Wire the event handlers as usual.
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.JsonHTTPLayout);

            Button btnGetDatasetsJson = (Button)FindViewById(Resource.Id.btnGetDatasetsJson);
            btnGetDatasetsJson.Click += btnGetDatasetsJson_Click;

            Button btnGetStationsJson = (Button)FindViewById(Resource.Id.btnGetStationsJson);
            btnGetStationsJson.Click += btnGetStationsJson_Click;
            tvOutputJson = (TextView)FindViewById(Resource.Id.tvOutputJson);

            Button btnGetDataTypesJson = (Button)FindViewById(Resource.Id.btnGetDataTypesJson);
            btnGetDataTypesJson.Click += btnGetDataTypesJson_Click;
        }

        // NOAA supports supports a number of different datasets. The list of DataSets
        // is a top-level object.
        protected void btnGetDatasetsJson_Click(object sender, EventArgs args)
        {
            // Belongs to System.Net.
            HttpWebRequest request = 
                WebRequest.CreateHttp("https://www.ncdc.noaa.gov/cdo-web/api/v2/datasets");

            // The following sample request contains additional parameters and selects actual data.
            //HttpWebRequest request = WebRequest.CreateHttp("https://www.ncdc.noaa.gov/cdo-web/api/v2/data?datasetid=GHCND&locationid=ZIP:28801&startdate=2010-05-01&enddate=2010-06-01");

            // The HTTP request will be made using a GET, rather than a POST or other verb.
            request.Method = "GET";
            
            // The following is my token that I obtained from the National Weather Service. 
            // You may use it in your examples but please don't share it.
            // You must use a token or the request will not work and you will get an error.
            // An exception is not thrown. Rather, the error code is returned in the JSON
            // itself.
            request.Headers.Add("token", "vczOaCmkCxBVQHkKGXeVcGoNcrzjdBab");

            // Get the response. This gets the response object but the response has not been 
            // read yet.
            HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();

            // Get the status code from the response. 200 OK, 404 Not Found, etc.
            HttpStatusCode i = httpResponse.StatusCode;

            // Setup the response stream and read the response into a string.
            // This is the data returned by the Web Service. Here, we are using
            // the .NET classes rather than the Java classes.
            Stream s = httpResponse.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            string resultString = sr.ReadToEnd();
            sr.Close();

            // Convert the string to JSON using System.Json.JsonValue. We are not
            // using Newtonsoft here.
            JsonValue value = JsonValue.Parse(resultString);
            tvOutputJson.Text = "";
            
            // Now process the JSON output. The following code is all based on the
            // JSON returned by the query. Use Postman to run the query and examine
            // the results.

            // The following gets the number of records in the returned
            // data.
            // int resultCount = (int)value["metadata"]["resultset"]["count"];
            int resultCount = (int)value["results"]["resultset"]["count"];

            // Iterate the results. and get the id and name values. I put in
            // the extra parentheses for clarity.
            for (int count = 0; count < resultCount; count++)
            {
                tvOutputJson.Text += ((value["results"][count]["id"]).ToString()) + " " +
                    ((value["results"][count]["name"]).ToString()) + "\n";
            }
        }

        // Here we have a similar query to get a list of weather stations.
        // 
        protected void btnGetStationsJson_Click(object sender, EventArgs args)
        {
            HttpWebRequest requestStations = 
                WebRequest.CreateHttp("https://www.ncdc.noaa.gov/cdo-web/api/v2/stations?limit=50");
            requestStations.Method = "GET";
            requestStations.Headers.Add("token", "vczOaCmkCxBVQHkKGXeVcGoNcrzjdBab");

            HttpWebResponse httpResponse = (HttpWebResponse)requestStations.GetResponse();
            HttpStatusCode i = httpResponse.StatusCode;

            Stream s = httpResponse.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            string resultString = sr.ReadToEnd();

            // The HTTP result is a string containing the unparsed JSON.
            // Call JsonValue.Parse to convert the string into a JSON object.
            JsonValue value = JsonValue.Parse(resultString);

            tvOutputJson.Text = "";
            int resultCount = (int)value["results"].Count;
            // foreach (var i1 in value["results"])
                for (int count = 0; count < resultCount; count++)
                {
                tvOutputJson.Text += (value["results"][count]["id"] + " " +
                    value["results"][count]["name"]) + "\n";
            }

            sr.Close();
        }

        // Here we get the data types supported by NWS. 
        protected void btnGetDataTypesJson_Click(object sender, EventArgs args)
        {
            HttpWebRequest requestStations =
                WebRequest.CreateHttp("https://www.ncdc.noaa.gov/cdo-web/api/v2/datatypes?limit=50");
            requestStations.Method = "GET";
            requestStations.Headers.Add("token", "vczOaCmkCxBVQHkKGXeVcGoNcrzjdBab");

            HttpWebResponse httpResponse = (HttpWebResponse)requestStations.GetResponse();
            HttpStatusCode i = httpResponse.StatusCode;

            Stream s = httpResponse.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            string resultString = sr.ReadToEnd();

            JsonValue value = JsonValue.Parse(resultString);
            tvOutputJson.Text = resultString;
        }
    }
}

// 110 120 130 140
//////MAKE BACK AND NEXT BUTTONS FOR THIS QUERY, INCLUDE LONGITUDE AND LATITUDE 
/////MAKE QUERY ON LOCATION QUERIES 