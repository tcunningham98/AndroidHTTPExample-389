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

using Android.Net;
using Android.Net.Http;

using Java.Net;
using Java.IO;

namespace AndroidHTTPExample
{
    //
    // Make a Simple HTTP request using the URLConnection object.
    // We do not process the HTML returned. Just display it in a TextView.
    // This class also demonstrates a first example of the AsyncTask. 
    // Android requires that HTTP requests be processed in a background process.
    //
    [Activity(Label = "SimpleHTTP")]
    public class SimpleHTTP : Activity
    {

        private Button btnMakeRequestJava;
        private Button btnMakeRequestNet;
        private TextView tvOutput;

        // Wire the buttons and get references to the global widgets as usual.
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SimpleHTTPLayout);

            btnMakeRequestJava = (Button)FindViewById(Resource.Id.btnMakeRequestJava);
            btnMakeRequestJava.Click += btnMakeRequestJava_Click;

            btnMakeRequestNet = (Button)FindViewById(Resource.Id.btnMakeRequestNet);
            btnMakeRequestNet.Click += btnMakeRequestNet_Click;

            tvOutput = (TextView)FindViewById(Resource.Id.tvOutput);
        }

        //
        //
        // Make a request using the Java IO libraries.
        //
        // The URL class here belongs to Java.Net.URL so it's a native Java class.
        // Here. We are using an Async method to do the work.
        //
        protected void btnMakeRequestJava_Click(object sender, EventArgs args)
        {
            // Create the URL
            URL url = new URL("http://students.coba.unr.edu/ekedahl/foo/index.html");

            // Create an instance of the AsynTask class (declared later in this module)
            // We pass an instance of the current activity to the constructor.
            DownloadWebpageTaskJava d;
            d = new DownloadWebpageTaskJava(this);

            // Begin execution of the AsyncTask.
            d.Execute(url); ;
        }

        // Make a request using the .NET IO Libraries. We pass the URL to
        // the AsyncWorker just like before. However, the worker uses the
        // .net classes instead of the Java classes.
        //
        protected void btnMakeRequestNet_Click(object sender, EventArgs args)
        {
            URL url = new URL("http://students.coba.unr.edu/ekedahl/foo/index.html");
            DownloadWebpageTaskNet d;
            d = new DownloadWebpageTaskNet(this);
            d.Execute(url); 
            
            // The following will work too because we don't need the variable
            // reference. The following syntax shows an example of a chained method
            // new DownloadWebpageTaskNet(this).Execute(url);
        }

        // Class to implement the AsyncTask for the Java implementation.
        //
        // AsyncTask takes 3 arguments.
        //
        // Argument 1: the data passed to the worker (RunInBackground)
        // Argument 2: the data passed back to the caller indicating progress.
        // Argument 3: the data passed back to the caller containing the result
        //              of the async request.
        public class DownloadWebpageTaskJava : AsyncTask<URL, string, string>
        {
            // Store a reference to the UI activity that created this object.
            public SimpleHTTP activity;

            // The activity is passed to the constructor. This gives us the 
            // ability to reference the activity and it's widgets in the OnPostExecute()
            // method. OnPostExecute() runs on the UI thread, rather than the worker thread. 
            public DownloadWebpageTaskJava(SimpleHTTP act)
            {
                this.activity = act;
            }


            //
            // RunInBackground is performed in the background task (when the Execute method is 
            // called). The URL is passed
            // to RunInBackground when the Execute method is called from the parent
            // activity. This is the Java.Net.URL class, rather than the .NET class. 
            //
            protected override string RunInBackground(URL[] urls)
            {
                // Java reader.
                InputStreamReader reader = null;
                
                // To store the string returned by the HTTP request. 
                string resultHTML = "";

                // Get the network information as before.
                ConnectivityManager connMgr;
                connMgr = (ConnectivityManager)
                    Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
                NetworkInfo networkInfo = connMgr.ActiveNetworkInfo;

                // Create the URL connection and set it's properties.
                URL u = urls[0];
                HttpURLConnection urlConnection = (HttpURLConnection) u.OpenConnection();

                urlConnection.RequestMethod = "GET";
                urlConnection.ReadTimeout = 10000; /* milliseconds */
                urlConnection.ConnectTimeout = 15000; /* milliseconds */
                urlConnection.DoInput = true; /* Allow reading of the response HTML */

                // Make sure we are connected to the network.
                if (networkInfo.IsConnected)
                {
                    try
                    {
                        // Connect to the server.
                        urlConnection.Connect();

                        // Set up the reader that will read from the connection's input
                        // stream. This class is part of the Java.IO class rather than the equivalent 
                        // .NET class.
                        reader =
                            new Java.IO.InputStreamReader(urlConnection.InputStream, "UTF-8");

                        // Read the data (HTML) and append it to the String (resultHTML). End of file
                        // is reached when the read method returns -1. Otherwise, it returns a 
                        // character.
                        int i;

                        // Read characters until there are no more.
                        while ((i = reader.Read()) != -1)
                        {
                            char c = (char)i;
                            // And append each character to the result string.
                            resultHTML += (c);

                        }
                        // Get the response code even though we don't use it. These are the 
                        // HTTP 200, 300, 400 response codes returned by all HTTP requests.
                        HttpStatus httpStatusResult = urlConnection.ResponseCode;

                        return resultHTML.ToString();
                    }
                    catch (System.Exception ex)
                    {
                        return ex.Message;
                    }
                    finally
                    {
                        // Close the reader.
                        reader.Close();
                    }
                }
                else
                {
                    return "There is no network connection.";
                }
            }

            //
            // The OnPostExecute method runs on the UI thread. So we can display 
            // the results in a UI control of the activity. This is the same
            // activity that we stored when the constructor was called.
            // 
            // Note that we have a reference to the current activity.
            protected override void OnPostExecute(string result)
            {
                // The argument result contains the result of the HTTP request - 
                // returned by the RunInBackground method.
                base.OnPostExecute(result);

                // Display the HTML in the TextView. 
                TextView tvOutput = (TextView)activity.FindViewById(Resource.Id.tvOutput);
                tvOutput.Text = result;
            }
        }

        //
        // Here we do the same thing but this time, we use the .NET IO classes instead of 
        // the Java classes. The end result is the same. All of the code is the same except for the 
        // specific .NET IO code.
        //
        public class DownloadWebpageTaskNet : AsyncTask<URL, string, string>
        {
            public SimpleHTTP activity;

            // Same as before. Pass the activity to the constructor and save it as before.
            public DownloadWebpageTaskNet(SimpleHTTP act)
            {
                this.activity = act;
            }

            // RunInBackground() works the same way too. It's called when the background thread 
            // is executed.
            protected override string RunInBackground(URL[] urls)
            {
                ConnectivityManager connMgr;
                connMgr = (ConnectivityManager)
                    Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
                NetworkInfo networkInfo = connMgr.ActiveNetworkInfo;

                URL u = urls[0];
                HttpURLConnection urlConnection = (HttpURLConnection) u.OpenConnection();

                System.IO.StreamReader srResults = null;

                if (networkInfo.IsConnected)
                {
                    try
                    {
                        urlConnection.Connect();
                        //
                        // TODO SHOW THE REPSONSE CODE.
                        //urlConnection.ResponseCode;

                        // Here is where the response reader differs a bit. Here I'm using the .NET 
                        // StreamReader instead of the Java.IO.InputStreamReader.
                        srResults = new System.IO.StreamReader(urlConnection.InputStream);
                        // The .NET StreamReader has a method to read the entire stream so we don't have 
                        // to read the stream one character at a time.
                        string results = srResults.ReadToEnd();
                        return results;
                    }
                    catch (System.Exception ex)
                    {
                        return ex.Message;
                    }

                    finally
                    {
                        srResults.Close();
                    }
                }
                else
                {
                    return "Not connected to the network.";
                }
                  
            }

            // OnPostExecute is the same for both the Java and .NET versions. The result 
            // of the HTTP request is passed to the method. Display it to the TextView.
            protected override void OnPostExecute(string result)
            {
                base.OnPostExecute(result);

                TextView tvOutput = (TextView) activity.FindViewById(Resource.Id.tvOutput);
                tvOutput.Text = result;
            }

        }
    }
}
// 240 250 260