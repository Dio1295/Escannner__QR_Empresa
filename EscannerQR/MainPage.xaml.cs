using EscannerQR.viewModels;
using EscannerQR.Vistas;
using SQLite;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace EscannerQR
{

    public partial class MainPage : ContentPage
    {

        private SQLiteAsyncConnection _conn;

        public MainPage()
        {
            InitializeComponent();
            _conn = DependencyService.Get<ISQLiteBD>().GetConnection();
            _conn.CreateTableAsync<T_Archivos>();
            _conn.CreateTableAsync<T_Items>();

            BindingContext = new ScanViewModel(this.Navigation);
            ToolbarItems.Add(new ToolbarItem("?", "DE", () =>
            {
                DisplayAlert("Creditos", "David Dionicio Carbajal Hernández \nViñoplastic Inyeccion \nVersion 1.4.0", "Entendido");
            }));

            BtnOK.IsEnabled = false;
            DeviceDisplay.KeepScreenOn = true;


        }

        private void BtnVerArchivos_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new V_Archivos());

   
        }

        private void BtnEscanearQR_Clicked(object sender, EventArgs e)
        {
            //ListaCodigos.IsVisible = true;
            NombreFolio.IsVisible = true;
            CtnLista.IsVisible = false;
            TxtFolio.Text = "";
        }


        private void BtnVerArchivos1_Clicked(object sender, EventArgs e)
        {
            
            Navigation.PushAsync(new V_Archivos());

        }

         public void BtnEscanearQR_P_Clicked(object sender, EventArgs e)
        {
             Navigation.PushAsync(new V_NombreArchivo());
            //NombreFolio.IsVisible = true;
            //FrButtons.IsVisible = false;
        }

        private void BtnOK_Clicked(object sender, EventArgs e)
        {
            CtnLista.IsVisible = true;
            NombreFolio.IsVisible = false;
            FrButtons.IsVisible = false;

        }

        private void TxtFolio_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtFolio.Text))
            {
                BtnOK.IsEnabled = false;
            }
            else
            {
                BtnOK.IsEnabled = true;
            }
        }
    }
}
