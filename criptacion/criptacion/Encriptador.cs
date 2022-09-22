using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace criptacion
{
    public partial class Encriptador : Form
    {
        public Dictionary<int, int> cifrador;

        public Encriptador()
        {
            InitializeComponent();
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }
        private String generarFichero()
        {
            string path = "";
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text Files | *.txt";
            saveFileDialog1.DefaultExt = "txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog1.FileName;
                label5.Text = "Se ha cargado el cifrador '" + Path.GetFileName(path) + "'";
            }

            return path;
        }
        public String cargarFichero()
        {
            string path = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
                label5.Text = "Se ha cargado el cifrador '" +Path.GetFileName(path)+ "'";
            }
            return path;
        }
        public void generarCifrador()
        {
            
            try
            {
                StreamWriter sw = new StreamWriter(generarFichero());
                shuffle();
                for (int i = 32; i < 127; i++)
                {
                    string line = "";
                    for (int j = 0; j < 15; j++)
                    {
                        Random rnd = new Random();
                        line += rnd.Next(0, 10);
                    }
                    if (cifrador[i] < 100) line += "0";
                    line += cifrador[i];
                    for (int j = 18; j < 30; j++)
                    {
                        Random rnd = new Random();
                        line += rnd.Next(0, 10);
                    }
                    sw.Write(line);
                    sw.Write("\n");

                }
                sw.Close();
            }

            catch (Exception ex)
            {
                label5.Text = "No se ha podido cargar el cifrador."+ Environment.NewLine
                    +"1.-Genera un cifrador txt"+Environment.NewLine
                    +"2.-Carga el cifrador generado";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            generarCifrador();
            textBox2.Text = actuacion(true);
            textBox4.Text = actuacion(false);

        }

        private void Encriptador_Load(object sender, EventArgs e)
        {
            this.cifrador = new Dictionary<int, int>();
            iniciarParametros();
            label5.Text = "No hay ningún cifrador activo."+Environment.NewLine+"Debes generar/cargar un cifrador";
            

        }
        public void iniciarParametros()
        {
            for (int i = 32; i < 127; i++)
            {
                cifrador.Add(i, i);
            }
            

        }

        public void shuffle()
        {
            int n = 126;
            Random random = new Random();

            for (int i = 32; i < n; i++)
            {
                int change = i + random.Next(n - i);
                swap(i, change);
            }
        }
        private void swap(int i, int change)
        {
            int helper = this.cifrador[i];
            this.cifrador[i] = this.cifrador[change];
            this.cifrador[change] = helper;
        }
        private void CargarCifrador_Click(object sender, EventArgs e)
        {
            cargarCifrador();
            textBox2.Text = actuacion(true);
            textBox4.Text = actuacion(false);

        }

        private void cargarCifrador()
        {
            try
            {
                String line = "";
                StreamReader sr = new StreamReader(cargarFichero());

                int i = 32;
                while (true)
                {
                    line = sr.ReadLine();
                    if (line == null) break;
                    cifrador[i] = int.Parse(line[15] + "") * 100 + int.Parse(line[16] + "") * 10 + int.Parse(line[17] + "");
                    i++;
                }
                sr.Close();
            }
            catch (Exception e)
            {
                label5.Text = "No se ha podido cargar el cifrador." + Environment.NewLine
                    + "1.-Genera un cifrador txt" + Environment.NewLine
                    + "2.-Carga el cifrador generado";
            }
        }

        public string actuacion(bool encriptar)
        {
            String texto = "";
            if (encriptar)
            {
                TextBox tb = textBox1;
                for (int i = 0; i < tb.Text.Length; i++)
                {
                    int value = (int)tb.Text[i];
                    try
                    {
                        texto += (char)cifrador[value] + "";
                    }
                    catch (Exception e)
                    {

                        //texto += tb.Text.Substring(i,1);
                        texto +=(char)tb.Text[i];
                    }

                }


            }
            else
            {
                TextBox tb = textBox3;
                for (int i = 0; i < tb.Text.Length; i++)
                {
                    
                    var keyValue=this.cifrador.FirstOrDefault(x => x.Value == (int)tb.Text[i]).Key;
                    if (keyValue== 0) texto += tb.Text.Substring(i, 1);
                    else texto += (char)keyValue + "";

                    //texto += tb.Text.Substring(i,1);



                }
            }
            return texto;
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox2.Text = actuacion(true);

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox4.Text = "";
            textBox4.Text = actuacion(false);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
