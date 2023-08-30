using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;


namespace Presentacion
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo=null;
        private OpenFileDialog archivo = null;
        public frmAltaArticulo()
        {
            InitializeComponent();
        }
        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }       

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Articulo articulo = new Articulo();
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                articulo.Codigo=(txtCodigo.Text);
                articulo.Nombre=txtNombre.Text;
                articulo.Descripcion=txtDescripcion.Text;
                articulo.Tipo=(Categoria)cboCategoria.SelectedItem;
                articulo.Marca=(Marca)cboMarca.SelectedItem;
                articulo.ImagenUrl=txtImagen.Text;

                negocio.agregar(articulo);
                MessageBox.Show("Agregado exitosamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
              CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
              MarcaNegocio marcaNegocio = new MarcaNegocio();
            try
            {
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboMarca.DataSource=marcaNegocio.listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
