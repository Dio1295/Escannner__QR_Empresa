using EscannerQR.viewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EscannerQR.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_NombreArchivo : ContentPage
    {

        private SQLiteAsyncConnection _conn = DependencyService.Get<ISQLiteBD>().GetConnection();
        DateTime FechaFolio;
        long IDFolio;
        public V_NombreArchivo()
        {
            InitializeComponent();
            BindingContext = new ScanViewModel(this.Navigation);
            TxtNomFol.Text = "";
            FechaFolio = DateTime.Now;
            _conn.CreateTableAsync<T_Archivos>();
        }

        private void TxtNomFol_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNomFol.Text))
            {
                BtnEscan.IsEnabled = false;
                
            }
            else
            {
                BtnEscan.IsEnabled = true;
                IDFolio = long.Parse(FechaFolio.ToString("yyyyMMddHHmmss"));
                

            }
        }

        private void BtnEscan_Clicked(object sender, EventArgs e)
        {
            var DatosFolios = new T_Archivos { Id = IDFolio, Folio = TxtNomFol.Text, Fecha = FechaFolio };
            _conn.InsertAsync(DatosFolios);

            Navigation.PushAsync(new V_PaginaEscaner(IDFolio.ToString()));
            Navigation.RemovePage(this);
        }
    }
}