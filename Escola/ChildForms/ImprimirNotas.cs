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

    public partial class ImprimirNotas : Form
    {
        private SQLiteConnection connection;
        
        List<Alunos> ListaAlunos;
        List<Turmas> ListaTurmas;
        List<Disciplinas> ListaDisciplinas;
        List<Notas> ListaNotas;
        List<Disciplinas> Disciplinas = new List<Disciplinas>();
        BaseDeDados BaseDeDados = new BaseDeDados();
        DGVPrinter printer = new DGVPrinter();
        public ImprimirNotas()
        {

            ListaAlunos = BaseDeDados.GetDataAlunos();//obtem todos os alunos
            ListaTurmas = BaseDeDados.GetDataTurmas(); //obtem todos as turmas
            ListaDisciplinas = BaseDeDados.GetDataDisciplinas();//obtem todas as disciplinas
            ListaNotas = BaseDeDados.GetDataNotas();//obtem todas as notas

            InitializeComponent();
            cbo_Turma.DataSource = ListaTurmas;
            cbo_Turma.DisplayMember = "Nome_Turma";
            cbo_Turma.ValueMember = "Num_Turma";
            cbo_Turma.SelectedIndex = -1;
        
            ListarTudo();
            timer1.Start();
            lb_data.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void cbo_Turma_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void chamarDisciplinas() //chama as disciplinas
        {

            int turmaescolhida = int.Parse(cbo_Turma.SelectedValue.ToString());
            cbo_disciplina.DataSource = null;
            cbo_disciplina.Items.Clear();
            Disciplinas.Clear();




            if (turmaescolhida != 0)//mostrar na combobox quais as disciplinas que pertencem aquela turma
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

        private void ListarTudo() //Adiciona tudo na dgv
        {
            dataGridView2.Rows.Clear();
            foreach (Alunos alunoL in ListaAlunos)
            {
                foreach (Turmas turmas in ListaTurmas)
                {
                    foreach (Disciplinas disciplinas in ListaDisciplinas)
                    {
                        foreach (Notas notas in ListaNotas)
                        {
                            if (notas.Id_Aluno == alunoL.Num_Aluno && alunoL.Cod_Turma == turmas.Num_Turma && turmas.Num_Turma == disciplinas.cod_Turma && notas.Id_Disciplina == disciplinas.Num_Disciplinas)
                            {
                                dataGridView2.Rows.Add(alunoL.NomeCompleto, turmas.Nome_Turma, disciplinas.Nome_Disciplina, notas.Nota, notas.Anotações, alunoL.Num_Aluno, turmas.Num_Turma, notas.Num_Nota, disciplinas.Num_Disciplinas);
                            }
                        }
                    }
                }
            }


        }

        private void cbo_Turma_SelectedValueChanged(object sender, EventArgs e)
        {


        }



        private void label5_Click(object sender, EventArgs e)
        {
            chamarDisciplinas();
        }

        private void button2_Click(object sender, EventArgs e) //pesquisa na base de dados
        {
            dataGridView2.Rows.Clear();
            

     
            Disciplinas disciplinas;
            disciplinas = new Disciplinas()
            {
                Nome_Disciplina = cbo_disciplina.Text
            };
            Turmas turmas;
            turmas = new Turmas()
            {
                Nome_Turma = cbo_Turma.Text
            };

            
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            
            connection.Open();
            if (turmas.Nome_Turma == "" || disciplinas.Nome_Disciplina == "")
            {
                SQLiteCommand comm = new SQLiteCommand("SELECT Alunos.NomeCompleto, Turmas.Nome_Turma,Disciplinas.Nome_Disciplina,  Notas.nota, Notas.anotacoes FROM ((Alunos INNER JOIN Turmas ON Alunos.codTurma = Turmas.NumTurma) INNER JOIN Disciplinas On Disciplinas.codTurma = Turmas.NumTurma) INNER JOIN Notas On Notas.id_disciplina = Disciplinas.NumDisciplina and  Notas.id_aluno = Alunos.NumAluno where  Turmas.Nome_Turma='" + turmas.Nome_Turma + "' or Disciplinas.Nome_Disciplina='" + disciplinas.Nome_Disciplina + "'", connection);
                using (SQLiteDataReader read = comm.ExecuteReader())
                {
                    while (read.Read())
                    {
                      dataGridView2.Rows.Add(new object[]
                        {
                    read.GetValue(0),
                    read.GetValue(1),
                    read.GetValue(2),
                    read.GetValue(3),
                    read.GetValue(4)

                        });
                    }
                }
            }
            if ( turmas.Nome_Turma != "" && disciplinas.Nome_Disciplina != "")
            {
                SQLiteCommand comm = new SQLiteCommand("SELECT Alunos.NomeCompleto, Turmas.Nome_Turma,Disciplinas.Nome_Disciplina,  Notas.nota, Notas.anotacoes FROM ((Alunos INNER JOIN Turmas ON Alunos.codTurma = Turmas.NumTurma) INNER JOIN Disciplinas On Disciplinas.codTurma = Turmas.NumTurma) INNER JOIN Notas On Notas.id_disciplina = Disciplinas.NumDisciplina and  Notas.id_aluno = Alunos.NumAluno where  Turmas.Nome_Turma='" + turmas.Nome_Turma + "' and Disciplinas.Nome_Disciplina='" + disciplinas.Nome_Disciplina + "'", connection);
                using (SQLiteDataReader read = comm.ExecuteReader())
                {
                    while (read.Read())
                    {
                        dataGridView2.Rows.Add(new object[]
                        {
                    read.GetValue(0),
                    read.GetValue(1),
                    read.GetValue(2),
                    read.GetValue(3),
                    read.GetValue(4)

                        });
                    }
                }
            }



        }

        private void cbo_disciplina_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbo_disciplina.SelectedItem != null)
            {

                lb_valuecombo.Text = "";
                lb_valuecombo.Text = cbo_disciplina.SelectedValue.ToString();

            }
        }

        private void bt_updateAluno_Click(object sender, EventArgs e) //imprimir
        {
            printer.Title = "Notas das Turmas ou Disciplinas escolhidos";
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

        private void button1_Click(object sender, EventArgs e)
        {
            cbo_Turma.SelectedIndex = -1;
            
            cbo_disciplina.SelectedIndex = -1;
        }


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            lb_horas.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ListarTudo();
        }
    }
}
