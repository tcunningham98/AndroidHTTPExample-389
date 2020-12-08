using Android.App;
using Android.Widget;
using Android.OS;
using Android.Net;
using Android.Content;

using Java.Net;
using Java.IO;
using System.Text;
using System;
using Java.Lang;

namespace AndroidHTTPExample
{

    //
    // Driver for all of the applicaiton's activities.
    // 
    [Activity(Label = "AndroidHTTPExample", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);


            // Network information.
            Button btnShowNetworkInfo =
                (Button)FindViewById(Resource.Id.btnShowNetworkInfo);
            btnShowNetworkInfo.Click += btnShowNetworkInfo_Click;

            Button btnShowAsyncDemo =
                (Button)FindViewById(Resource.Id.btnShowAsyncDemo);
            btnShowAsyncDemo.Click += btnShowAsyncDemo_Click;

            // A simple WebReqeust
            Button btnSimpleHTTPRequest = 
                (Button)FindViewById(Resource.Id.btnSimpleHTTPRequest);
            btnSimpleHTTPRequest.Click += btnSimpleHTTPRequest_Click;

            // An HTTP request to a restful site using Json.
            Button btnJsonHTTPRequest =
                (Button)FindViewById(Resource.Id.btnJsonHTTPRequest);
            btnJsonHTTPRequest.Click += btnJsonHTTPRequest_Click;

            // An HTTP request to a somewhat restful site using XML.
            Button btnXmlHTTPRequest =
                (Button)FindViewById(Resource.Id.btnXmlHTTPRequest);
            btnXmlHTTPRequest.Click += btnXmlHTTPRequest_Click;
        }

        protected void btnShowNetworkInfo_Click(object sender, EventArgs args)
        {
            Intent networkInfo = new Intent(this, typeof(NetworkInfoLocal));
            StartActivity(networkInfo);
        }

        protected void btnShowAsyncDemo_Click(object sender, EventArgs args)
        {
            Intent asyncActivity = new Intent(this, typeof(AsyncActivity));
            StartActivity(asyncActivity);
        }

        protected void btnSimpleHTTPRequest_Click(object sender, EventArgs args)
        {
            Intent simpleHTTPRequest = new Intent(this, typeof(SimpleHTTP));
            StartActivity(simpleHTTPRequest);
        }

        protected void btnJsonHTTPRequest_Click(object sender, EventArgs args)
        {
            Intent jsonHTTPRequest = new Intent(this, typeof(JsonHTTP));
            StartActivity(jsonHTTPRequest);
        }

        protected void btnXmlHTTPRequest_Click(object sender, EventArgs args)
        {
            Intent xmlHTTPRequest = new Intent(this, typeof(XmlHTTP));
            StartActivity(xmlHTTPRequest);
        }
    }
}
// 60 70 80 