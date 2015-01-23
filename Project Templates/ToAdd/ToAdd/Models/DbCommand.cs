/*Procedimiento de prueba----------------------------

 PROCEDURE "SP_SUM_TEST" (
  "VALOR1" IN NUMBER, 
  "VALOR2" IN NUMBER, 
  "VALOR3" IN NUMBER DEFAULT 5, 
  "CODIGO_SALIDA" OUT VARCHAR2, 
  "MENSAJE_SALIDA" OUT VARCHAR2) 
  IS
BEGIN
   CODIGO_SALIDA := 'Ok';
   MENSAJE_SALIDA := VALOR1 + VALOR2 + VALOR3;

END; 

-------------------------------------------------------- */
using DGII_PFD.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenericDAL;
using System.IO;
using CommonTasksLib.Data;
using CommonTasksLib.Collections;
using System.Data.Common;

namespace DGII_PFD
{
    using OracleGenericDao = GenericDAO<OracleCommand, OracleConnection, OracleDataAdapter>;
    using System.Collections;
    public class DbCommand
    {
        private static OracleParameter[] ObtenerParametrosDesdeLista(string procedimiento, List<PFD_PARAMETROS> parametros, int estandarizado, out int codigoSalida, out string mensajeSalida)
        {
            var oracleParams = new List<OracleParameter>();
            
            foreach (var item in parametros)
            {
                if (item.REQUERIDO == 0)
                {
                    if (item.TIPO != (Int32)DataTypes.Objeto)
                    {
                        if (string.IsNullOrEmpty(((string[])(item.VALOR))[0]))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (((System.Web.HttpPostedFileBase[])(item.VALOR))[0] == null) {
                            continue;
                        }
                    }
                    
                }
                OracleParameter param = new OracleParameter();
                param.ParameterName = item.PARAMETRO;
                param.OracleDbType = (OracleDbType)Convert.ToInt32(item.TIPO);
                if (param.OracleDbType == OracleDbType.Varchar2)
                {
                    param.Size = 4000;
                }

                object valor = null;
                DataTypes itemType = (DataTypes)item.TIPO;
                switch (itemType)
                {
                    case DataTypes.Caracter:
                        valor = Convert.ToChar(((string[])(item.VALOR))[0]);
                        break;
                    case DataTypes.Texto:
                        valor = Convert.ToString(((string[])(item.VALOR))[0]);
                        break;
                    case DataTypes.Entero:
                        valor = Convert.ToInt32(((string[])(item.VALOR))[0]);
                        break;
                    case DataTypes.Entero_Largo:
                        valor = Convert.ToInt64(((string[])(item.VALOR))[0]);
                        break;
                    case DataTypes.Entero_Corto:
                        valor = Convert.ToInt16(((string[])(item.VALOR))[0]);
                        break;
                    case DataTypes.Decimal:
                        valor = Convert.ToDecimal(((string[])(item.VALOR))[0]);
                        break;
                    case DataTypes.Flotante:
                        valor = Convert.ToDouble(((string[])(item.VALOR))[0]);
                        break;
                    case DataTypes.Fecha:
                        valor = Convert.ToDateTime(((string[])(item.VALOR))[0]);
                        break;
                    case DataTypes.Hora:
                        valor = Convert.ToDateTime(((string[])(item.VALOR))[0]);
                        break;
                    case DataTypes.Byte:
                        valor = Convert.ToByte(((string[])(item.VALOR))[0]);
                        break;
                    case DataTypes.Objeto:
                        //valor = ((System.Web.HttpPostedFileBase[])(item.VALOR))[0];

                        //Read file to byte array

                        byte[] data;
                        using (Stream inputStream = ((System.Web.HttpPostedFileWrapper)(((System.Web.HttpPostedFileBase[])(item.VALOR))[0])).InputStream)
                        {
                            MemoryStream memoryStream = inputStream as MemoryStream;
                            if (memoryStream == null)
                            {
                                memoryStream = new MemoryStream();
                                inputStream.CopyTo(memoryStream);
                            }
                            data = memoryStream.ToArray();
                        }
                        valor = data;
                        break;
                    default:
                        valor = Convert.ToString(((string[])(item.VALOR))[0]);
                        break;
                }

                param.Value = valor;


                oracleParams.Add(param);
            }
            mensajeSalida = "";
            codigoSalida = -1;
            
            if (estandarizado == 1)
            {
                //Parametros de salida obligatorios del procedimiento......
                OracleParameter parametroResultado = new OracleParameter();
                parametroResultado.ParameterName = "codigo_salida";
                parametroResultado.OracleDbType = OracleDbType.Int32;
                parametroResultado.Direction = ParameterDirection.Output;
                //parametroResultado.Size = 4000;

                codigoSalida = 1;

                OracleParameter parametroMensaje = new OracleParameter();
                parametroMensaje.ParameterName = "mensaje_salida";
                parametroMensaje.OracleDbType = OracleDbType.Varchar2;
                parametroMensaje.Direction = ParameterDirection.Output;
                parametroMensaje.Size = 4000;

                mensajeSalida = "mensaje_salida";

                oracleParams.Add(parametroResultado);
                oracleParams.Add(parametroMensaje);
                //...................

            }
            else {
                var allParams = DbCommand.GetParametersInfo(procedimiento);

                var parametrosSalida = allParams.Where(x => x.IN_OUT == "OUT").ToList();
                bool firstFound = false;
                
               
                
                for (int i = 0; i < parametrosSalida.Count; i++)
                {
                    var item = parametrosSalida[i];
                    OracleParameter parametroSalida = new OracleParameter();
                    parametroSalida.Direction = ParameterDirection.Output;
                    parametroSalida.ParameterName = item.NOMBRE;
                    parametroSalida.OracleDbType = DbCommand.GetParameterTypeFromString(item.TIPO);

                    if (item.TIPO == "VARCHAR2")
                    {
                        parametroSalida.Size = 4000;    
                    }
                    oracleParams.Add(parametroSalida);
                    if (!firstFound && item.TIPO == "VARCHAR2")
                    {
                        mensajeSalida = item.NOMBRE;
                        firstFound = true;
                    }
                }

                if (firstFound)
                {
                    codigoSalida = 0;
                }

                //Sobreescribiendo IN IN/OUT...
                var parametrosEntrada = allParams.Where(x => x.IN_OUT == "IN/OUT").ToList();
                //var arrayParams = oracleParams.ToArray();
                for (int i = 0; i < parametrosEntrada.Count; i++)
                {
                    var item = parametrosEntrada[i];
                    oracleParams.First(x => x.ParameterName == item.NOMBRE).Direction = ParameterDirection.InputOutput;

                    
                }
            }
           
           
            
            return oracleParams.ToArray();
        }

        public static OracleDbType GetParameterTypeFromString(string p)
        {
            var type = new OracleDbType();
            p = p.ToUpper();
            switch (p)
            {
                case "VARCHAR2" :
                    type = OracleDbType.Varchar2;
                    break;
                case "INT16":
                    type = OracleDbType.Int16;
                    break;
                case "INT32":
                    type = OracleDbType.Int32;
                    break;
                case "NUMBER":
                    type = OracleDbType.Int32;
                    break;
                case "INT64":
                    type = OracleDbType.Int64;
                    break;
                case "DECIMAL":
                    type = OracleDbType.Decimal;
                    break;
                case "DOUBLE":
                    type = OracleDbType.Double;
                    break;
                case "DATE":
                    type = OracleDbType.Date;
                    break;
                case "TIMESTAMP":
                    type = OracleDbType.TimeStamp;
                    break;
                case "BYTE":
                    type = OracleDbType.Byte;
                    break;
                case "BLOB":
                    type = OracleDbType.Blob;
                    break;
                case "CHAR":
                    type = OracleDbType.Char;
                    break;
                default:
                    type = OracleDbType.Varchar2;
                    break;
            }

            return type;
        }
        protected static string GetConnectionString(string ConnectionName)
        {
            System.Configuration.Configuration rootWebConfig =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/DGII_PFD");
            System.Configuration.ConnectionStringSettings connString;
            connString = rootWebConfig.ConnectionStrings.ConnectionStrings[ConnectionName];

            return connString.ConnectionString;
        }
        public static List<Parametro> GetParametersInfo(string PROCEDIMIENTO)
        {
            
            string connection = GetConnectionString("OracleConn");
            OracleGenericDao dao = new OracleGenericDao(connection);
            dao.OpenConnection();
            Object[] prms = { PROCEDIMIENTO, null };
            Object[] dirs = { "Input", "Output" };
            string names = "P_NOMBRE_PROCEDIMIENTO,C_PARAMETERS";
            dao.FillCommand("pfd_procedimientos_internos.GET_PROCEDURE_PARAMS", prms, names, dirs);
            dao.Command.Parameters[1].OracleDbType = OracleDbType.RefCursor;
            OracleDataReader reader = dao.Command.ExecuteReader();

            var result = reader.Select(
                r => new Parametro
                {
                    NOMBRE = r["ARGUMENT_NAME"].ToString(),
                    POSICION = r["POSITION"].ToString(),
                    TIPO = r["DATA_TYPE"].ToString(),
                    IN_OUT = r["IN_OUT"].ToString()
                }).ToList();
            dao.CloseConnection();

            return result;
        }

        public static void InsertarRegistroEnLog(int idProcedimiento, int codigoResultado)
        {

            using (var db = new PFDContext())
            {
                string userName = HttpContext.Current.User.Identity.Name;
                var usuarioLogeado = db.PFD_USUARIOS.FirstOrDefault(x => x.NOMBRE_USUARIO.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
                PFD_LOG log = new PFD_LOG
                {
                    ID_PROCEDIMIENTO = idProcedimiento,
                    ID_USUARIO = usuarioLogeado.ID,
                    CODIGO_RESPUESTA = codigoResultado,
                    FECHA = DateTime.Now
                };
                db.PFD_LOG.Add(log);
                db.SaveChanges();
            }
        }

        public static MensajeResultado EjecutarProcedimiento(PFD_PROCEDIMIENTOS procedimiento)
        {
            try
            {
                int codigoResultado = 0;
                string mensajeResultado;
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["OracleConn"].ConnectionString;
                using (OracleConnection conn = new OracleConnection(connString))
                {

                    using (OracleCommand cmd = new OracleCommand())
                    {
                        int codigoParametro = -1;
                        string nombreParametroMensaje="";
                        cmd.Connection = conn;
                        cmd.CommandText = procedimiento.PROCEDIMIENTO;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.BindByName = true;

                        OracleParameter[] oracleParameters = ObtenerParametrosDesdeLista(procedimiento.PROCEDIMIENTO, procedimiento.Parametros, Convert.ToInt32(procedimiento.ESTANDARIZADO),out codigoParametro, out nombreParametroMensaje);
                        foreach (var param in oracleParameters)
                        {
                            cmd.Parameters.Add(param);
                        }

                        
                        conn.Open();

                        //Intentar ejecutar con los parametros obtenidos
                        try
                        {
                            cmd.ExecuteNonQuery();
                            if (codigoParametro == 1)
                            {
                                //Mensaje estandarizado
                                codigoResultado = Convert.ToInt32(cmd.Parameters["codigo_salida"].Value.ToString());
                                mensajeResultado = cmd.Parameters["mensaje_salida"].Value.ToString();
                            }
                            else if (codigoParametro == 0)
                            {
                                //Mensaje no estandarizado, codigo de salida neutro
                                codigoResultado = 1;
                                mensajeResultado = cmd.Parameters[nombreParametroMensaje].Value.ToString();
                            }
                            else
                            {
                               //Mensaje sin codigo de salida
                                codigoResultado = 1;
                                mensajeResultado = "Procedimiento ejecutado correctamente";
                            }
                            

                            //Registrando ejecución en el log
                            InsertarRegistroEnLog((Int32)procedimiento.ID, codigoResultado);
                        }
                        catch (Exception ex)
                        {
                            //Capturar error e insertar en log con código -1  
                            InsertarRegistroEnLog((Int32)procedimiento.ID, -1);
                            //Devolver error en un MensajeResultado
                            MensajeResultado responseError = new MensajeResultado { codigo = -1, mensaje = ex.Message };
                            return responseError;
                        }
                        

                        
                    }

                    MensajeResultado response = new MensajeResultado { codigo = codigoResultado, mensaje = mensajeResultado };
                    return response;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}