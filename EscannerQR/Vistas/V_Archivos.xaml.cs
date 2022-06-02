using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EscannerQR.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Archivos : ContentPage
    {
        private SQLiteAsyncConnection _conn;
        private ObservableCollection<T_Archivos> _TablaArchivos;
        public V_Archivos()
        {
            InitializeComponent();
            _conn = DependencyService.Get<ISQLiteBD>().GetConnection();
            
        }

        protected async override void OnAppearing()
        {
            var Resultados = await _conn.Table<T_Archivos>().ToListAsync();
            _TablaArchivos = new ObservableCollection<T_Archivos>(Resultados.OrderByDescending(i => i.Id));
            ListaArchivos.ItemsSource = _TablaArchivos;
            


            base.OnAppearing();
        }

        private void ListaArchivos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var obj = (T_Archivos)e.SelectedItem;
            var item = obj.Id;
            string F_Archivo = Convert.ToString(item);
            try
            {
                Navigation.PushAsync(new V_Items(F_Archivo));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void BtnBorrarArchivos_Clicked(object sender, EventArgs e)
        {

            var EliminarOK = await DisplayAlert("Eliminar todo", "¿Desea eliminar todo?", "SI", "NO");
            if(EliminarOK == true)
            {
                await _conn.DeleteAllAsync<T_Archivos>();
                await _conn.ExecuteAsync("Delete from sqlite_sequence where name = 'T_Archivos';");

                await _conn.DeleteAllAsync<T_Items>();
                await _conn.ExecuteAsync("Delete from sqlite_sequence where name = 'T_Items';");

                await _conn.DeleteAllAsync<T_SeriesEtiquetas>();
                await _conn.ExecuteAsync("Delete from sqlite_sequence where name = 'T_SeriesEtiquetas';");
                OnAppearing();
            }

        }


        
     
    }
}