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
    public partial class ImprimirDisciplinas : Form
    {
        List<Turmas> ListaTurmas;
        List<Disciplinas> ListaDisciplinas; 
        List<Professores> ListaProfessores;
        List<Disciplinas> Disciplinas = new List<Disciplinas>();
        DGVPrinter printer = new DGVPrinter();
        BaseDeDados BaseDeDados = new BaseDeDados();
        private SQLiteConnection connection;
        public ImprimirDisciplinas()
        {
            InitializeComponent();
            ListaDisciplinas = BaseDeDados.GetDataDisciplinas();  //obtem todas as disciplinas
            ListaTurmas = BaseDeDados.GetDataTurmas();  //obtem todas as turmas
            ListaProfessores = BaseDeDados.GetDataProfessores();  //obtem todos os professores
            cbo_Turma.DataSource = ListaTurmas;
            cbo_Turma.DisplayMember = "Nome_Turma";
            cbo_Turma.ValueMember = "Num_Turma";
            cbo_Turma.SelectedIndex = -1;


            cbo_disciplina.DataSource = ListaDisciplinas;
            cbo_disciplina.DisplayMember = "Nome_Disciplina";
            cbo_disciplina.ValueMember = "Num_Disciplinas";
            cbo_disciplina.SelectedIndex = -1;

            cbo_disciplina.DataSource = ListaDisciplinas;
            cbo_disciplina.DisplayMember = "Nome_Disciplina";
            cbo_disciplina.ValueMember = "Num_Disciplinas";
            cbo_disciplina.SelectedIndex = -1;

            cbo_professores.DataSource = ListaProfessores;
            cbo_professores.DisplayMember = "Primeiro_Nome";
            cbo_professores.ValueMember = "Num_Professor";
            cbo_professores.SelectedIndex = -1;
            ListarTudo();
            timer1.Start();
            lb_data.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void ListarTudo()//Adiciona tudo na dgv
        {

            foreach (Turmas turmas in ListaTurmas)
            {
                foreach (Disciplinas disciplinas in ListaDisciplinas)
                {
                     foreach(Professores professores in ListaProfessores)
                    {
                        if (turmas.Num_Turma == disciplinas.cod_Turma && disciplinas.cod_Professor == professores.Num_Professor)
                        {
                            dataGridView2.Rows.Add(disciplinas.Nome_Disciplina, turmas.Nome_Turma, disciplinas.Descrição_Disciplina, professores.Primeiro_Nome);
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) //pesquisar
        {
            dataGridView2.Rows.Clear();


            Turmas turmas;
            turmas = new Turmas()
            {
               Nome_Turma = cbo_Turma.Text
            };
            Disciplinas disciplinas;
            disciplinas = new Disciplinas()
            {
                Nome_Disciplina = cbo_disciplina.Text
            };

            Professores professores;
            professores = new Professores()
            {
                Primeiro_Nome = cbo_professores.Text
            };



            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);

            connection.Open();
            
            if ((turmas.Nome_Turma != "" && disciplinas.Nome_Disciplina == "" && professores.Primeiro_Nome == "") || (turmas.Nome_Turma == "" && disciplinas.Nome_Disciplina != "" && professores.Primeiro_Nome == "") || (turmas.Nome_Turma == "" && disciplinas.Nome_Disciplina == "" && professores.Primeiro_Nome != ""))
            {
                SQLiteCommand comm = new SQLiteCommand("SELECT Disciplinas.Nome_Disciplina, Turmas.Nome_Turma, Disciplinas.Descricao_Disciplina, Professores.PrimeiroNome FROM (Disciplinas INNER JOIN Turmas ON Disciplinas.codTurma = Turmas.NumTurma) INNER JOIN Professores ON Disciplinas.codprofessor = Professores.NumProfessor where Turmas.Nome_Turma='" + turmas.Nome_Turma + "' or Disciplinas.Nome_Disciplina='" + disciplinas.Nome_Disciplina + "' or Professores.PrimeiroNome ='" + professores.Primeiro_Nome + "'", connection);
                using (SQLiteDataReader read = comm.ExecuteReader())
                {
                    while (read.Read())
                    {
                        dataGridView2.Rows.Add(new object[]
                          {
                    read.GetValue(0),
                    read.GetValue(1),
                    read.GetValue(2),
                    read.GetValue(3)

                          });
                    }
                }
            }
            if (turmas.Nome_Turma != "" && disciplinas.Nome_Disciplina != "" && professores.Primeiro_Nome =="")
            {
                SQLiteCommand comm = new SQLiteCommand("SELECT Disciplinas.Nome_Disciplina, Turmas.Nome_Turma, Disciplinas.Descricao_Disciplina, Professores.PrimeiroNome FROM (Disciplinas INNER JOIN Turmas ON Disciplinas.codTurma = Turmas.NumTurma) INNER JOIN Professores ON Disciplinas.codprofessor = Professores.NumProfessor where Turmas.Nome_Turma='" + turmas.Nome_Turma + "'and Disciplinas.Nome_Disciplina='" + disciplinas.Nome_Disciplina + "' or Professores.PrimeiroNome ='" + professores.Primeiro_Nome + "'", connection);
                using (SQLiteDataReader read = comm.ExecuteReader())
                {
                    while (read.Read())
                    {
                        dataGridView2.Rows.Add(new object[]
                        {
                    read.GetValue(0),
                    read.GetValue(1),
                    read.GetValue(2),
                    read.GetValue(3)

                        });
                    }
                }
            }
            if (turmas.Nome_Turma == "" && disciplinas.Nome_Disciplina != "" && professores.Primeiro_Nome != "")
            {
                SQLiteCommand comm = new SQLiteCommand("SELECT Disciplinas.Nome_Disciplina, Turmas.Nome_Turma, Disciplinas.Descricao_Disciplina, Professores.PrimeiroNome FROM (Disciplinas INNER JOIN Turmas ON Disciplinas.codTurma = Turmas.NumTurma) INNER JOIN Professores ON Disciplinas.codprofessor = Professores.NumProfessor where Disciplinas.Nome_Disciplina='" + disciplinas.Nome_Disciplina + "' and Professores.PrimeiroNome ='" + professores.Primeiro_Nome + "'", connection);
                using (SQLiteDataReader read = comm.ExecuteReader())
                {
                    while (read.Read())
                    {
                        dataGridView2.Rows.Add(new object[]
                        {
                    read.GetValue(0),
                    read.GetValue(1),
                    read.GetValue(2),
                    read.GetValue(3)

                        });
                    }
                }
            }

            if (turmas.Nome_Turma != "" && disciplinas.Nome_Disciplina != "" && professores.Primeiro_Nome != "")
            {
                SQLiteCommand comm = new SQLiteCommand("SELECT Disciplinas.Nome_Disciplina, Turmas.Nome_Turma, Disciplinas.Descricao_Disciplina, Professores.PrimeiroNome FROM (Disciplinas INNER JOIN Turmas ON Disciplinas.codTurma = Turmas.NumTurma) INNER JOIN Professores ON Disciplinas.codprofessor = Professores.NumProfessor where Turmas.Nome_Turma='" + turmas.Nome_Turma + "'and Disciplinas.Nome_Disciplina='" + disciplinas.Nome_Disciplina + "' and Professores.PrimeiroNome ='" + professores.Primeiro_Nome + "'", connection);
                using (SQLiteDataReader read = comm.ExecuteReader())
                {
                    while (read.Read())
                    {
                        dataGridView2.Rows.Add(new object[]
                        {
                    read.GetValue(0),
                    read.GetValue(1),
                    read.GetValue(2),
                    read.GetValue(3)

                        });
                    }
                }
            }

        }

        private void label5_Click(object sender, EventArgs e)
        {
            chamarDisciplinas();
        }


        public void chamarDisciplinas() 
        {

            int turmaescolhida = int.Parse(cbo_Turma.SelectedValue.ToString());
            cbo_disciplina.DataSource = null;
            cbo_disciplina.Items.Clear();
            Disciplinas.Clear();




            if (turmaescolhida != 0) //mostrar na combobox quais as disciplinas que pertencem aquela turma
            {
                foreach (Disciplinas disciplinas in ListaDisciplinas)
                {

                    if (disciplinas.cod_Turma == turmaescolhida)
                    {
                        Disciplinas disciplinas1 = new Disciplinas()
                        {
                            Num_Disciplinas = disciplinas.Num_Disciplinas,
                            Nome_Disciplina = disciplinas.Nome_Disciplina
                        };

                        Disciplinas.Add(disciplinas1);

                    }

                }
                cbo_disciplina.DataSource = Disciplinas;
                cbo_disciplina.DisplayMember = "Nome_Disciplina";
                cbo_disciplina.ValueMember = "Num_Disciplinas";
            }
        }

        private void bt_updateAluno_Click(object sender, EventArgs e) //imprimir
        {
            printer.Title = "Disciplinas Escolhidas";
            printer.SubTitle = string.Format("Data:  {0}", DateTime.Now);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "foxlearn";
            printer.FooterSpacing = 15;
            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.PrintDataGridView(dataGridView2);
        }

        private void button1_Click(object sender, EventArgs e) //limpar campos
        {
            cbo_disciplina.SelectedIndex = -1;
            cbo_professores.SelectedIndex = -1;
            cbo_Turma.SelectedIndex = -1;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lb_horas.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void button3_Click(object sender, EventArgs e) //botão reset dgv
        {
            dataGridView2.Rows.Clear();
            ListarTudo();
        }
    }
}

