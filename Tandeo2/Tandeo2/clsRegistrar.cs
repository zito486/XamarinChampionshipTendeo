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
using Tandeo2.Services;

namespace Tandeo2
{
    [Activity(Label = "clsRegistrar")]
    public class clsRegistrar : Activity
    {
        Button btnGuardar;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Registrar);
            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
            btnGuardar = FindViewById<Button>(Resource.Id.btnGuardar);
            btnGuardar.Click += btnGuardarClick;
        }

        private async void btnGuardarClick(object sender, EventArgs e)
        {
            ServiceHelper shelper = new ServiceHelper();
            EditText txtNombre = FindViewById<EditText>(Resource.Id.txtNombre);
            EditText txtCantidad = FindViewById<EditText>(Resource.Id.txtCantidad);
            await shelper.InsertarEntidad(txtNombre.Text, Convert.ToDecimal(txtCantidad.Text));
            Android.Widget.Toast.MakeText(this, "Informacion actualizada", Android.Widget.ToastLength.Short).Show();
            notificate("Registro almacenado", "Se guardo correctamente el registro");   //RN - Notificacion local
        }

        public void notificate(string title,string message)
        {
            // Instantiate the builder and set notification elements:
            Notification.Builder builder = new Notification.Builder(this)
                .SetContentTitle(title)
                .SetContentText(message);

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }
    }
}