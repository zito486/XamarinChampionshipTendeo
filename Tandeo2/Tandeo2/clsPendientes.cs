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
using System.Threading.Tasks;
using Tandeo2.Services;

namespace Tandeo2
{
    [Activity(Label = "Tandas/Pagos Pendientes")]
    public class clsPendientes : ListActivity
    {
        string[] items;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.Pendientes);
            // Create your application here

            ServiceHelper shelper = new ServiceHelper();
            await shelper.BuscarRegistros();
            items = shelper._PublicTandaItems;
            ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
            
        }


        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var t = items[position];
            var intent = new Intent(this, typeof(clsDisminuir));
            intent.PutExtra("TandaId", t.ToString());
            StartActivityForResult(intent, 1);
            
        }

    }
}