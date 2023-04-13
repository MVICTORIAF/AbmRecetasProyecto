using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.dominio
{
    class Receta
    {
        public int idReceta { get; set; }
        public string nReceta { get; set; }
        public string cheff { get; set; }
        public int tReceta { get; set; }
        public List<DetalleReceta> Detalles { get; set; }

        public Receta()
        {
            Detalles = new List<DetalleReceta>();
        }

        public void AgregarDetalle(DetalleReceta detalle)
        {
            Detalles.Add(detalle);
        }

        public void QuitarDetalle(int indice)
        {
            Detalles.RemoveAt(indice);
        }

        public double CalcularTotal()
        {
            double total = 0;
            foreach (DetalleReceta item in Detalles)
            {
                total = total + 1;
            }

            return total;
        }

		//public bool Confirmar()
		//{
			
		//	bool resultado = true;

		//	SqlConnection cnn = new SqlConnection();
		//	SqlTransaction trans = null;

  //          try
  //          {
  //              cnn.ConnectionString = @"Data Source=DESKTOP-64M431K\SQLEXPRESS;Initial Catalog=ultimaplis;Integrated Security=True";
		//		cnn.Open();
		//		trans = cnn.BeginTransaction();
		//		SqlCommand cmd = new SqlCommand();
				
		//		cmd.Transaction = trans;
		//		cmd.Connection = cnn;
		//		cmd.CommandText = "SP_INSERTAR_RECETA";
		//		cmd.CommandType = CommandType.StoredProcedure;
		//		cmd.Parameters.AddWithValue("@tipo_receta", this.tReceta);
		//		cmd.Parameters.AddWithValue("@nombre", this.nReceta);
		//		cmd.Parameters.AddWithValue("@cheff", this.cheff);
		//		SqlParameter pOut = new SqlParameter();
		//		pOut.ParameterName = "@nroreceta";
		//		pOut.DbType = DbType.Int32;
		//		pOut.Direction = ParameterDirection.Output;
		//		cmd.Parameters.Add(pOut);
		//		cmd.ExecuteNonQuery();
		//		int recetanro = (int)pOut.Value;

		//		int detalleNro = 1;

		//		foreach (DetalleReceta item in Detalles)
		//		{
		//			SqlCommand cmdDet = new SqlCommand();
		//			cmdDet.Connection = cnn;
		//			cmdDet.Transaction = trans;
		//			cmdDet.CommandText = "SP_INSERTAR_DETALLES";
		//			cmdDet.CommandType = CommandType.StoredProcedure;
		//			cmdDet.Parameters.AddWithValue("@id_receta", recetanro);
		//			cmdDet.Parameters.AddWithValue("@id_ingrediente", item.ingrediente.idIngrediente);
		//			cmdDet.Parameters.AddWithValue("@cantidad", item.Cantidad);
		//			cmdDet.ExecuteNonQuery();
					
		//		}

		//		trans.Commit();
		//		cnn.Close();
  //      }
		//	catch (Exception)
		//	{
		//		//en caso que quiera saber que error ocurre
		//		//MessageBox.Show("error: " + E.Message);
		//		trans.Rollback();
		//		resultado = false;
		//	}
		//	finally
		//	{
		//		if (cnn != null && cnn.State == ConnectionState.Open) cnn.Close();
		//	}

		//	return resultado;
		//}
	}
}
