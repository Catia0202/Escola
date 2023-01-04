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
    public partial class AdicionarTurma : Form
    {
        List<Turmas> turmas = new List<Turmas>();
        List<Turmas> ListaTurmas;
        List<Alunos> ListaAlunos;
        List<Disciplinas> ListaDisciplinas;
        Turmas Turma = new Turmas();
        BaseDeDados BaseDeDados = new BaseDeDados();

        public AdicionarTurma()
        {
            InitializeComponent();
            timer1.Start();
            ListarTurmas();
            ListaAlunos = BaseDeDados.GetDataAlunos(); //Obter todos os alunos da bd
            ListaDisciplinas = BaseDeDados.GetDataDisciplinas(); //obter todas as disciplinas da bd
            lb_data.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Num_Aleatorio();
        }

        private void bt_adicionarTurma_Click(object sender, EventArgs e) //adicionar turma
        {
            bool encontra = false;
            Turmas NovaTurma;
            if (Validaform())
            {
                foreach (DataGridViewRow row in dataGridView1.Rows) //validação de já haver uma turma com o mesmo nome
                {
                    if ( txt_NomeTurma.Text == row.Cells[1].Value.ToString())
                    {
                        encontra = true;
                        MessageBox.Show("Já existe uma turma com esse nome", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (!encontra)
                {
                    NovaTurma = new Turmas()
                    {
                        Num_Turma = int.Parse(txt_numTurma.Text),
                        Nome_Turma = txt_NomeTurma.Text,
                        cod_Disciplina = 1,
                        Descrição = txt_Descricao.Text
                    };
                    turmas.Add(NovaTurma);
                    //Turmas.DeletaTudo();
                    Turma.AdicionarTurmaBD(turmas);
                    turmas.Clear();
                    limpacampos();
                    Num_Aleatorio();
                    ListarTurmas();
                }
            }
        }

        private void limpacampos() //limpa campos
        {
            txt_NomeTurma.Text = "";
            txt_Descricao.Text = "";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lb_horas.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) //Clicar na dgc e aparecer nos campos
        {
            txt_numTurma.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txt_NomeTurma.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txt_Descricao.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            mostraralunosTurma();
            mostrarDisciplinasTurma();
        }

        private void txtpesquisa_TextChanged(object sender, EventArgs e) //pesquisar na dgv
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

        private void Num_Aleatorio() //Num aleatorio para a turma
        {
            Random rnd = new Random();
            String r = rnd.Next(1, 1000000).ToString("D6");
            txt_numTurma.Text = r.ToString();
        }

        private void ListarTurmas() //Adicionar todas as turmas na dgv
        {
            dataGridView1.Rows.Clear();
            BaseDeDados.CriarTabelas();
            ListaTurmas = BaseDeDados.GetDataTurmas();//obter todas as turmas da bd
            foreach (Turmas turmaL in ListaTurmas)
            {
                dataGridView1.Rows.Add(turmaL.Num_Turma, turmaL.Nome_Turma, turmaL.Descrição, turmaL.cod_Disciplina);

            }
        }



        private void mostraralunosTurma() //Adicionar todos os alunos da turmas na dgv
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

        private void mostrarDisciplinasTurma() //Adicionar todas as disciplinas da turmas na dgv
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
        private bool Validaform() //Validações
        {
            bool output = true;
            //Validação de campo vazio
            if ((string.IsNullOrEmpty(txt_NomeTurma.Text) || (string.IsNullOrEmpty(txt_numTurma.Text) || (string.IsNullOrEmpty(txt_Descricao.Text)))))
            {
                MessageBox.Show("Preencha todos os campos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
            }
            return output;
        }

        private void bt_limpar_Click(object sender, EventArgs e)
        {
            limpacampos();
        }

    }
}
