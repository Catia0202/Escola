using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Escola.Classes;
using System.Data.SQLite;
using DGVPrinterHelper;
namespace Escola.ChildForms
{
    public partial class ImprimirProfessores : Form
    {
        private SQLiteConnection connection;

        List<Professores> ListaProfessores;
        BaseDeDados BaseDeDados = new BaseDeDados();
        DGVPrinter printer = new DGVPrinter();
        public ImprimirProfessores()
        {
            InitializeComponent();
            ListarProfessores();
            timer1.Start();
            lb_data.Text = DateTime.Now.ToString("dd/MM/yyyy");

        }

        private void ListarProfessores() //adiciona na dgv todos os professores
        {
            dataGridView1.Rows.Clear();
            ListaProfessores = BaseDeDados.GetDataProfessores(); //obtem todos os professores da db
           

            foreach (Professores professoresL in ListaProfessores)
            {
              dataGridView1.Rows.Add( professoresL.Primeiro_Nome, professoresL.Ultimo_Nome, professoresL.Genero);   
            }
        }

        private void button2_Click(object sender, EventArgs e) //pesquisa na base de dados
        {
            dataGridView1.Rows.Clear();


            Professores professores;
            professores = new Professores()
            {
                Primeiro_Nome = txtProfessor.Text,
                Genero = cbo_genero.Text
                
            };
         


            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);

            connection.Open();
            if (professores.Primeiro_Nome == "" ||professores.Genero=="")
            {
                SQLiteCommand comm = new SQLiteCommand("SELECT Professores.PrimeiroNome, Professores.UltNome, Professores.Genero FROM Professores  where  Professores.PrimeiroNome = '" + professores.Primeiro_Nome + "'or Professores.Genero='"+professores.Genero +"'", connection);
                using (SQLiteDataReader read = comm.ExecuteReader())
                {
                    while (read.Read())
                    {
                        dataGridView1.Rows.Add(new object[]
                          {
                    read.GetValue(0),
                    read.GetValue(1),
                    read.GetValue(2)
                          });
                    }
                }
            }

        }

        private void bt_updateAluno_Click(object sender, EventArgs e) //imprimir
        {
            printer.Title = "Professores Escolhidos";
            printer.SubTitle = string.Format("Data:  {0}", DateTime.Now);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "foxlearn";
            printer.FooterSpacing = 15;
            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.PrintDataGridView(dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtProfessor.Text = "";
            cbo_genero.SelectedIndex = -1;
        }



        private void timer1_Tick_1(object sender, EventArgs e)
        {
            lb_horas.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ListarProfessores();
        }
    }
}
