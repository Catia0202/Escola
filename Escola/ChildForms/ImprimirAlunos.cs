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
    public partial class ImprimirAlunos : Form
    {
        List<Alunos> ListaAlunos;
        List<Turmas> ListaTurmas;
        DGVPrinter printer = new DGVPrinter();
        BaseDeDados BaseDeDados = new BaseDeDados();
        private SQLiteConnection connection;
        public ImprimirAlunos()
        {
            InitializeComponent();
            ListaAlunos = BaseDeDados.GetDataAlunos(); //obtem todos os alunos
            ListaTurmas = BaseDeDados.GetDataTurmas();//obtem todas as turmas
            cbo_turmas.DataSource = ListaTurmas;
            cbo_turmas.DisplayMember = "Nome_Turma";
            cbo_turmas.ValueMember = "Num_Turma";
            cbo_turmas.SelectedIndex = -1;
            ListarAlunos();
            timer1.Start();
            lb_data.Text = DateTime.Now.ToString("dd/MM/yyyy");

        }


      

        private void bt_PEsquisar_Click(object sender, EventArgs e) //pesquisar
        {
            dataGridView1.Rows.Clear();
            Turmas turmas;
            turmas = new Turmas()
            {
                Nome_Turma = cbo_turmas.Text
            };
            Alunos alunos;
            alunos = new Alunos()
            {
                Genero = cbo_genero.Text
            };

            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            //pesquisar na base de dados e depois adicionar na datagrid os valores que foram encontrados na pesquisa
            connection.Open();
            //caso algum dos campos estiver vazio
            if ((turmas.Nome_Turma == "" && alunos.Genero == "") || (turmas.Nome_Turma != "" && alunos.Genero == "") || (turmas.Nome_Turma == "" && alunos.Genero != ""))
            {
                dataGridView1.Rows.Clear();
                SQLiteCommand comm = new SQLiteCommand("SELECT NomeCompleto, Email, telemovel,genero,dataNascimento,morada,Turmas.Nome_Turma FROM (Alunos INNER JOIN Turmas ON Alunos.codTurma = Turmas.NumTurma) where Turmas.Nome_Turma='" + turmas.Nome_Turma +  "' or Alunos.genero ='" + alunos.Genero + "'", connection);
                using (SQLiteDataReader read = comm.ExecuteReader())
                {
                    while (read.Read())
                    {
                        dataGridView1.Rows.Add(new object[]
                          {
                    read.GetValue(0),
                    read.GetValue(1),
                    read.GetValue(2),
                    read.GetValue(3),
                    read.GetValue(4),
                    read.GetValue(5),
                    read.GetValue(6)
                          });
                    }
                }
            }
            //caso apenas o genero estiver vazio
            if (turmas.Nome_Turma != "" && alunos.Genero == "")
            {
                dataGridView1.Rows.Clear();
                SQLiteCommand comm = new SQLiteCommand("SELECT NomeCompleto, Email, telemovel,genero,dataNascimento,morada,Turmas.Nome_Turma FROM (Alunos INNER JOIN Turmas ON Alunos.codTurma = Turmas.NumTurma) where Turmas.Nome_Turma='" + turmas.Nome_Turma + "'", connection);
                using (SQLiteDataReader read = comm.ExecuteReader())
                {
                    while (read.Read())
                    {
                        dataGridView1.Rows.Add(new object[]
                        {
               read.GetValue(0),
                    read.GetValue(1),
                    read.GetValue(2),
                    read.GetValue(3),
                    read.GetValue(4),
                    read.GetValue(5),
                    read.GetValue(6)
                 

                        });
                    }
                }
            }
            //caso ambos estejam preenchidos
            if ( turmas.Nome_Turma != "" && alunos.Genero != "")
            {
                dataGridView1.Rows.Clear();
                SQLiteCommand comm = new SQLiteCommand("SELECT NomeCompleto, Email, telemovel,genero,dataNascimento,morada,Turmas.Nome_Turma FROM (Alunos INNER JOIN Turmas ON Alunos.codTurma = Turmas.NumTurma) where Turmas.Nome_Turma ='" + turmas.Nome_Turma + "'and Alunos.genero ='" + alunos.Genero + "'", connection);
                using (SQLiteDataReader read = comm.ExecuteReader())
                {
                    while (read.Read())
                    {
                        dataGridView1.Rows.Add(new object[]
                        {
                     read.GetValue(0),
                    read.GetValue(1),
                    read.GetValue(2),
                    read.GetValue(3),
                    read.GetValue(4),
                    read.GetValue(5),
                    read.GetValue(6)
                        });
                    }
                }
            }
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lb_horas.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void bt_imprimir_Click(object sender, EventArgs e) //imrpimir (com a classe DGVPrinter)
        {
            printer.Title = "Alunos Escolhidas";
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

        private void button2_Click(object sender, EventArgs e)
        {
            ListarAlunos();
        }

        private void button1_Click(object sender, EventArgs e) //limpar campos
        {
            cbo_genero.SelectedIndex = -1;
            cbo_turmas.SelectedIndex = -1;
        }
        public void ListarAlunos() //adicionar todos os alunos na dgv
        {
            dataGridView1.Rows.Clear();
            BaseDeDados.CriarTabelas();
            foreach (Alunos alunoL in ListaAlunos)
            {
                foreach (Turmas turmasl in ListaTurmas)
                {
                    if (alunoL.Cod_Turma == turmasl.Num_Turma)
                    {
                        string path = @"" + alunoL.imagem;
                        dataGridView1.Rows.Add(alunoL.NomeCompleto, alunoL.Email, alunoL.Telemovel, alunoL.Genero, alunoL.Data_Nascimento, alunoL.morada, turmasl.Nome_Turma);
                    }
                }

            }

        }
    }
}
