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
using Java.Lang;

namespace AndroidHTTPExample
{
    [Activity(Label = "AsyncActivity")]
    public class AsyncActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AsyncLayout);

            Button btnOutputAsync = (Button)FindViewById(Resource.Id.btnRunAsync);
            btnOutputAsync.Click += btnOutputAsync_Click;

        }

        protected void btnOutputAsync_Click(object sender, EventArgs e)
        {
            DemoAsyncTask d;
            d = new DemoAsyncTask(this);
            d.Execute(100000000);

        }
    }

    public class DemoAsyncTask : AsyncTask<int, int, long>
    {
        public AsyncActivity activity;

        public DemoAsyncTask(AsyncActivity act)
        {
            this.activity = act;
        }

        protected override long RunInBackground(params int[] @params)
        {
            int i = (int) this.Handle;
            int iterations = @params[0];
            int percent = iterations / 10;
            int percentOut = 10;
            long total = 0 ;
            for (int x = 0; x < iterations; x++)
            {
                if (x % percent == 0)
                {
                    PublishProgress(new int[] { percentOut, i });
                    percentOut += 10;
                }
                total += x;
            }
            return total;
        }

        protected override void OnProgressUpdate(params int[] values)
        {
            base.OnProgressUpdate(values);
            TextView tvOutputAsync = (TextView)activity.FindViewById(
                Resource.Id.tvOutputAsync);
            tvOutputAsync.Text += "(" + values[1].ToString() + ") " + 
                values[0].ToString() + " percent " + "\n";
        }

        protected override void OnPostExecute(long result)
        {
            base.OnPostExecute(result);

            // Display the HTML in the TextView. 
            TextView tvOutputAsync = (TextView)activity.FindViewById(
                Resource.Id.tvOutputAsync);
            tvOutputAsync.Text += result.ToString() ;
        }
    }
}

// 50 60 70 80 