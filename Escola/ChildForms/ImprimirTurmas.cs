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
    public partial class ImprimirTurmas : Form
    {
        List<Turmas> ListaTurmas;
        DGVPrinter printer = new DGVPrinter();
        BaseDeDados BaseDeDados = new BaseDeDados();
        private SQLiteConnection connection;
        public ImprimirTurmas()
        {
            InitializeComponent();
          

            ListaTurmas = BaseDeDados.GetDataTurmas(); //obtem todas as turmas da db
            cbo_turmas.DataSource = ListaTurmas;
            cbo_turmas.DisplayMember = "Nome_Turma";
            cbo_turmas.ValueMember = "Num_Turma";
            cbo_turmas.SelectedIndex = -1;
            ListarTurmas();
            

        }

        private void ListarTurmas() //adiciona todas as turmas à dgv
        {
            timer1.Start();
            lb_data.Text = DateTime.Now.ToString("dd/MM/yyyy");
            dataGridView1.Rows.Clear();
            foreach (Turmas turmas in ListaTurmas)
            {
               dataGridView1.Rows.Add( turmas.Nome_Turma, turmas.Descrição);
            }
        }

        private void button2_Click(object sender, EventArgs e) //pesquina na base de dados
        {
            dataGridView1.Rows.Clear();


            Turmas turmas;
            turmas = new Turmas()
            {
                Nome_Turma = cbo_turmas.Text
            };

            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);

            connection.Open();
            dataGridView1.Rows.Clear();
                SQLiteCommand comm = new SQLiteCommand("SELECT Nome_Turma,descrição FROM Turmas where Turmas.Nome_Turma='" + turmas.Nome_Turma  + "'", connection);
                using (SQLiteDataReader read = comm.ExecuteReader())
                {
                    while (read.Read())
                    {
                        dataGridView1.Rows.Add(new object[]
                          {
                    read.GetValue(0),
                    read.GetValue(1)
                  

                          });
                    }
                }
            }

        private void button1_Click(object sender, EventArgs e)
        {
            cbo_turmas.SelectedIndex = -1;
        }


        private void bt_updateAluno_Click(object sender, EventArgs e) //imprimir
        {
            printer.Title = "Turmas Escolhidas";
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

        private void timer1_Tick(object sender, EventArgs e)
        {
          
                lb_horas.Text = DateTime.Now.ToString("HH:mm:ss");
        

        }

        private void button3_Click(object sender, EventArgs e) //Reset
        {
            ListarTurmas();
        }
    }
    
}
