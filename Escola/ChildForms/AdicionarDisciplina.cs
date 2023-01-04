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
    public partial class AdicionarDisciplina : Form
    {
        List<Disciplinas> Disciplinas = new List<Disciplinas>();
        List<Turmas> ListaTurmas;
        List<Disciplinas> ListaDisciplinas;
        List<Professores> ListaProfessores;
        Disciplinas disciplinas = new Disciplinas();
        BaseDeDados BaseDeDados = new BaseDeDados();
        public AdicionarDisciplina()
        {
            InitializeComponent();
            timer1.Start();
            

            lb_data.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ListarDisciplinas();
            ListaTurmas = BaseDeDados.GetDataTurmas(); //Vai buscar todas as turmas na BD
            ListaProfessores = BaseDeDados.GetDataProfessores(); //Vai buscar todos os professores na BD

            comboBox1.DataSource = ListaTurmas;
            comboBox1.DisplayMember = "Nome_Turma";
            comboBox1.ValueMember = "Num_Turma";

            comboBox2.DataSource = ListaProfessores;
            comboBox2.DisplayMember = "Primeiro_Nome";
            comboBox2.ValueMember = "Num_Professor";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            Num_Aleatorio();

            if (comboBox1.Items.Count <= 0) //Validação se existe turmas ou professores inseridos na base de dados
            {
                MessageBox.Show("Ainda não existem turmas criadas. Não será possível adicionar nenhuma disciplina à mesma", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (comboBox2.Items.Count <= 0)
            {
                MessageBox.Show("Ainda não inseriu nenhum professor. Não será possível atribuir nenhum professor à disciplina criada", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void bt_adicionarDisciplinas_Click(object sender, EventArgs e) //Adicionar Disciplina
        {
            Disciplinas NovaDisciplina;
            bool encontra = false;
            if (Validaform())
            {

                foreach (DataGridViewRow row in dataGridView1.Rows) //Validação se já existe uma disciplina com o mesmo nome na mesma turma
                {
                    if (txt_NomeDisciplina.Text == row.Cells[1].Value.ToString() && comboBox1.Text == row.Cells[5].Value.ToString())
                    {
                        encontra = true;
                        MessageBox.Show("Já existe uma discplina com esse nome nesta turma", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (!encontra)
                {
                    NovaDisciplina = new Disciplinas()
                    {
                        Num_Disciplinas = int.Parse(txt_numDisciplina.Text),
                        Nome_Disciplina = txt_NomeDisciplina.Text,
                        Descrição_Disciplina = txt_Descricao.Text,
                        cod_Turma = int.Parse(lb_valuecombo.Text),
                        cod_Professor = int.Parse(value_combo2.Text)
                    };

                    Disciplinas.Add(NovaDisciplina); //Adiciona na Lista
                    disciplinas.AdicionarDisciplinasBD(Disciplinas);//Adiciona na bd o que está na lista
                    Disciplinas.Clear(); //limpar para não haver repetições
                    ListarDisciplinas();
                    limpacampos();
                    Num_Aleatorio();
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

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                value_combo2.Text = "";
                value_combo2.Text = comboBox2.SelectedValue.ToString();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) //Clicar na datagrid e aparecer as inf's
        {
            txt_numDisciplina.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txt_NomeDisciplina.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txt_Descricao.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            comboBox2.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void txtpesquisa_TextChanged(object sender, EventArgs e) //Pesquisar na dgv
        {
            string searchValue = txtpesquisa.Text;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[1].Value.ToString().Contains(searchValue) || row.Cells[2].Value.ToString().Contains(searchValue) || row.Cells[3].Value.ToString().Contains(searchValue) || row.Cells[5].Value.ToString().Contains(searchValue))
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
        private void bt_limpar_Click(object sender, EventArgs e)
        {
            limpacampos();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lb_horas.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Num_Aleatorio()  //Número aleatório para a disciplina
        {
            Random rnd = new Random();
            String r = rnd.Next(1, 1000000).ToString("D6");
            txt_numDisciplina.Text = r.ToString();
        }

        private void ListarDisciplinas() //Adicionar todas as disciplinas na BD na datagridView
        {
            dataGridView1.Rows.Clear();
            BaseDeDados.CriarTabelas();
            ListaDisciplinas = BaseDeDados.GetDataDisciplinas();
            ListaProfessores = BaseDeDados.GetDataProfessores();
            ListaTurmas = BaseDeDados.GetDataTurmas();


            foreach (Disciplinas disciplinasL in ListaDisciplinas)
            {
                foreach (Professores professores in ListaProfessores)
                {
                    foreach (Turmas turmas in ListaTurmas)
                    {
                        if (disciplinasL.cod_Professor == professores.Num_Professor && disciplinasL.cod_Turma == turmas.Num_Turma)
                        {
                            dataGridView1.Rows.Add(disciplinasL.Num_Disciplinas, disciplinasL.Nome_Disciplina, disciplinasL.Descrição_Disciplina, disciplinasL.cod_Professor, professores.Primeiro_Nome, turmas.Nome_Turma);
                        }
                    }
                }
            }
        }

        private void limpacampos() //limpar todos os campos
        {
            
            txt_NomeDisciplina.Text = "";
            txt_Descricao.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }

        private bool Validaform()
        {
            //validação de campo vazio
            bool output = true;
            if ((string.IsNullOrEmpty(txt_numDisciplina.Text) || (string.IsNullOrEmpty(txt_NomeDisciplina.Text) || (string.IsNullOrEmpty(txt_Descricao.Text))|| (string.IsNullOrEmpty(comboBox1.Text) || (string.IsNullOrEmpty(comboBox2.Text))))))
            {
                MessageBox.Show("Preencha todos os campos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
            }
            return output;
        }

      
       
    }
}
