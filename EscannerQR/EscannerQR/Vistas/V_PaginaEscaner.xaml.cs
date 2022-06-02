using Plugin.SimpleAudioPlayer;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;

namespace EscannerQR.Vistas
{


    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_PaginaEscaner : ContentPage
    {
        private SQLiteAsyncConnection _conn = DependencyService.Get<ISQLiteBD>().GetConnection();
        
        public ObservableCollection<T_SeriesEtiquetas> Series { get; set; }
        public ObservableCollection<T_Items> Escaneos { get; set; }

        string IDFolioI;

        private bool _isScanning = true;

        public V_PaginaEscaner(string N_IDfolio)
        {
            InitializeComponent();
            IDFolioI = N_IDfolio;
            _conn.CreateTableAsync<T_Items>();
            _conn.CreateTableAsync<T_SeriesEtiquetas>();
            //BtnFlashApa.IsVisible = false;
            Series = new ObservableCollection<T_SeriesEtiquetas>();

            //var opciones = new MobileBarcodeScanningOptions { DelayBetweenContinuousScans = 30000};
            ZxingScannerView.Options = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
                DelayBetweenContinuousScans = 1000,
                PossibleFormats = new List<ZXing.BarcodeFormat>
                { ZXing.BarcodeFormat.QR_CODE}
            };

            
        }

        protected override void OnAppearing()
        {
            MostrarFolio();
            MostrarSeries();
            MostrarItems();
            base.OnAppearing();
        }

        private async void MostrarFolio()
        {
            long IDFolioL = long.Parse(IDFolioI);
            var fol = await _conn.Table<T_Archivos>().Where(f => f.Id == IDFolioL).FirstAsync();
            var _fol = fol.Folio;
            string FolioNombre = Convert.ToString(_fol);
            LbArchivo.Text = "Archivo: " + FolioNombre.ToString();
        }

        private async void MostrarSeries()
        {
            var SeriesResultados = await _conn.Table<T_SeriesEtiquetas>().Where(a => a.IDArchivo == IDFolioI).OrderByDescending(a => a.ID).ToListAsync();
            Series = new ObservableCollection<T_SeriesEtiquetas>(SeriesResultados);
            ListaSeries.ItemsSource = Series;

            LbTotalRegistros.Text = "Total Escaneos: " + Series.Count().ToString();
        }

        private async void MostrarItems()
        {
            var ItemsResultados = await _conn.Table<T_Items>().Where(a => a.Folio_Vin == IDFolioI).OrderByDescending(a => a.FechaEscaneo).ToListAsync();
            Escaneos = new ObservableCollection<T_Items>(ItemsResultados);
            ListaEscaneos.ItemsSource = Escaneos;

            LbContador.Text = "Total Articulos: " + Escaneos.Count().ToString();
            
        }

        ///////////Configuracion de Audio de bip y de error
        //private void Play()
        //{
        //    var assembly = typeof(App).GetTypeInfo().Assembly;
        //    Stream audiostream = assembly.GetManifestResourceStream("EscannerQR.Beep_1.mp3");
        //    var audio = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
        //    audio.Load(audiostream);
        //    audio.Play();
        //}
        private void PlayError()
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            Stream audiostreamError = assembly.GetManifestResourceStream("EscannerQR.Error.mp3");
            var audioError = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            audioError.Load(audiostreamError);
            audioError.Play();
        }

        private void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                ///Valida si esta escaneando o no
            if (_isScanning)
            {
                    /// Establece en falso para amarrar el proceso - Si encuentra algo se congela la pantalla
                _isScanning = false;
                ZxingScannerView.IsScanning = false;
                    //ZxingScannerView.IsAnalyzing = false;

                    try
                    {

                        string IDEtiquetaI = "";
                        string IDNumparteI = "";
                        string NumparteI = "";
                        string DescripcionI = "";
                        string CantidadI = "";
                        string TipoEmpaqueI = "";
                        string OrdenCompraI = "";
                        string FolioImpI = "";

                        int ContarPorNumParte = 1;
                        int AcumTotal = 1;

                        //Separa la cadena en partes que contengan "|"
                        var resultados = result.Text.Split('|');

                        //Numera cuantos campos existen en N.
                        int n = resultados.Length;
                        if (n != 8)
                        {
                            PlayError();
                            Vibration.Vibrate(1000);
                            await DisplayAlert("Codigo QR incompatible", "El codigo QR no cumple con la estructura de campos necesaria", "OK");
                            //audioError.Play();
                            
                            //Insertar audio error - Error de JavaLang al activarse, no es necesario que lleve sonido.
                        }
                        else
                        {
                            //Si alguna de las columnas no contiene algun caracter (Si su recorrido es 0).
                            if (resultados[0].Length == 0 || resultados[1].Length == 0 || resultados[2].Length == 0 || resultados[3].Length == 0 || resultados[4].Length == 0 || resultados[5].Length == 0 || resultados[6].Length == 0 || resultados[7].Length == 0)
                            {
                                //Inserta cual de los siguientes elementos falta en el codigo para no escanearlo.
                                string IdentificaCabeceras = "Sin serie de Etiqueta,  Sin codigo de articulo,Sin descripcion de articulo,Sin orden de compra,Sin cantidad estandar,Sin tipo de empaque, Sin IdArticulo";
                                string[] cabec = IdentificaCabeceras.Split(',');
                                for (int i = 0; i < 8; i++)
                                {

                                    if (resultados[i].Length == 0)
                                    {
                                        //Inserta Audio de error
                                        PlayError();
                                        Vibration.Vibrate(1000);
                                        await DisplayAlert("Codigo QR con valores vacíos", "Falta un valor en el codigo que impide el escaneo. \nValor vacío: " + cabec[i].ToString(), "OK");
                                        

                                    }
                                }
                            }
                            else
                            {
                                //Identifica cada campo
                                IDEtiquetaI = resultados[0];
                                NumparteI = resultados[1];
                                DescripcionI = resultados[2];
                                OrdenCompraI = resultados[3];
                                CantidadI = resultados[4];
                                TipoEmpaqueI = resultados[5];
                                IDNumparteI = resultados[6];
                                FolioImpI = resultados[7];


                                //Separa el campo IDEtiqueta
                                int guion = IDEtiquetaI.IndexOf('-');
                                string IDserieQRI = IDEtiquetaI.Substring(0, guion);

                                //Cuenta cuantos escaneos hay
                                string Counterst = Escaneos.Count().ToString();


                                //LblResultados.HorizontalTextAlignment = TextAlignment.Start;
                                //LblResultados.VerticalTextAlignment = TextAlignment.Center;

                                //Busca en las serie (IDEtiquetaI) y valida si existe o no
                                var id_Series = Series.Where(s => s.NumSerie == IDEtiquetaI).FirstOrDefault();
                                if (id_Series != null)
                                {
                                    //Si existe, manda mensaje de codigo repetido y no inserta nada
                                    PlayError();
                                    Vibration.Vibrate(1000);

                                    await DisplayAlert("Codigo QR repetido", $"Codigo serie repetida {IDEtiquetaI}", "OK");
                                    
                                }
                                else
                                {
                                    try
                                    {



                                        //Busca ahora el ID del numero de parte, la cantidad, tipo de empaque y orden de compra
                                        var ItemsEscaner = Escaneos.Where(e =>
                                                                e.IDNumparte == IDNumparteI &&
                                                                e.Cantidad == CantidadI &&
                                                                e.TipoEmpaque == TipoEmpaqueI &&
                                                                e.OrdenCompra == OrdenCompraI

                                                                //e.IDserieQR == IDserieQRI && 
                                                                //e.Numparte == NumparteI && 
                                                                //e.Descripcion == DescripcionI &&
                                                                //e.FolioImp == FolioImpI
                                                                ).FirstOrDefault();
                                        //Valida si existe alguno
                                        if (ItemsEscaner != null)
                                        {
                                            var assembly = typeof(App).GetTypeInfo().Assembly;
                                            Stream audiostream = assembly.GetManifestResourceStream("EscannerQR.Beep_1.mp3");
                                            var audio = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                                            audio.Load(audiostream);
                                            audio.Play();

                                            Vibration.Vibrate(100);

                                            //Si existe, busca con los mismos datos y los agrupa llamandolo LineasACambiar
                                            //var LineaACambiar = Escaneos.Where(e =>
                                            //                    e.IDNumparte == IDNumparteI &&
                                            //                    e.Cantidad == CantidadI &&
                                            //                    e.TipoEmpaque == TipoEmpaqueI &&
                                            //                    e.OrdenCompra == OrdenCompraI).FirstOrDefault();

                                            //Añade el contador + 1 y multiplica el acumulado del contador por el standarpack
                                            ContarPorNumParte = int.Parse(ItemsEscaner.CantidadReg) + 1;
                                            AcumTotal = int.Parse(ItemsEscaner.Cantidad) * ContarPorNumParte;

                                            //Convierte a string para actualizar
                                            ItemsEscaner.CantidadReg = ContarPorNumParte.ToString();
                                            ItemsEscaner.CantidadTot = AcumTotal.ToString();

                                            //Actualiza
                                            try
                                            {
                                                await _conn.UpdateAsync(ItemsEscaner);
                                                OnAppearing();
                                                
                                            }
                                            catch
                                            {
                                                Vibration.Vibrate(1000);
                                                await DisplayAlert("escaneo", "Error al ingresar linea", "OK");
                                            }



                                        }
                                        else
                                        {
                                            var assembly = typeof(App).GetTypeInfo().Assembly;
                                            Stream audiostream = assembly.GetManifestResourceStream("EscannerQR.Beep_1.mp3");
                                            var audio = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                                            audio.Load(audiostream);
                                            audio.Play();

                                            Vibration.Vibrate(100);

                                            //Si aun no existe la linea, ingresa todos los campos con la cantidad 1
                                            var DatosItems = new T_Items
                                            {
                                                Folio_Vin = IDFolioI.ToString(),
                                                IDserieQR = IDserieQRI,
                                                IDNumparte = IDNumparteI,
                                                Numparte = NumparteI,
                                                Descripcion = DescripcionI,
                                                OrdenCompra = OrdenCompraI,
                                                TipoEmpaque = TipoEmpaqueI,
                                                FolioImp = FolioImpI,
                                                Cantidad = CantidadI,
                                                CantidadReg = "1",
                                                CantidadTot = CantidadI,
                                                FechaEscaneo = DateTime.Now
                                            };
                                            Escaneos.Add(DatosItems);
                                            await _conn.InsertAsync(DatosItems);

                                            OnAppearing();
                                            



                                        }

                                        //si no existe, agregar los datos y continuar
                                        var DatosSerie = new T_SeriesEtiquetas
                                        {
                                            NumSerie = IDEtiquetaI,
                                            IDArchivo = IDFolioI.ToString()
                                        };

                                        Series.Add(DatosSerie);
                                        await _conn.InsertAsync(DatosSerie);
                                        OnAppearing();

                                    }
                                    catch
                                    {
                                        Vibration.Vibrate(1000);
                                        await DisplayAlert("Codigo QR", "Error al insertar QR", "OK");

                                    }





                                }
                                //await DisplayAlert("scan resultado", NombreFolioI + result.Text, "OK");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //En caso de algun error, muestra un aviso de error de escaneo y te manda el siguiente mensaje.
                        Vibration.Vibrate(1000);
                        await DisplayAlert("Error en escaneo", "Favor de revisar el siguiente error " + ex, "OK");
                    }


                    /// Reestablece la camara hasta que termina el proceso
                    ZxingScannerView.IsScanning = true;
                    //ZxingScannerView.IsAnalyzing = true;
                    _isScanning = true;
                    //ZxingScannerView.Options.DelayBetweenAnalyzingFrames = 3000;
                }
            });
        }


        protected override bool OnBackButtonPressed()
        {
            //Inserta una pagina nueva al presionar el boton de atras.
            Navigation.InsertPageBefore(new V_Items(IDFolioI), this);
            //Navigation.RemovePage(this);
            //return true;
            return base.OnBackButtonPressed();
        }

    }
    

}