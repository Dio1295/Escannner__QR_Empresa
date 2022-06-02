//using EscannerQR.ViewModels;
using EscannerQR.ViewModels;
using EscannerQR.Vistas;
using Plugin.SimpleAudioPlayer;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;


namespace EscannerQR.viewModels

{



    //// Definimos las columnas de la lista que tenemos como tabla
    //class Codigos
    //{
    //    public string NumLinea { get; set; }
    //    public string Folio_Vin { get; set; }
    //    public string IDserieQR { get; set; }
    //    public string IDNumparte { get; set; }
    //    public string Numparte { get; set; }
    //    public string OrdenCompra { get; set; }
    //    public string Descripcion { get; set; }
    //    public string TipoEmpaque { get; set; }
    //    public string Cantidad { get; set; }
    //    public string FolioImp { get; set; }
    //    public string CantidadReg { get; set; }
    //    public string CantidadTot { get; set; }
    //}

    //class IDEtiquetas
    //{
    //    public string IDEtiqueta { get; set; }
    //}

    //class Folios
    //{
    //    public string Folio { get; set; }
    //}
    ////Estructura para poner valores en lista CSV
    //class CSVEsctructure { public List<string> Values { get; set; } = new List<string>(); }






class ScanViewModel :BaseViewModel
    {
        //private SQLiteAsyncConnection _conn;
        //public DateTime FechaI;
        //private string _folio;
        //public string Folio
        //{
        //    get { return _folio; }
        //    set { _folio = value; }
        //}

        //public string _IDFolio;
        //public string IDFolio
        //{
        //    get { return _IDFolio; }
        //    set { _IDFolio = value; }
        //}

        ////Navegacion para llegar a la clase Modelo-Vista
        public INavigation Navigation { get; set; }

        ////Comandos que vienen de la pagina principal
        ////Comando para escaneo desde binding
        //public ICommand ScanCommand { get; set; }

        //public ICommand SeguirEscaneandoCommand { get; set; }

        ////comando para exportar a excel desde binding
        //public ICommand ExportToExcelCommand { private set; get; }

        //public ICommand Verarchivos { private set; get; }

        ////Creamos la lista a la cual llamaremos Items.
        //public ObservableCollection<Codigos> Items { get; set; }
        //public ObservableCollection<IDEtiquetas> IDEtis { get; set; }


        ////LISTA - LISTVIEW - MODELOVISTA
        public ScanViewModel(INavigation navigation)
        {
            //    //Generamos la accion del comando Scanncommand a la clase ScanCode
            Navigation = navigation;

            //    ScanCommand = new Command(async () => await ScanCode());
            //    //BtnEscanearQR_P_Clicked = new Command(async () => await ScanCode());


            //    //Generamos vinculo entre la clase Codigos y la lista Items para llenado
            //    Items = new ObservableCollection<Codigos>();
            //    IDEtis = new ObservableCollection<IDEtiquetas>();

            //    // Generamos la accion del comando a la clase GenerarCSV
            //    ExportToExcelCommand = new Command(async () => await GeneraCSV());



        }

        ////INTERFAZ PROPIA DE ESCANEO ZXING EN VENTANA NUEVA
        //async Task ScanCode()
        //{

        //    //Creacion de archivo
        //    FechaI = DateTime.Now;
        //    string FolioI = "";
        //    long ID_Folio;
        //    FolioI = Folio;
        //    ID_Folio = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

        //    _conn = DependencyService.Get<ISQLiteBD>().GetConnection();
        //    await _conn.CreateTableAsync<T_Archivos>();
        //    await _conn.CreateTableAsync<T_Items>();
        //    await _conn.CreateTableAsync<T_SeriesEtiquetas>();

        //    var DatosFolios = new T_Archivos { Id = ID_Folio, Folio = FolioI, Fecha = FechaI };
        //    await _conn.InsertAsync(DatosFolios);

        //    string ID_FolioI = ID_Folio.ToString();

        //    Vibration.Vibrate();
        //    var duration = TimeSpan.FromSeconds(1);

        //    //Sonido de escaneo
        //    var assembly = typeof(App).GetTypeInfo().Assembly;
        //    Stream audiostream = assembly.GetManifestResourceStream("EscannerQR.Beep_1.mp3");
        //    Stream audiostreamError = assembly.GetManifestResourceStream("EscannerQR.Error.mp3");
        //    var audio = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
        //    var audioError = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
        //    audio.Load(audiostream);
        //    audioError.Load(audiostreamError);



        //    //Opciones para escaneo (Pausa entre escaneos, Solo Formato QR)

        //    var zxing = new ZXingScannerView
        //    {
        //        HorizontalOptions = LayoutOptions.FillAndExpand,
        //        VerticalOptions = LayoutOptions.FillAndExpand,
        //        AutomationId = "zxingScannerView",
        //    };
        //    var options = new MobileBarcodeScanningOptions { DelayBetweenContinuousScans = 1500 };
        //    options.PossibleFormats = new List<BarcodeFormat>()
        //    {
        //        ZXing.BarcodeFormat.QR_CODE
        //    };



        //    //Texto que va en la interfaz de escaneo y propiedades de opacidad
        //    var overlay = new ZXingDefaultOverlay
        //    {
        //        TopText = "Coloca el codigo frente al dispositivo",
        //        BottomText = "El escaneo es automatico",
        //        ShowFlashButton = true,
        //        Opacity = 1,
        //        HorizontalOptions = LayoutOptions.FillAndExpand,
        //        VerticalOptions = LayoutOptions.FillAndExpand,

        //    };

        //    //Mandamos la variable overlay a la nueva pagina de escaneo
        //    overlay.BindingContext = overlay;


        //    //Parametros de la pagina para habilitar el titulo de la pagina
        //    var page = new ZXingScannerPage(options, overlay)
        //    {
        //        Title = "Escaner QR Viñoplastic",


        //    };


        //    //overlay.ShowFlashButton = page.HasTorch;
        //    //overlay.FlashButtonClicked += (sender, e) =>
        //    //{
        //    //    zxing.IsTorchOn = !zxing.IsTorchOn;
        //    //};

        //    //Activar la navegacion para abrir la pagina como nueva
        //    await Navigation.PushAsync(page, true);



        //    //Declaramos el contador de escaneos (Reinicia cada ves que vuelves a escanear).
        //    Items.Clear();
        //    IDEtis.Clear();

        //    int Counter = 0;

        //    //Toma los resultados de la pagina
        //    page.OnScanResult += (result) =>
        //    {

        //        page.IsScanning = false;

        //        Device.BeginInvokeOnMainThread(() =>
        //        {
        //            string IDEtiquetaI = "";
        //            string IDNumparteI = "";
        //            string NumparteI = "";
        //            string DescripcionI = "";
        //            string CantidadI = "";
        //            string TipoEmpaqueI = "";
        //            string OrdenCompraI = "";
        //            string FolioImpI = "";


        //            int CountByPart = 1;
        //            int AcumTotal = 1;


        //            //Divide resultados separados por comas
        //            var resultados = result.Text.Split('|');
        //            int n = resultados.Length;
        //            if (n != 8)
        //            {
        //                audioError.Play();
        //                Application.Current.MainPage.DisplayAlert("Codigo QR incompatible", "El codigo QR no cumple con la estructura de campos necesaria", "OK");
        //                page.IsScanning = true;
        //            }
        //            else
        //            {

        //                if (resultados[0].Length == 0 || resultados[1].Length == 0 || resultados[2].Length == 0 || resultados[3].Length == 0 || resultados[4].Length == 0 || resultados[5].Length == 0 || resultados[6].Length == 0 || resultados[7].Length == 0)
        //                {
        //                    audioError.Play();
        //                    string Cabeceras = "Sin serie de Etiqueta,  Sin codigo de articulo,Sin descripcion de articulo,Sin orden de compra,Sin cantidad estandar,Sin tipo de empaque, Sin IdArticulo";
        //                    string[] cabec = Cabeceras.Split(',');
        //                    for (int i = 0; i < 8; i++)
        //                    {
        //                        if (resultados[i].Length == 0)
        //                        {
        //                            audioError.Play();
        //                            Application.Current.MainPage.DisplayAlert("Codigo QR con valores vacíos", "Falta un valor en el codigo que impide el escaneo. \nValor vacío: " + cabec[i].ToString(), "OK");
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    //Variables de los resultados
        //                    IDEtiquetaI = resultados[0];
        //                    NumparteI = resultados[1];
        //                    DescripcionI = resultados[2];
        //                    OrdenCompraI = resultados[3];
        //                    CantidadI = resultados[4];
        //                    TipoEmpaqueI = resultados[5];
        //                    IDNumparteI = resultados[6];
        //                    FolioImpI = resultados[7];


        //                    int guion = IDEtiquetaI.IndexOf('-');
        //                    string IDserieQRI = IDEtiquetaI.Substring(0, guion);

        //                    //Contador de escaneos acumulados
        //                    Counter++;

        //                    string counter = Items.Count().ToString();

        //                    //string contador = Counter.ToString();

        //                    //Añade los codigos en la lista 
        //                    var id = IDEtis.Where(i => i.IDEtiqueta == IDEtiquetaI).FirstOrDefault();
        //                    if (id != null)
        //                    {
        //                        audioError.Play();
        //                        Application.Current.MainPage.DisplayAlert("Codigo QR repetido", $"Codigo serie repetida {IDEtiquetaI}", "OK");
        //                        page.IsScanning = false;
        //                    }
        //                    else
        //                    {
        //                        IDEtis.Add(new IDEtiquetas() { IDEtiqueta = IDEtiquetaI });
        //                        // Si el <codigo | Nombre | Cantidad | TipoEmpaque> existe Actualiza la cantidad de CountbyPart y CantTot tanto en la lista Codigos como en T_Items de SQLite
        //                        var it = Items.Where(i => i.IDserieQR == IDserieQRI && i.IDNumparte == IDNumparteI && i.Numparte == NumparteI && i.Descripcion == DescripcionI && i.Cantidad == CantidadI && i.TipoEmpaque == TipoEmpaqueI && i.OrdenCompra == OrdenCompraI && i.FolioImp == FolioImpI).FirstOrDefault();
        //                        if (it != null)
        //                        {
        //                            audio.Play();
        //                            var linea = Items.FirstOrDefault(i => i.IDserieQR == IDserieQRI && i.IDNumparte == IDNumparteI && i.Numparte == NumparteI && i.Descripcion == DescripcionI && i.Cantidad == CantidadI && i.TipoEmpaque == TipoEmpaqueI && i.OrdenCompra == OrdenCompraI && i.FolioImp == FolioImpI);
        //                            CountByPart = int.Parse(linea.CantidadReg) + 1;
        //                            AcumTotal = int.Parse(linea.Cantidad) * CountByPart;
        //                            Items.Remove(linea);
        //                            Items.Add(new Codigos() { NumLinea = counter, Folio_Vin = ID_FolioI, IDserieQR = IDserieQRI, IDNumparte = IDNumparteI, Numparte = NumparteI, Descripcion = DescripcionI, OrdenCompra = OrdenCompraI, TipoEmpaque = TipoEmpaqueI, FolioImp = FolioImpI, Cantidad = CantidadI, CantidadReg = CountByPart.ToString(), CantidadTot = AcumTotal.ToString() });


        //                            // Se aparta la actualizacion en SQLite porque BeginInovkeMainthread no soporta await
        //                            ActualizaDato(IDserieQRI, IDNumparteI, NumparteI, DescripcionI, CantidadI, TipoEmpaqueI, CountByPart, AcumTotal, ID_FolioI, OrdenCompraI, FolioImpI);
        //                            overlay.TopText = $"Folio generado: {FolioI} \nCodigo: {NumparteI} - {DescripcionI} \nNumero de escaneos: {IDEtis.Count()}";
        //                        }
        //                        else
        //                        {
        //                            audio.Play();
        //                            Items.Add(new Codigos() { NumLinea = "1", Folio_Vin = ID_FolioI, IDserieQR = IDserieQRI, IDNumparte = IDNumparteI, Numparte = NumparteI, Descripcion = DescripcionI, OrdenCompra = OrdenCompraI, TipoEmpaque = TipoEmpaqueI, FolioImp = FolioImpI, Cantidad = CantidadI, CantidadReg = CountByPart.ToString(), CantidadTot = CantidadI });
        //                            var DatosItems = new T_Items { Folio_Vin = ID_FolioI, IDserieQR = IDserieQRI, IDNumparte = IDNumparteI, Numparte = NumparteI, Descripcion = DescripcionI, OrdenCompra = OrdenCompraI, TipoEmpaque = TipoEmpaqueI, FolioImp = FolioImpI, Cantidad = CantidadI, CantidadReg = CountByPart.ToString(), CantidadTot = CantidadI };
        //                            _conn.InsertAsync(DatosItems);
        //                            overlay.TopText = $"Folio generado: {FolioI} \nCodigo: {NumparteI} - {DescripcionI} \nNumero de escaneos: {IDEtis.Count()}";

        //                        }
        //                    }

        //                    //Reproduce audio al añadir codigo
        //                    //Efecto de vibracion al añadir un codigo
        //                    //Vibration.Vibrate(duration);  

        //                }
        //            }

        //        });
        //    };


    }

        //async Task ActualizaDato(string IDserieQRI, string IDNumparteI, string NumparteI, string DescripcionI, string CantidadI, string TipoEmpaqueI, int CountByPart, int AcumTotal, string ID_FolioI, string OrdenCompraI, string FolioImpI)
        //{
        //    var DatosItems1 = await _conn.GetAsync<T_Items>( i => i.IDserieQR == IDserieQRI && i.IDNumparte == IDNumparteI && i.Numparte == NumparteI && i.Descripcion == DescripcionI && i.Cantidad == CantidadI && i.TipoEmpaque == TipoEmpaqueI && i.Folio_Vin == ID_FolioI && i.OrdenCompra == OrdenCompraI && i.FolioImp == FolioImpI);
        //    DatosItems1.CantidadReg = CountByPart.ToString();
        //    DatosItems1.CantidadTot = AcumTotal.ToString();

        //    //= new T_Items { Numparte = NumparteI, Descripcion = DescripcionI, Cantidad = CantidadI, TipoEmpaque = TipoEmpaqueI, Folio_Vin = FolioI, CantidadReg = CountByPart.ToString(), CantidadTot = AcumTotal.ToString() };
        //    await _conn.UpdateAsync(DatosItems1);
        //}


        //public static string EncriptaString(string str)
        //{
        //    //Convierte a Hexadecimal
        //    byte[] Hexa1 = System.Text.Encoding.UTF8.GetBytes(str);
        //    str = BitConverter.ToString(Hexa1);
        //    str = str.Replace("-", "");

        //    //Encripta a base 64
        //    byte[] EncriptadoV2 = System.Text.Encoding.UTF8.GetBytes(str);
        //    str = Convert.ToBase64String(EncriptadoV2);
        //    return str;

        //}

        //GENERA CSV
        //async Task GeneraCSV()
        //{

        //    if (Items.Count == 0)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", "No hay filas que enviar, Escanea nuevamente", "OK");

        //    }
        //    else
        //    {
        //        //Creamos el folder donde se genera el CSV y el nombre del archivo (HE.CSV)
        //        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        //        string filename = Path.Combine(path, Folio + ".csv");
        //        //string filename = Path.Combine(path, "HE.csv");

        //        //var csv = new StringBuilder();
        //        //Utilidad para escribir dentro del archivo (StreamWriter)

        //        var filestream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

        //        using (var streamWriter = new StreamWriter(filestream))
        //        {
        //            //Leemos la lista en un foreach y lo añadimos por columnas
        //            foreach (var i in Items)
        //            {

        //                ////i.IDserieQR = EncriptaString(i.IDserieQR);
        //                ////i.Numparte = EncriptaString(i.Numparte);
        //                ////i.Descripcion = EncriptaString(i.Descripcion);
        //                ////i.OrdenCompra = EncriptaString(i.OrdenCompra);
        //                ////i.TipoEmpaque = EncriptaString(i.TipoEmpaque);
        //                ////i.Cantidad = EncriptaString(i.Cantidad);
        //                ////i.CantidadReg = EncriptaString(i.CantidadReg);
        //                ////i.CantidadTot = EncriptaString(i.CantidadTot);
        //                ////i.FolioImp = EncriptaString(i.FolioImp);
        //                ////i.Folio_Vin = EncriptaString(i.Folio_Vin);
        //                ////i.IDNumparte = EncriptaString(i.IDNumparte);

        //                var line = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}", EncriptaString(i.IDserieQR), EncriptaString(i.Numparte), EncriptaString(i.Descripcion), EncriptaString(i.OrdenCompra), EncriptaString(i.TipoEmpaque), EncriptaString(i.CantidadReg), EncriptaString(i.Cantidad),  EncriptaString(i.CantidadTot), EncriptaString(i.IDNumparte), EncriptaString(i.FolioImp), EncriptaString(i.Folio_Vin));
        //                streamWriter.WriteLine(line);
        //                streamWriter.Flush();
        //            }
        //            //Cierra el archivo para que al crear otro archivo no acumule datos.
        //            //streamWriter.Close();
        //        };


        //        //Para solo abrir el archivo creado
        //        //await Launcher.OpenAsync(new OpenFileRequest()
        //        //{
        //        //    File = new ReadOnlyFile(filename)
        //        //});


        //        //Para compartir el archivo csv
        //        await Share.RequestAsync(new ShareFileRequest()
        //        {
        //            Title = "Enviar",
        //            File = new ShareFile(filename)
        //        });
        //    }

        //}






    //}
}
