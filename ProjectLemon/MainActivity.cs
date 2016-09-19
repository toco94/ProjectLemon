using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Android.OS;
using Android.Locations;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ProjectLemon
{
    [Activity(Label = "ProjectLemon", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IOnMapReadyCallback, ILocationListener
    {
        //int count = 1;
        string key = "AIzaSyCq7XqwYUeGOVLqs4FzvjDrYYRGLEar3-A";
        private GoogleMap map;
        Button btnNormal;
        Button btnHybrid;
        Button btnSatellite;
        Button btnTerrain;
        MarkerOptions options;
        LatLng latlngCetys = new LatLng(32.50660123141241, -116.92439664155245);
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            btnNormal = FindViewById<Button>(Resource.Id.btnNormal);
            btnHybrid = FindViewById<Button>(Resource.Id.btnHybrid);
            btnSatellite = FindViewById<Button>(Resource.Id.btnSatellite);
            btnTerrain = FindViewById<Button>(Resource.Id.btnTerrain);
            btnNormal.Click += BtnNormal_Click;
            btnHybrid.Click += BtnHybrid_Click;
            btnSatellite.Click += BtnSatellite_Click;
            btnTerrain.Click += BtnTerrain_Click;
            setUpMap();
            //map.MyLocationEnabled = true;
        }

        private void BtnTerrain_Click(object sender, EventArgs e)
        {
            string url = "https://maps.googleapis.com/maps/api/directions/json?origin="
                + options.Position.Latitude.ToString() + "," + options.Position.Longitude.ToString() +
                "&destination=" + latlngCetys.Latitude.ToString() + "," + latlngCetys.Longitude.ToString() +
                "&key=" + key;
            //"https://maps.googleapis.com/maps/api/directions/json?origin=32.477443576,-117.025249343&destination=32.5066012314124,-116.924396641552&key=AIzaSyCq7XqwYUeGOVLqs4FzvjDrYYRGLEar3-A"
            //string url = "https://maps.googleapis.com/maps/api/directions/json?origin=Chicago,IL&destination=Los+Angeles,CA&waypoints=Joplin,MO|Oklahoma+City,OK&key=AIzaSyCq7XqwYUeGOVLqs4FzvjDrYYRGLEar3-A";
            var client = new WebClient();
            var content = client.DownloadString(url);
            dynamic route = JObject.Parse(content);
            List<Location> locList = DecodePolylinePoints("affeEjogiU]fAc@rAgBlFq@lBEV?LJf@`@hBlAzG}@n@OPa@FuANb@jK?NkAH{Gf@YtEUdCOZYRm@Z_@f@Qf@Y|BEb@bEr@lF`ATAVB~AXjBr@`@RHQ`CcEt@iAl@i@j@w@rBqDjDyF~DaGpBsCbAaBvCqEnEyG~A}B|BoDzBkDfA}BtBgFt@iBd@qA^uAPeAXoDd@cGLkCCo@O}@iAsDSk@y@eCmAkDUy@U}A]wD_@_FIw@a@wBMe@[_Am@aAk@{@iAgAkAw@m@YqDeAuAe@q@[iBwAu@_Ac@u@a@{@k@{AWcASaAUiBO{B@kCLqBN{AVkA|@}ChBmGx@uCTwA@e@?w@Eq@Kw@Om@Uk@Sc@gA_BqFqI}@eBuAyBk@}@wAoBgHiKiIwL_A{A_AmBmAgC_CyFaAiBy@kAm@k@aBqAgC_BoE{CeCiBiB{AyBkB{KwJwBkBs@w@_AsAm@cAgAwD_AkDeB_Gw@aD[oA]uB]sAYgBg@uCc@gBm@{A]m@q@u@g@_@oEuDeDyC{CsCqFcEs@w@}@q@mFeDmB{Aa@YsA_A}EwDiNgK{AoAqHqFgBiAsF{DwAo@{@QkAIo@Aq@B{Dt@{Ez@uEt@uCd@cCh@y@Pm@iAmAyBi@o@MSI]EYMQBs@z@kXXoIHeANmBTyA\\iAb@}@lAaB~@eAhB{BfA}A`@y@t@yBXaBZaCtA{NRqCJwBBgBCiBGkAIu@k@_CuEuOiGkQgA}Du@qDo@yDqA{LCiANwCFeB?gAGwAMuADWAYGq@Mk@eBqFUw@Km@AU?OFYDIHIx@a@\\MzB{@nC{@vDsA`EyA`Dw@vHkArB[bC]^Fv@?vCSPC?\\NlBFp@AHg@d@KP@ZJv@");
            PolylineOptions pOptions = new PolylineOptions();
            foreach (Location l in locList)
            {
                pOptions.Add(new LatLng(l.Latitude, l.Longitude));
            }
            pOptions.InvokeColor(-65536);
            Polyline poly = map.AddPolyline(pOptions);
        }

        private void BtnSatellite_Click(object sender, EventArgs e)
        {
            map.MapType = GoogleMap.MapTypeSatellite;
        }

        private void BtnHybrid_Click(object sender, EventArgs e)
        {
            map.MapType = GoogleMap.MapTypeHybrid;
        }

        private void BtnNormal_Click(object sender, EventArgs e)
        {
            map.MapType = GoogleMap.MapTypeNormal;
        }

        void setUpMap()
        {
            if(map == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
            }

        }

        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            Location location;
            LocationManager locManager = (LocationManager)GetSystemService(Context.LocationService);
            locManager.RequestLocationUpdates(LocationManager.GpsProvider, 0, 0, this);
            location = locManager.GetLastKnownLocation(LocationManager.GpsProvider);
            LatLng myLatLng = new LatLng(location.Latitude, location.Longitude);
            CameraUpdate cam = CameraUpdateFactory.NewLatLngZoom(myLatLng, 17);
            map.MoveCamera(cam);
            options = new MarkerOptions().SetPosition(myLatLng).SetTitle("Das Marker").SetSnippet("das snippet").Draggable(true);
            map.AddMarker(options);
            map.MarkerDragEnd += Map_MarkerDragEnd;
            //map.TrafficEnabled = true;
        }

        private void Map_MarkerDragEnd(object sender, GoogleMap.MarkerDragEndEventArgs e)
        {
            LatLng pos = e.Marker.Position;
            Console.WriteLine(pos.ToString());
        }

        public void OnLocationChanged(Location location)
        {
            //options.SetPosition(new LatLng(location.Latitude, location.Longitude));
            //map.AddMarker(options);
            Console.WriteLine(location.HasSpeed.ToString());
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }
        private List<Location> DecodePolylinePoints(string encodedPoints)
        {
            if (encodedPoints == null || encodedPoints == "") return null;
            List<Location> poly = new List<Location>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            try
            {
                while (index < polylinechars.Length)
                {
                    // calculate next latitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length)
                        break;

                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    //calculate next longitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length && next5bits >= 32)
                        break;

                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                    Location p = new Location("");
                    p.Latitude = Convert.ToDouble(currentLat) / 100000.0;
                    p.Longitude = Convert.ToDouble(currentLng) / 100000.0;
                    poly.Add(p);
                }
            }
            catch (Exception ex)
            {
                // logo it
            }
            return poly;
        }
    }

}

