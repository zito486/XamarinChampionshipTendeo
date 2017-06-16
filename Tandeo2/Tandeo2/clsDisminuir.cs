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
    [Activity(Label = "Disminuir Saldos")]
    public class clsDisminuir : Activity
    {
        Button btnRestar;
        TextView txtInformacion, txtCantidad;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Disminuir);

            String value = Intent.GetStringExtra("TandaId");
            txtInformacion = FindViewById<TextView>(Resource.Id.txtInformacion);
            txtInformacion.Text = value;

            btnRestar = FindViewById<Button>(Resource.Id.btnRestar);
            btnRestar.Click += btnRestarClick;
        }

        private async void btnRestarClick(object sender, EventArgs e)
        {
            ServiceHelper shelper = new ServiceHelper();
            txtInformacion = FindViewById<TextView>(Resource.Id.txtInformacion);
            txtCantidad = FindViewById<TextView>(Resource.Id.txtResta);
            var id = txtInformacion.Text.Split(' ')[0];

            await shelper.RestarCantidad(id, Convert.ToDecimal(txtCantidad.Text));

            Android.Widget.Toast.MakeText(this, "Informacion actualizada", Android.Widget.ToastLength.Short).Show();
        }
    }
}
