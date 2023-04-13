using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.dominio
{
    class DetalleReceta
    {
		public DetalleReceta(Ingrediente ingrediente, int cantidad)
		{
			this.ingrediente = ingrediente;
			Cantidad = cantidad;
		}

		public Ingrediente ingrediente { get; set; }
		public int Cantidad { get; set; }
	}
}
