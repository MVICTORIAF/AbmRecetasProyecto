using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.dominio
{
    class Ingrediente
    {
        public Ingrediente(int idIngrediente, string nIngrediente, string unidad_medida)
        {
            this.idIngrediente = idIngrediente;
            NIngrediente = nIngrediente;
            this.unidad_medida = unidad_medida;
        }

        public int idIngrediente { get; set; }
        public string NIngrediente { get; set; }
        public string unidad_medida { get; set; }

    }
}
