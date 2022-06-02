

using EscannerQR.viewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EscannerQR.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Items : ContentPage
    {

        private SQLiteAsyncConnection _conn = DependencyService.Get<ISQLiteBD>().GetConnection();
        public string IDFolioseleccionado;
        private ObservableCollection<T_Items> _TablaItems;
        private ObservableCollection<T_SeriesEtiquetas> _TablaEtiquetas;


        //public Archivos(string F_Archivo);

        public long IDF;
        public string TextoArchivo;


        public V_Items(string F_Archivo)
        {
            InitializeComponent();
            IDFolioseleccionado = F_Archivo;
            _conn.CreateTableAsync<T_Archivos>();
            _conn.CreateTableAsync<T_Items>();
            _conn.CreateTableAsync<T_SeriesEtiquetas>();

        }

        class CSVEstructura { public List<string> Valores { get; set; } = new List<string>(); }

        protected async override void OnAppearing()
        {

            //await _conn.CreateTableAsync<T_Items>();
            //await _conn.CreateTableAsync<T_SeriesEtiquetas>();
            //_conn.QueryAsync<T_Items>("SELECT * FROM T_Items Where Folio_Vin = ?",Folioseleccionado.ToString());
            //var ItemsResultados = await _conn.Table<T_Items>().ToListAsync();


            var ItemsResultados = await _conn.Table<T_Items>().Where(a => a.Folio_Vin == IDFolioseleccionado).OrderByDescending(a => a.FechaEscaneo).ToListAsync();
            _TablaItems = new ObservableCollection<T_Items>(ItemsResultados);
            ListaItems.ItemsSource = _TablaItems;

            var SeriesResultados = await _conn.Table<T_SeriesEtiquetas>().Where(a => a.IDArchivo == IDFolioseleccionado).ToListAsync();
            _TablaEtiquetas = new ObservableCollection<T_SeriesEtiquetas>(SeriesResultados);
            ListaSeries.ItemsSource = _TablaEtiquetas;

            IDF = long.Parse(IDFolioseleccionado);




            var obj = await _conn.Table<T_Archivos>().Where(b => b.Id == IDF).FirstAsync();
            var item = obj.Folio;
            TextoArchivo = Convert.ToString(item);
            //LblFolio.Text = IDFolioseleccionado;
            LblIDFolio.Text = TextoArchivo.ToString();
            LblFolio.Text = "Archivo: " + TextoArchivo;
            base.OnAppearing();
        }

        private void BtnExportarExcel_Clicked(object sender, EventArgs e)
        {
            GeneraCSVItems();
        }

        public static string EncriptaString(string str)
        {
            //Convierte a Hexadecimal
            byte[] Hexa1 = System.Text.Encoding.UTF8.GetBytes(str);
            str = BitConverter.ToString(Hexa1);
            str = str.Replace("-", "");

            //Encripta a base 64
            byte[] EncriptadoV2 = System.Text.Encoding.UTF8.GetBytes(str);
            str = Convert.ToBase64String(EncriptadoV2);
            return str;
        }



        private void GeneraCSVItems()
        {
            if (_TablaItems.Count() == 0)
            {
                DisplayAlert("Error", "No hay filas para convertir, Se eliminó el archivo", "OK");
                _conn.Table<T_Archivos>().Where(X => X.Id == IDF).DeleteAsync();
                
                Navigation.PopAsync();
                //BindingContext = new ScanViewModel(this.Navigation);
            }
            else
            {

                //Creamos el folder donde se genera el CSV y el nombre del archivo (HE.CSV)
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                //string filename = Path.Combine(path, "HE.csv");

                string filename = Path.Combine(path, TextoArchivo + ".csv");

                var csv = new StringBuilder();
                //Utilidad para escribir dentro del archivo (StreamWriter)
                using (var streamWriter = new StreamWriter(filename, false))
                {
                    //Leemos la lista en un foreach y lo añadimos por columnas
                    foreach (var i in _TablaItems)
                    {
                        //var line = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}", EncriptaString(i.IDserieQR), EncriptaString(i.Numparte), EncriptaString(i.Descripcion), EncriptaString(i.OrdenCompra), EncriptaString(i.TipoEmpaque), EncriptaString(i.CantidadReg), EncriptaString(i.Cantidad),  EncriptaString(i.CantidadTot), EncriptaString(i.IDNumparte), EncriptaString(i.FolioImp), EncriptaString(i.Folio_Vin));
                        var line = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}", 
                                                    //EncriptaString(i.IDserieQR), 
                                                    EncriptaString(i.Numparte), 
                                                    EncriptaString(i.Descripcion), 
                                                    EncriptaString(i.OrdenCompra), 
                                                    EncriptaString(i.TipoEmpaque), 
                                                    EncriptaString(i.CantidadReg), 
                                                    EncriptaString(i.Cantidad), 
                                                    EncriptaString(i.CantidadTot), 
                                                    EncriptaString(i.IDNumparte),
                                                    //EncriptaString(i.FolioImp), 
                                                    EncriptaString(i.Folio_Vin)
                                                    );

                        streamWriter.WriteLine(line);
                        streamWriter.Flush();
                    }
                    //Cierra el archivo para que al crear otro archivo no acumule datos.
                    //streamWriter.Close();
                };

                //Para solo abrir el archivo creado
                //await Launcher.OpenAsync(new OpenFileRequest()
                //{
                //    File = new ReadOnlyFile(filename)
                //});


                //Para compartir el archivo csv
                Share.RequestAsync(new ShareFileRequest()
                {
                    Title = "Enviar",
                    File = new ShareFile(filename)
                });
            }
        }

        private async void BtnEliminarArchivo_Clicked(object sender, EventArgs e)
        {


            var EliminadoOK = await DisplayAlert("Eliminar archivo", TextoArchivo + " sera eliminado", "Si", "No");
            if (EliminadoOK == true)
            {
                await _conn.Table<T_Archivos>().Where(X => X.Id == IDF).DeleteAsync();
                await _conn.Table<T_Items>().Where(X => X.Folio_Vin == IDFolioseleccionado).DeleteAsync();
                await _conn.Table<T_SeriesEtiquetas>().Where(X=> X.IDArchivo == IDFolioseleccionado).DeleteAsync();
                await Navigation.PopAsync();
            }

        }

        private void BtnContinuaEscaneo_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new V_PaginaEscaner(IDFolioseleccionado));
            Navigation.RemovePage(this);
        }
    }
}