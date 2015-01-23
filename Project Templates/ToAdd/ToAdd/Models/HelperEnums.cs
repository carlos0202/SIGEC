using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGII_PFD.Models
{
    public class HelperEnums
    {
    }

    public enum DataTypes
    {
        Caracter = 104,       // OracleDbType.Char
        Texto = 126,         // OracleDbType.Varchar2
        Entero = 112,         // OracleDbType.Int32
        Entero_Largo = 113,   // OracleDbType.Int64
        Entero_Corto = 111,   // OracleDbType.Int16
        Decimal = 107,        // OracleDbType.Decimal
        Flotante = 108,       // OracleDbType.Double
        Fecha = 106,          // OracleDbType.Date
        Fecha_y_Hora = 106,   // OracleDbType.Date
        Hora = 123,           // OracleDbType.TimeStamp
        Byte = 103,           // OracleDbType.Byte
        Objeto = 102,         // OracleDbType.Blob
    }
}