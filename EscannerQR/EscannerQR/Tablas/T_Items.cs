using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace EscannerQR
{
    public class T_Items
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Folio_Vin { get; set; }
        [MaxLength(255)]
        public string IDserieQR { get; set; }
        [MaxLength(255)]
        public string IDNumparte { get; set; }
        [MaxLength(255)]
        public string Numparte { get; set; }
        [MaxLength(255)]
        public string OrdenCompra { get; set; }
        [MaxLength(255)]
        public string Descripcion { get; set; }
        [MaxLength(255)]
        public string TipoEmpaque { get; set; }
        [MaxLength(255)]
        public string Cantidad { get; set; }
        [MaxLength(255)]
        public string FolioImp { get; set; }
        [MaxLength(255)]
        public string CantidadReg { get; set; }
        [MaxLength(255)]
        public string CantidadTot { get; set; }
        [MaxLength(255)]
        public DateTime FechaEscaneo { get; set; }
    }
}
