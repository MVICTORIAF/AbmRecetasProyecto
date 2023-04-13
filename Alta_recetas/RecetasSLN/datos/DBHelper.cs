using RecetasSLN.dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.datos
{
    class DBHelper
    {
        private SqlConnection conn;
        private string CadenaConexion = @"Data Source=DESKTOP-64M431K\SQLEXPRESS;Initial Catalog=ultimaplis;Integrated Security=True";
        private static DBHelper instancia;

        public DBHelper()
        {
            conn = new SqlConnection(CadenaConexion);
        }

        public static DBHelper ObtenerInstancia()
        {
            if (instancia == null)

                instancia = new DBHelper();
            return instancia;

        }

        public bool Confirmar(Receta oReceta)
        {

            bool resultado = true;

            SqlConnection cnn = new SqlConnection();
            SqlTransaction trans = null;

            try
            {
                cnn.ConnectionString = @"Data Source=DESKTOP-64M431K\SQLEXPRESS;Initial Catalog=ultimaplis;Integrated Security=True";
                cnn.Open();
                trans = cnn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();

                cmd.Transaction = trans;
                cmd.Connection = cnn;
                cmd.CommandText = "SP_INSERTAR_RECETA";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo_receta", oReceta.tReceta);
                cmd.Parameters.AddWithValue("@nombre", oReceta.nReceta);
                cmd.Parameters.AddWithValue("@cheff", oReceta.cheff);
                SqlParameter pOut = new SqlParameter();
                pOut.ParameterName = "@nroreceta";
                pOut.DbType = DbType.Int32;
                pOut.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pOut);
                cmd.ExecuteNonQuery();
                int recetanro = (int)pOut.Value;

                SqlCommand cmdDetalle;
                int detalleNro = 1;

                foreach (DetalleReceta item in oReceta.Detalles)
                {
                    SqlCommand cmdDet = new SqlCommand();
                    cmdDet.Connection = cnn;
                    cmdDet.Transaction = trans;
                    cmdDet.CommandText = "SP_INSERTAR_DETALLES";
                    cmdDet.CommandType = CommandType.StoredProcedure;
                    cmdDet.Parameters.AddWithValue("@id_receta", recetanro);
                    cmdDet.Parameters.AddWithValue("@id_ingrediente", item.ingrediente.idIngrediente);
                    cmdDet.Parameters.AddWithValue("@cantidad", item.Cantidad);
                    cmdDet.ExecuteNonQuery();

                }

                trans.Commit();
                cnn.Close();
            }
            catch (Exception)
            {
              
                trans.Rollback();
                resultado = false;
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open) cnn.Close();
            }

            return resultado;
        }

        public DataTable ConsultarDB(string NomProc)
        {
            DataTable tabla = new DataTable();
            SqlCommand cmd = new SqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = NomProc;
            cmd.CommandType = CommandType.StoredProcedure;
            tabla.Load(cmd.ExecuteReader());
            conn.Close();
            return tabla;
        }
    }
}
