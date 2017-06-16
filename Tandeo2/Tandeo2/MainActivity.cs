using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;

namespace Tandeo2
{
[Activity(Label = "Tandeo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button btnPendientes, btnAgregar;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);

            //R2-Vistas y navegacion
            btnPendientes = FindViewById<Button>(Resource.Id.btnConsultar);
            btnPendientes.Click += btnPendientesClick;

            btnAgregar = FindViewById<Button>(Resource.Id.btnNuevo);
            btnAgregar.Click += btnNuevoClick;

            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
        }

        private void btnPendientesClick(object sender, EventArgs e)
        {
            //R3- Conectividad
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                var intent = new Intent(this, typeof(clsPendientes));

                StartActivityForResult(intent, 1);
            }
            else
            {
                Toast.MakeText(this, "Actualmente no estás conectado a Internet", ToastLength.Long).Show();
            }
        }

        private void btnNuevoClick(object sender, EventArgs e)
        {
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                var intent = new Intent(this, typeof(clsRegistrar));

                StartActivityForResult(intent, 2);
            }
            else
            {
                Toast.MakeText(this, "Actualmente no estás conectado a Internet", ToastLength.Long).Show();
            }
        }

        private void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                Toast.MakeText(this, "Conectado a Internet", ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this, "No hay una conexión disponible", ToastLength.Long).Show();
            }
        }

    }
}

