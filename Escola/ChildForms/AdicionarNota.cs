using System;
using System.Collections;
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
    public partial class AdicionarNota : Form
    {
        List<Alunos> ListaAlunos;
        List<Turmas> ListaTurmas;
        List<Disciplinas> ListaDisciplinas;
        List<Disciplinas> Disciplinas = new List<Disciplinas>();
        BaseDeDados BaseDeDados = new BaseDeDados();
        List<Notas> ListaNotas;
        List<Notas> Notas = new List<Notas>();
        Notas notas = new Notas();
        public AdicionarNota()
        {
            InitializeComponent();
            ListaAlunos = BaseDeDados.GetDataAlunos();
            ListaTurmas = BaseDeDados.GetDataTurmas();
     
            ListaDisciplinas = BaseDeDados.GetDataDisciplinas();
            comboBox1.SelectedIndex = -1;
            ListarTudo();
            timer1.Start();
            lb_data.Text = DateTime.Now.ToString("dd/MM/yyyy");

            if(dataGridView1.Rows.Count < 1) //validação de não haver alunos inseridos na BD
            {
                MessageBox.Show("Ainda não adicionou nenhum aluno para ser possível atribuir nota", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) //Clicar na dgv e aparecer as inf's
        {
            Num_Aleatorio();
            txt_PrimeiroNome.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txt_turma.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            lb_Codaluno.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            lb_codTurma.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            comboBox1.DataSource = null;
            comboBox1.Items.Clear();
            Disciplinas.Clear();
            int turmaescolhida = int.Parse(lb_codTurma.Text); 
                if (turmaescolhida != 0) //Mostrar apenas as disciplinas que pertencem à respetiva turma escolhida
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
                         comboBox1.SelectedIndex = -1;

                if (comboBox1.Items.Count <= 0)
                {
                    MessageBox.Show("Ainda não existem disciplinas atríbuídas a esta turma, por isso não será possível atribuir nenhuma nota a nenhum aluno pertencente a esta turma", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                }
                mostrarNotasAlunos();
        }

        private void bt_adicionarNota_Click(object sender, EventArgs e) //Adicionar Nota
        {
            bool encontra = false;
            Notas NovaNota;
            if (Validaform())
            {
                foreach (DataGridViewRow row in dataGridView1.Rows) //Validação se já atribuiu uma nota a esse aluno nessa disciplina
                {
                    foreach(DataGridViewRow row2 in dataGridView2.Rows)
                    if (lb_valuecombo.Text == row2.Cells[4].Value.ToString() && txt_PrimeiroNome.Text == row.Cells[0].Value.ToString() )
                    {
                        encontra = true;
                        MessageBox.Show("Já atribuiu uma nota a esse aluno nessa disciplina, se pretende alterar vá para 'Editar Notas'", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (!encontra)
                {
                    NovaNota = new Notas()
                    {
                        Num_Nota = int.Parse(txt_NumNota.Text),
                        Nota = double.Parse(txt_Nota.Text),
                        Anotações = txt_observacao.Text,
                        Id_Aluno = int.Parse(lb_Codaluno.Text),
                        Id_Disciplina = int.Parse(lb_valuecombo.Text)
                    };
                    Notas.Add(NovaNota); //Adicionar na lista
                    notas.AdicionarNotasBD(Notas);//Adicionar na bd o que está na lista
                    Notas.Clear();
                    limpacampos();
                    mostrarNotasAlunos();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                lb_valuecombo.Text = "";
                lb_valuecombo.Text = comboBox1.SelectedValue.ToString();
            }
        }

        private void bt_limpar_Click(object sender, EventArgs e)
        {
            limpacampos();
        }

        private void ListarTudo() //Inserir todas as informações na dgv
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
        private void mostrarNotasAlunos() //Inserir todas as informações na dgv
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

        private bool Validaform() //Validações
        {
            bool output = true;
            //Validação de preencher todos os campos
            if ((string.IsNullOrEmpty(txt_NumNota.Text) || (string.IsNullOrEmpty(txt_PrimeiroNome.Text) || (string.IsNullOrEmpty(txt_observacao.Text)) || (string.IsNullOrEmpty(comboBox1.Text) || (string.IsNullOrEmpty(txt_turma.Text) || (string.IsNullOrEmpty(txt_Nota.Text)))))))
            {
                MessageBox.Show("Preencha todos os campos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
            }
            //validação da nota 
            if (!string.IsNullOrEmpty(txt_Nota.Text)){
                if (int.Parse(txt_Nota.Text) > 20 || int.Parse(txt_Nota.Text) < 0)
                {
                    MessageBox.Show("A nota tem que ser entre 0 e 20", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    output = false;
                }
            }
            return output;
        }

        private void limpacampos() //limpar todos os campos
        {
           txt_NumNota.Text = "";
            txt_PrimeiroNome.Text = "";
            txt_turma.Text = "";
            comboBox1.SelectedIndex = -1;
            txt_observacao.Text = "";
            txt_Nota.Text = "";
        }
        private void Num_Aleatorio() //Num aleatório para nota
        {
            Random rnd = new Random();
            String r = rnd.Next(1, 1000000).ToString("D6");

            txt_NumNota.Text = r.ToString();
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

        private void txtpesquisa_TextChanged(object sender, EventArgs e) //pesquina na dgv
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
    }
}
