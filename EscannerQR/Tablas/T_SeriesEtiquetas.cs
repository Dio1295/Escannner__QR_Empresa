using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EscannerQR
{
    public class T_SeriesEtiquetas
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set;}
        [MaxLength(255)]
        public string NumSerie { get; set; }
        [MaxLength(255)]
        public string IDArchivo { get; set; }

    }
}
