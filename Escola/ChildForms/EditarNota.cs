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
    public partial class EditarNota : Form
    {
        List<Alunos> ListaAlunos;
        List<Turmas> ListaTurmas;
        List<Disciplinas> ListaDisciplinas;
        List<Disciplinas> Disciplinas = new List<Disciplinas>();
        BaseDeDados BaseDeDados = new BaseDeDados();
        List<Notas> ListaNotas;
      
        Notas NotaAEditar = new Notas();
        public EditarNota()
        {
            InitializeComponent();
            ListaAlunos = BaseDeDados.GetDataAlunos(); //recebe todos os alunos da db
            ListaTurmas = BaseDeDados.GetDataTurmas(); //recebe todas as turmas da db
            ListaDisciplinas = BaseDeDados.GetDataDisciplinas(); //recebe todas  as disciplinas da db
            ListarTudo();
            timer1.Start();
            lb_data.Text = DateTime.Now.ToString("dd/MM/yyyy");

        }

        private void ListarTudo() //adiciona todas as inofrmações na dgv
        {
            dataGridView1.Rows.Clear();
            BaseDeDados.CriarTabelas();
            foreach (Alunos alunos in ListaAlunos)
            {
                foreach (Turmas turmas in ListaTurmas)
                {
                    if (alunos.Cod_Turma == turmas.Num_Turma)
                    {
                        dataGridView1.Rows.Add(alunos.NomeCompleto, turmas.Nome_Turma, alunos.Num_Aluno, turmas.Num_Turma);
                    }
                }
            }
        }

        private void mostrarNotasAlunos() //Adiciona todos os alunos na dgv
        {
            dataGridView2.Rows.Clear();
            ListaNotas = BaseDeDados.GetDataNotas();
            int alunoselecionado = int.Parse(lb_Codaluno.Text);

            foreach (Notas notas in ListaNotas)
            {
                foreach (Alunos alunoL in ListaAlunos)
                {
                    foreach (Disciplinas disciplinas in ListaDisciplinas)
                    {

                        if (notas.Id_Aluno == alunoselecionado)
                        {
                            if (notas.Id_Aluno == alunoL.Num_Aluno && notas.Id_Disciplina == disciplinas.Num_Disciplinas)
                            {
                                dataGridView2.Rows.Add(disciplinas.Nome_Disciplina, notas.Nota, notas.Anotações, notas.Num_Nota, disciplinas.Num_Disciplinas);
                            }
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) //clicar na dgv e mostrar as inf's 
        {
         
            txt_PrimeiroNome.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txt_turma.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            lb_Codaluno.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            lb_codTurma.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            comboBox1.DataSource = null;
            comboBox1.Items.Clear();
            Disciplinas.Clear();
            int turmaescolhida = int.Parse(lb_codTurma.Text);
            if (turmaescolhida != 0)
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
                comboBox1.DataSource = Disciplinas;
                comboBox1.DisplayMember = "Nome_Disciplina";
                comboBox1.ValueMember = "Num_Disciplinas";
            }

            mostrarNotasAlunos();

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)//clicar na dgv e mostrar as inf's 
        {
          
            comboBox1.Text = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
            txt_Nota.Text = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
            txt_observacao.Text = dataGridView2.SelectedRows[0].Cells[2].Value.ToString();
            txt_NumNota.Text = dataGridView2.SelectedRows[0].Cells[3].Value.ToString();
            lb_codDisciplina.Text = dataGridView2.SelectedRows[0].Cells[4].Value.ToString();
            comboBox1.Enabled = false;
        }

        private void bt_updateNotas_Click(object sender, EventArgs e) //Editar nota
        {
            Notas NotaEditar;
            if (Validaform())
            {
                NotaEditar = new Notas()
                {
                    Num_Nota = int.Parse(txt_NumNota.Text),
                    Nota = double.Parse(txt_Nota.Text),
                    Anotações = txt_observacao.Text,
                    Id_Aluno = int.Parse(lb_Codaluno.Text),
                    Id_Disciplina = int.Parse(lb_codDisciplina.Text)

                };


                NotaAEditar.UpdateNotasBD(NotaEditar);


                mostrarNotasAlunos();
                limpacampos();
            }
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
      
        }

        private bool Validaform() //Validações
        {
            //validação de campos vazios
            bool output = true;
            if ((string.IsNullOrEmpty(txt_NumNota.Text) || (string.IsNullOrEmpty(txt_PrimeiroNome.Text) || (string.IsNullOrEmpty(txt_observacao.Text)) || (string.IsNullOrEmpty(comboBox1.Text) || (string.IsNullOrEmpty(txt_turma.Text) || (string.IsNullOrEmpty(txt_Nota.Text)))))))
            {
                MessageBox.Show("Preencha todos os campos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
            }
            //validação da nota
            if (txt_Nota.Text != "")
            {
                if (int.Parse(txt_Nota.Text) > 20 || int.Parse(txt_Nota.Text) < 0)
                {
                    MessageBox.Show("A nota tem que ser entre 0 e 20", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    output = false;
                }
            }
           
            return output;
        }

        private void limpacampos() //limpa todos os campos
        {
            txt_NumNota.Text = "";
            txt_PrimeiroNome.Text = "";
            txt_turma.Text = "";
            comboBox1.SelectedIndex = -1;
            txt_observacao.Text = "";
            txt_Nota.Text = "";
        }

        private void bt_apagarNotas_Click(object sender, EventArgs e) //apaga a nota
        {
            Notas NotaADeletar;
            if (Validaform())
            {
                NotaADeletar = new Notas()
                {
                    Num_Nota = int.Parse(txt_NumNota.Text)
                };
                NotaADeletar.DeleteNotasBD(NotaADeletar);
                ListarTudo();
                mostrarNotasAlunos();
                limpacampos();
            }
        }

        private void txt_Nota_TextChanged(object sender, EventArgs e) //validação apenas numeros
        {
            foreach (char car in txt_Nota.Text)
            {
                if (!char.IsDigit(car))
                {
                    MessageBox.Show("Atenção! Insira apenas números");
                    txt_Nota.ResetText();
                    break;
                }
            }
        }


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            lb_horas.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void txtpesquisa_TextChanged(object sender, EventArgs e) //pesquisa na dgv
        {
            string searchValue = txtpesquisa.Text;

            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells[0].Value.ToString().Contains(searchValue) || row.Cells[1].Value.ToString().Contains(searchValue) || row.Cells[2].Value.ToString().Contains(searchValue))
                    {
                        dataGridView2.Refresh();
                        dataGridView2.Rows.RemoveAt(row.Index);
                        dataGridView2.Rows.Insert(row.Index + 1, row);
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

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
