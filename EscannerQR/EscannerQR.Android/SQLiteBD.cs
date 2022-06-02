using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EscannerQR.Droid;
using Xamarin.Forms;
using SQLite;
using System.IO;
using EscannerQR.Vistas;
[assembly: Dependency(typeof(SQLiteBD))]

namespace EscannerQR.Droid
{
    public class SQLiteBD : ISQLiteBD
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            var path = Path.Combine(documentsPath, "MySQLite.db3");
            var db = new SQLiteAsyncConnection(path);
            
            return new SQLiteAsyncConnection(path);
           
        }
    }
}