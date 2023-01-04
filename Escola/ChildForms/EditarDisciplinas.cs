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
    public partial class EditarDisciplinas : Form
    {
     
        List<Turmas> ListaTurmas;
        List<Disciplinas> ListaDisciplinas;
        List<Professores> ListaProfessores;
        Disciplinas disciplinas = new Disciplinas();
        BaseDeDados BaseDeDados = new BaseDeDados();
        Disciplinas DisciplinaAEditar = new Disciplinas();

        public EditarDisciplinas()
        {
            InitializeComponent();
            timer1.Start();
            lb_data.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ListarDisciplinas();
            ListaProfessores = BaseDeDados.GetDataProfessores(); //recebe todos os professores
            ListaTurmas = BaseDeDados.GetDataTurmas();//recebe todas as turmas
            comboBox1.DataSource = ListaTurmas;
            comboBox1.DisplayMember = "Nome_Turma";
            comboBox1.ValueMember = "Num_Turma";

            comboBox2.DataSource = ListaProfessores;
            comboBox2.DisplayMember = "Primeiro_Nome";
            comboBox2.ValueMember = "Num_Professor";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

        }

        private void ListarDisciplinas() //adiciona todas as disciplinas na dgv
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
      private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) //Clicar na dgv e aparecer as inf´s
        {
            txt_numDisciplina.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txt_NomeDisciplina.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txt_Descricao.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            comboBox2.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();

        }

        private void bt_updateDisciplinas_Click(object sender, EventArgs e) //edita disciplina
        {
            Disciplinas DisciplinaAEditar;
            if (Validaform())
            {
                DisciplinaAEditar = new Disciplinas()
                {
                    Num_Disciplinas = int.Parse(txt_numDisciplina.Text),
                    Nome_Disciplina = txt_NomeDisciplina.Text,
                    Descrição_Disciplina = txt_Descricao.Text,
                    cod_Turma = int.Parse(lb_valuecombo.Text),
                    cod_Professor = int.Parse(value_combo2.Text)
                };
                disciplinas.UpdateDisciplinasBD(DisciplinaAEditar);
                ListarDisciplinas();
            }
        }

        private void EditarDisciplinas_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                lb_valuecombo.Text = "";
                lb_valuecombo.Text = comboBox1.SelectedValue.ToString();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                value_combo2.Text = "";
                value_combo2.Text = comboBox2.SelectedValue.ToString();
            }
        }

        private void bt_apagarDisciplinas_Click(object sender, EventArgs e) //Apaga disciplina
        {
            Disciplinas DisciplinasADeletar;
            DisciplinasADeletar = new Disciplinas()
            {
                Num_Disciplinas = int.Parse(txt_numDisciplina.Text)
            };
            DisciplinasADeletar.DeleteDisciplinasBD(DisciplinasADeletar);
            ListarDisciplinas();
            limpacampos();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            limpacampos();
        }
        private void limpacampos() //limpar todos os campos
        {
            txt_numDisciplina.Text = "";
            txt_NomeDisciplina.Text = "";
            txt_Descricao.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }
        private bool Validaform() //validações
        {
            //validação de campos vazios
            bool output = true;
            if ((string.IsNullOrEmpty(txt_numDisciplina.Text) || (string.IsNullOrEmpty(txt_NomeDisciplina.Text) || (string.IsNullOrEmpty(txt_Descricao.Text)) || (string.IsNullOrEmpty(comboBox1.Text) || (string.IsNullOrEmpty(comboBox2.Text))))))
            {
                MessageBox.Show("Preencha todos os campos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
            }
            return output;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lb_horas.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void txtpesquisa_TextChanged(object sender, EventArgs e) //pesquisa na dgv
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
    }
}
