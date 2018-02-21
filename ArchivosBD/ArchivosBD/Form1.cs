using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ArchivosBD
{
    public partial class Form1 : Form
    {
        static string CadenaConex = @"Data Source=DELL;Initial Catalog=archivosbd;Integrated Security=True";
        SqlConnection cn = new SqlConnection(CadenaConex);
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dtb;
        string ar = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                this.txtRuta.Enabled = false;
                txtTitulo.Focus();
                cmd = new SqlCommand("SELECT id, nombre FROM imagenes", cn);
                da = new SqlDataAdapter(cmd);
                dtb = new DataTable();
                da.Fill(dtb);
                dataGridView1.DataSource = dtb;
                dataGridView1.Columns[1].Width = 250;
                cn.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Limpiar()
        {
            txtRuta.Clear();
            txtTitulo.Clear();
        }

        private string GenerarNombreFichero()
        {
            int ultimoTick = 0;
            while (ultimoTick == Environment.TickCount)
            {
                System.Threading.Thread.Sleep(1);
            }
            ultimoTick = Environment.TickCount;
            return DateTime.Now.ToString("yyyyMMddhhmmss") + "." + ultimoTick.ToString();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Abrir";
            open.Filter = "Archivos Docx(*.docx)|*.docx|Archivos doc(*.doc)|*.doc|Todos los Archivos(*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                ar = open.FileName;
                this.txtRuta.Text = ar;
                txtTitulo.Text = open.SafeFileName;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtRuta.Text != "" && txtTitulo.Text != "")
                {
                    FileStream fs = new FileStream(ar, FileMode.Open);
                    //Creamos un array de bytes para almacenar los datos ledos por fs.
                    Byte[] data = new byte[fs.Length];
                    //Y guardamos los datos en el array data
                    fs.Read(data, 0, Convert.ToInt32(fs.Length));
                    if (cn.State == 0)
                    {
                        cn.Open();
                    }
                    cmd = new SqlCommand("proc_utileriaimagenes", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@doc", SqlDbType.VarBinary).Value = data;
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = this.txtTitulo.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Guardado Correctamente");
                    this.Form1_Load(null, null);
                    cn.Close();
                    fs.Close();
                    Limpiar();
                }
                else
                {
                    MessageBox.Show("Adjuntar y escribir Ttulo", "Error Guardar", MessageBoxButtons.OK);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLeer_Click(object sender, EventArgs e)
        {
            try
            {
                int i = this.dataGridView1.CurrentRow.Index;
                int cod = int.Parse(this.dataGridView1.Rows[i].Cells[0].Value.ToString());
                cmd = new SqlCommand("select doc from imagenes where id=" + cod + "", cn);
                da = new SqlDataAdapter(cmd);
                dtb = new DataTable();
                da.Fill(dtb);
                DataRow f = dtb.Rows[0];
                byte[] bits = ((byte[])(f.ItemArray[0]));
                string sFile = "tmp" + GenerarNombreFichero() + ".doc";
                FileStream fs = new FileStream(sFile, FileMode.Create);

                //Y escribimos en disco el array de bytes que conforman el fichero Word
                fs.Write(bits, 0, Convert.ToInt32(bits.Length));
                fs.Close();
                System.Diagnostics.Process obj = new System.Diagnostics.Process();
                obj.StartInfo.FileName = sFile;
                obj.Start();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }     
        }

        
    }
}
