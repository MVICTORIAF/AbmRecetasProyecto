
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RecetasSLN.datos;
using RecetasSLN.dominio;

namespace RecetasSLN
{
    public partial class Frm_Alta : Form
    {
        private Receta nuevaReceta;
        private DBHelper gestor;

        public Frm_Alta()
        {
            InitializeComponent();
            nuevaReceta = new Receta();
            gestor = new DBHelper();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

            if (txtNombre.Text == "")
            {
                MessageBox.Show("Debe especificar un nombre para la receta.", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNombre.Focus();
                return;
            }
            if (txtCheff.Text == "")
            {
                MessageBox.Show("Debe especificar un cheff.", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNombre.Focus();
                return;
            }
            if (dgvDetalles.Rows.Count < 3)
            {
                MessageBox.Show("Ha olvidado ingredientes? 3 ingredientes minimo.", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboIngrediente.Focus();
                return;
            }

            LimpiarCampos();
            GuardarReceta();
            lblNroReceta.Text = Convert.ToString(ProximaReceta());

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();

            }
            else
            {
                return;
            }
        }

        private void Frm_Alta_Presupuesto_Load(object sender, EventArgs e)
        {
            CargarCombo();
            lblNroReceta.Text = Convert.ToString(ProximaReceta());
            
            
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboTipo.Text.Equals(string.Empty))
            {
                MessageBox.Show("Debe seleccionar un tipo de receta", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (cboIngrediente.Text.Equals(string.Empty))
            {
                MessageBox.Show("Debe seleccionar un ingrdiente", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //no puede haber mas de un ingrediente a la vez 
            foreach (DataGridViewRow row in dgvDetalles.Rows)
            {
                if (row.Cells["Ingrediente"].Value.ToString().Equals(cboIngrediente.Text))
                {
                    MessageBox.Show("Este ingrediente ya se encuentra en la receta", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            DataRowView item = (DataRowView)cboIngrediente.SelectedItem;

            int ing = Convert.ToInt32(item.Row.ItemArray[0]); //columna 1
            string nom = item.Row.ItemArray[1].ToString();
            string uni;
            if (rbtCm.Checked)
            {
                uni = "cm3";
            }
            else
            {
                uni = "gramos";
            }
            int cant = Convert.ToInt32(nudCantidad.Text);


            Ingrediente i = new Ingrediente(ing, nom, uni );
            DetalleReceta detalle = new DetalleReceta(i, cant);

            nuevaReceta.AgregarDetalle(detalle);
            dgvDetalles.Rows.Add(new object[] { ing, nom, cant, uni });
            lblTotal.Text = Convert.ToString(nuevaReceta.CalcularTotal());

        }

        private void LimpiarCampos()
        {
            cboTipo.SelectedIndex = -1;
            cboIngrediente.SelectedIndex = -1;
            nudCantidad.Value = 1;
            txtCheff.Clear();
            txtNombre.Clear();
            lblTotal.Text = "";
            
        }

        //private bool ExisteProductoEnGrilla(string text)//no se que es esto
        //{
        //    foreach (DataGridViewRow fila in dgvDetalles.Rows)
        //    {
        //        if (fila.Cells["ingrediente"].Value.Equals(text))
        //            return true;
        //    }
        //    return false;
        //}

        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles.CurrentCell.ColumnIndex == 4)
            {
                nuevaReceta.QuitarDetalle(dgvDetalles.CurrentRow.Index);
                dgvDetalles.Rows.Remove(dgvDetalles.CurrentRow);
                lblTotal.Text=Convert.ToString(nuevaReceta.CalcularTotal());
            }
        }

        private void CargarCombo()
        {
            cboIngrediente.DataSource = gestor.ConsultarDB("SP_CONSULTAR_INGREDIENTES");
            cboIngrediente.ValueMember = "id_ingrediente";
            cboIngrediente.DisplayMember = "n_ingrediente";
        }


        private int ProximaReceta()
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = @"Data Source=DESKTOP-64M431K\SQLEXPRESS;Initial Catalog=ultimaplis;Integrated Security=True";
            cnn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_PROXIMO_ID";
            SqlParameter param = new SqlParameter("@next", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);
            cmd.ExecuteNonQuery();
            //como no tenia nada cargado 
            int proximoId; 
            if (param.Value == DBNull.Value)
            {
                proximoId = 1;
            }
            else
            {
                proximoId = Convert.ToInt32(param.Value);
            }
            cnn.Close();

            return proximoId;
        }
        private void GuardarReceta()
        {

         nuevaReceta.nReceta = txtNombre.Text;
         nuevaReceta.cheff = txtCheff.Text;
         nuevaReceta.tReceta = cboTipo.SelectedIndex;
            
        if (gestor.Confirmar(nuevaReceta))
        {
            MessageBox.Show("Receta registrado con exito.", "Informe", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }
        else
        {
            MessageBox.Show("ERROR. No se pudo registrar la receta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        }
    }
}
