using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EscannerQR.Vistas
{
    public interface ISQLiteBD
    {
        SQLiteAsyncConnection GetConnection();
    }
}
