using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace EscannerQR
{
    public class T_Archivos
    {

            [PrimaryKey ]
            public long Id { get; set; }
            [MaxLength(255)]
            public string Folio { get; set; }
            //[MaxLength(255)]
            public DateTime Fecha { get; set; }

    }
}
