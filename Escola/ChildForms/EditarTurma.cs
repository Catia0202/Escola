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
namespace Escola.ChildForms
{
    public partial class EditarTurma : Form
    {
       
        List<Turmas> ListaTurmas;
        List<Alunos> ListaAlunos;
   
        List<Disciplinas> ListaDisciplinas;
        Turmas Turma = new Turmas();
        BaseDeDados BaseDeDados = new BaseDeDados();



        public EditarTurma()
        {
            InitializeComponent();
            timer1.Start();
            ListarTurmas();
            ListaAlunos = BaseDeDados.GetDataAlunos();
            ListaDisciplinas = BaseDeDados.GetDataDisciplinas();
            lb_data.Text = DateTime.Now.ToString("dd/MM/yyyy");


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)//Clicar dgv e aparecer as inf's
        {
            txt_numTurma.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txt_NomeTurma.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txt_Descricao.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            mostraralunosTurma();
            mostrarDisciplinasTurma();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lb_horas.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void bt_updateTurma_Click(object sender, EventArgs e) //edita turma
        {
            Turmas TurmaAEditar;
            if (Validaform())
            {
                    TurmaAEditar = new Turmas()
                    {
                        Num_Turma = int.Parse(txt_numTurma.Text),
                        Nome_Turma = txt_NomeTurma.Text,
                        cod_Disciplina = 1,
                        Descrição = txt_Descricao.Text
                    };
                    Turma.UpdateTurmaBD(TurmaAEditar);

                    ListarTurmas();
                    limpacampos();
            }
        }

        private bool Validaform() //validações
        {
            //validar campos vazios
            bool output = true;
            if ((string.IsNullOrEmpty(txt_NomeTurma.Text) || (string.IsNullOrEmpty(txt_numTurma.Text) || (string.IsNullOrEmpty(txt_Descricao.Text)))))
            {
                MessageBox.Show("Preencha todos os campos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
            }
            return output;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            limpacampos();
        }

        private void limpacampos() //limpa campos
        {
            txt_NomeTurma.Text = "";
            txt_numTurma.Text = "";
            txt_Descricao.Text = "";
        }

        private void txtpesquisa_TextChanged(object sender, EventArgs e) //pesquisa na dgv
        {
            string searchValue = txtpesquisa.Text;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[1].Value.ToString().Contains(searchValue) || row.Cells[2].Value.ToString().Contains(searchValue))
                    {
                        dataGridView1.Refresh();
                        dataGridView1.Rows.RemoveAt(row.Index);
                        dataGridView1.Rows.Insert(row.Index + 1, row);
                        row.Selected = true;
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e) //apaga turma
        {
            Turmas TurmaADeletar;
            TurmaADeletar = new Turmas()
            {
                Num_Turma = int.Parse(txt_numTurma.Text)
            };
            TurmaADeletar.DeleteTurmaBD(TurmaADeletar);
            ListarTurmas();
            limpacampos();
        }
        private void ListarTurmas() //lista todas as turmas
        {
            dataGridView1.Rows.Clear();
            BaseDeDados.CriarTabelas();
            ListaTurmas = BaseDeDados.GetDataTurmas();
            foreach (Turmas turmaL in ListaTurmas)
            {
                dataGridView1.Rows.Add(turmaL.Num_Turma, turmaL.Nome_Turma, turmaL.Descrição, turmaL.cod_Disciplina);
            }
        }
        private void mostraralunosTurma() //lista todos os alunos que pertencem à turma
        {
            dataGridView2.Rows.Clear();
            int turmaselecionada = int.Parse(txt_numTurma.Text);

            foreach (Turmas turmaL in ListaTurmas)
            {
                foreach (Alunos alunoL in ListaAlunos)
                {

                    if (turmaL.Num_Turma == turmaselecionada)
                    {
                        if (turmaL.Num_Turma == alunoL.Cod_Turma)
                        {
                            dataGridView2.Rows.Add(alunoL.NomeCompleto);
                        }
                    }
                }



            }
        }


        private void mostrarDisciplinasTurma() //lista todas as disciplinas que pertencem à turma
        {
            dataGridView3.Rows.Clear();
            int turmaselecionada = int.Parse(txt_numTurma.Text);

            foreach (Turmas turmaL in ListaTurmas)
            {
                foreach (Disciplinas disciplinasL in ListaDisciplinas)
                {

                    if (turmaL.Num_Turma == turmaselecionada)
                    {
                        if (turmaL.Num_Turma == disciplinasL.cod_Turma)
                        {
                            dataGridView3.Rows.Add(disciplinasL.Nome_Disciplina);
                        }
                    }
                }
            }
        }
    }
}
