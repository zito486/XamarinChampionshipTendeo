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
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Tandeo2.Models;
using System.IO;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;

namespace Tandeo2.Services
{
    public class ServiceHelper
    {
        MobileServiceClient clienteServicio = new MobileServiceClient(@"http://tandeo.azurewebsites.net");
        private MobileServiceClient client;
        private IMobileServiceTable<TandaItem> _TandaItemTable;
        private IMobileServiceTable<TandaItem> _TandaItemTableLocal;
        public string[] _PublicTandaItems;
        public int maximo;
        const string applicationURL = @"http://tandeo.azurewebsites.net/";
        const string localDbFilename = "localstore.db";

        public async Task BuscarRegistros()
        {
            //R4-Consusltar base de datos
            _TandaItemTable = clienteServicio.GetTable<TandaItem>();
            System.Collections.Generic.List<TandaItem> items = await _TandaItemTable.ToListAsync();

            _PublicTandaItems = new string[items.Count()];
            var i = 0;
            foreach (var item in items)
            {
                _PublicTandaItems[i] = item.id + " - " + item.nombre;
                i++;
            }
            client = new MobileServiceClient(applicationURL);
            await InitLocalStoreAsync();
        }

        public async Task SiguienteId()
        {
            _TandaItemTable = clienteServicio.GetTable<TandaItem>();
            System.Collections.Generic.List<TandaItem> items = await _TandaItemTable.ToListAsync();
            if (items.Any())
            {
                maximo = Convert.ToInt32(items.Max(x => x.id));
            }
            else
            {
                maximo = 0;
            }
            
            
        }

        public async Task InsertarEntidad(string _nombre, decimal _deuda)
        {
            try
            {
                _TandaItemTable = clienteServicio.GetTable<TandaItem>();
                await this.SiguienteId();

                await _TandaItemTable.InsertAsync(new TandaItem
                {
                    id = (maximo + 1).ToString(),
                    nombre = _nombre,
                    deuda = _deuda
                });

            }
            catch (Exception ex)
            {
                
            }
        }

        public async Task RestarCantidad(string _id, decimal cantidad)
        {
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                _TandaItemTable = clienteServicio.GetTable<TandaItem>();
                client = new MobileServiceClient(applicationURL);
                _TandaItemTableLocal = client.GetTable<TandaItem>();
                System.Collections.Generic.List<TandaItem> itemsLocal = await _TandaItemTableLocal.ToListAsync();
                if (itemsLocal.Any()) //R5 - vacia los datos desde el sqlLite
                {
                    for (int i = itemsLocal.Count -1; i >= 0; i--)
                    {
                        System.Collections.Generic.List<TandaItem> itemsAActualizar = await _TandaItemTable.ToListAsync();
                        var itemTandaActualizar = itemsAActualizar.Where(x => x.id == itemsLocal[i].id).FirstOrDefault();
                        if (itemTandaActualizar.deuda != itemsLocal[i].deuda)
                        {
                            itemTandaActualizar.deuda = itemsLocal[i].deuda;
                            await _TandaItemTable.UpdateAsync(itemTandaActualizar);
                        }
                        itemsLocal.Remove(itemsLocal[i]);
                    }
                }
            }
            else
            {
                _TandaItemTable = client.GetTable<TandaItem>();
            }
            System.Collections.Generic.List<TandaItem> items = await _TandaItemTable.ToListAsync();
            var itemTanda = items.Where(x => x.id == _id).FirstOrDefault();
            itemTanda.deuda = itemTanda.deuda - cantidad;
            
            await _TandaItemTable.UpdateAsync(itemTanda);
           
        }

        private async Task InitLocalStoreAsync()
        {
            string path = Path.Combine(System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.Personal), localDbFilename);
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<TandaItem>();
            await client.SyncContext.InitializeAsync(store);

            
        }

    }
}