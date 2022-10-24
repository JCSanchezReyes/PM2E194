using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PM2E112.Models;
using System.Diagnostics;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Xamarin.Essentials;

namespace PM2E112.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListSitios : ContentPage
    {
        public ListSitios()
        {
            InitializeComponent();
      
        }
        private async void Cargar_Sitios()

        {
            var sitios = await App.DBase.getListSitio();
            Lista.ItemsSource = sitios;
        }

        private async void ListSitio_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var sitio = (Sitios)e.Item;

          
                bool answer = await DisplayAlert("AVISO", "¿Quiere ir al mapa?", "Si", "No");
                Debug.WriteLine("Answer: " + answer);

                if (answer == true)
                {

                    Map map = new Map();
                    map.BindingContext = sitio;
                    await Navigation.PushAsync(map);
                };
            

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();//recargara de nuevo la lista
            Lista.ItemsSource = await App.DBase.getListSitio();//Espera coleccion de elementos para enumerar en la forma que queramos
        }

        private async void Eliminar_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Confirmacion", "¿Quiere eliminar el registro?", "Si", "No");
            Debug.WriteLine("Answer: " + answer);
            if (answer == true)
            {
                var idSitio = (Sitios)(sender as MenuItem).CommandParameter;
                var result = await App.DBase.DeleteSitio(idSitio);

               if (result==1)
               {
                    DisplayAlert("Aviso", "Registro Eliminado", "OK");
                    Cargar_Sitios();
               }else
                {
                    DisplayAlert("Aviso", "Revisa", "OK");
                }
            };
        }

        private async void IrMapa_Clicked(object sender, EventArgs e)
        {
            var idSitio = (Sitios)(sender as MenuItem).CommandParameter;
            //await DisplayAlert("Aviso", "sitio " + idSitio, "ok");
     
            bool answer = await DisplayAlert("AVISO", "¿Quiere ir al mapa?", "Si", "No");
            Debug.WriteLine("Answer: " + answer);

            if (answer == true)
            {
                try
                {
                    var georequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                    var tokendecancelacion = new System.Threading.CancellationTokenSource();
                    var location = await Geolocation.GetLocationAsync(georequest, tokendecancelacion.Token);
                    if (location != null)
                    {

                        Map map = new Map();
                        //map.BindingContext = mi.CommandParameter.ToString();
                        await Navigation.PushAsync(map);
                    }
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    await DisplayAlert("Advertencia", "Este dispositivo no soporta GPS" + fnsEx, "Ok");
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    await DisplayAlert("Advertencia", "Error de Dispositivo, validar si su GPS esta activo", "Ok");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();

                }
                catch (PermissionException pEx)
                {
                    await DisplayAlert("Advertencia", "Sin Permisos de Geolocalizacion" + pEx, "Ok");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Advertencia", "Sin Ubicacion " + ex, "Ok");
                }  
            };
        }
    }
}