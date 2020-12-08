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

namespace AndroidHTTPExample
{
    //
    // Display network status information.
    //
    [Activity(Label = "NetworkInfoLocal")]
    public class NetworkInfoLocal : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.NetworkInfoLayout);
            CheckBox chkIsAvailable = (CheckBox)FindViewById(Resource.Id.chkIsAvailable);
            CheckBox chkIsConnected = (CheckBox)FindViewById(Resource.Id.chkIsConnected);
            CheckBox chkIsRoaming = (CheckBox)FindViewById(Resource.Id.chkIsRoaming);

            TextView tvNetworkState =
                (TextView)FindViewById(Resource.Id.tvNetworkState);
            TextView tvDetailedNetworkState =
                (TextView)FindViewById(Resource.Id.tvDetailedNetworkState);
            TextView tvConnectivityType =
                (TextView)FindViewById(Resource.Id.tvConnectivityType);
            TextView tvConnectivityTypeName =
                (TextView)FindViewById(Resource.Id.tvConnectivityTypeName);
            TextView tvBytesSent = (TextView)FindViewById(Resource.Id.tvBytesSent);
            TextView tvBytesReceived = (TextView)FindViewById(Resource.Id.tvBytesReceived);

            //
            // The ConnectivityManager answers questions about the state of network
            // connectivity or and raise events when the connectivity state
            // changes.
            //
            // The ConnectivityManager is the connection between the application 
            // and the actual service (ConnectivityService).
            Android.Net.ConnectivityManager connMgr;
            connMgr = (ConnectivityManager)
                Android.App.Application.Context.
                GetSystemService(Context.ConnectivityService);

            // Get information about the network via the NetworkInfo class.
            // The network information is obtained from the ConnectivityManager.
            NetworkInfo networkInfo = connMgr.ActiveNetworkInfo;

            // Get the course and detailed network state and display 
            // the resulting values. Both of these data types are just
            // enumerations.
            NetworkInfo.State networkInfoState = networkInfo.GetState();
            NetworkInfo.DetailedState networkInfoDetailedState =
                networkInfo.GetDetailedState();

            // And display the network information.
            tvNetworkState.Text = networkInfoState.ToString();
            tvDetailedNetworkState.Text = networkInfoDetailedState.ToString();

            // Get the various network flag values.
            chkIsAvailable.Checked = networkInfo.IsAvailable;
            chkIsConnected.Checked = networkInfo.IsConnected;
            chkIsRoaming.Checked = networkInfo.IsRoaming;

            // Figure out the connection type.
            ConnectivityType ct = networkInfo.Subtype;
            tvConnectivityType.Text = ct.ToString();

            String connectivityTypeName = networkInfo.SubtypeName;
            tvConnectivityTypeName.Text = connectivityTypeName;

            // The TrafficStats class gets network statistics since
            // the device was last booted.
            long bytesSent = Android.Net.TrafficStats.MobileTxBytes;
            long bytesReceived = Android.Net.TrafficStats.MobileRxBytes;

            tvBytesSent.Text = bytesSent.ToString();
            tvBytesReceived.Text = bytesReceived.ToString();

        }

    }
}
// 95