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
using System.Text.RegularExpressions;
namespace Escola.ChildForms
{
    public partial class AdicionarAluno : Form
    {
        List<Alunos> alunos = new List<Alunos>();
        List<Alunos> ListaAlunos;
        Alunos Alunos = new Alunos();
        BaseDeDados BaseDeDados = new BaseDeDados();
        List<Turmas> ListaTurmas;
        public AdicionarAluno()
        {
            InitializeComponent();
            timer1.Start();
            Num_Aleatorio();
            ListaTurmas = BaseDeDados.GetDataTurmas(); //Vai buscar todas as turmas na BD
            ListarAlunos();
            lb_data.Text = DateTime.Now.ToString("dd/MM/yyyy");
            comboBox1.DataSource = ListaTurmas;
            comboBox1.DisplayMember = "Nome_Turma";
            comboBox1.ValueMember = "Num_Turma";
            dtp_dataNascimento.MinDate = new DateTime(2004, 1, 1);
            dtp_dataNascimento.MaxDate= new DateTime(2010, 1, 31);
            comboBox1.SelectedIndex = -1;

            if (comboBox1.Items.Count <= 0) //Validação de não haver turmas inseridas na BD
            {
                MessageBox.Show("Ainda não existem turmas criadas. Não será possível adicionar nenhum aluno", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Num_Aleatorio() //Número aleatório para o aluno
        {
            Random rnd = new Random();
            String r = rnd.Next(1, 1000000).ToString("D6");

            txt_numAluno.Text = r.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lb_horas.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void bt_adicionarAluno_Click(object sender, EventArgs e) //Adicionar Aluno
        {
            Alunos NovoAluno;
            Alunos AlunoDtg;
           
            if (rdb_feminino.Checked)
            {
                lb_genero.Text = "Feminino";


            }
            else if (rbd_masculino.Checked)
            {
                lb_genero.Text = "Masculino";
            }


            //PARA NÃO IGNORAR OS DADOS QUE JÁ ESTÃO NA DATAGRIDVIEW

            if (Validaform())
            {

                foreach (DataGridViewRow item in dataGridView1.Rows)
                {

                    AlunoDtg = new Alunos()
                    {
                        Num_Aluno = int.Parse(item.Cells[0].Value.ToString()),
                        NomeCompleto = item.Cells[1].Value.ToString(),
                        Email = item.Cells[2].Value.ToString(),
                        Telemovel = int.Parse(item.Cells[3].Value.ToString()),
                        Genero = item.Cells[4].Value.ToString(),
                        Data_Nascimento = item.Cells[5].Value.ToString(),
                        morada = item.Cells[6].Value.ToString(),
                        imagem = item.Cells[7].Value.ToString(),
                        Cod_Turma = int.Parse(item.Cells[8].Value.ToString())

                    };
                    alunos.Add(AlunoDtg);
                }

                ///ADICIONAR UM NOVO ALUNO
                NovoAluno = new Alunos()
                {
                    Num_Aluno = int.Parse(txt_numAluno.Text),
                    NomeCompleto = txt_PrimeiroNome.Text,
                    Email = txt_Email.Text,
                    Telemovel = int.Parse(txt_telemovel.Text),
                    Genero = lb_genero.Text,
                    Data_Nascimento = dtp_dataNascimento.Text,
                    morada = txt_morada.Text,
                    imagem = lb_Imagem.Text,
                    Cod_Turma = int.Parse(lb_valuecombo.Text)
                };

                alunos.Add(NovoAluno); //Adiciona na lista
                Alunos.DeletaTudo();
                Alunos.AdicionarAlunoBD(alunos); //Adiciona na bd o que está na lista
                alunos.Clear(); //limpar para não haver repetições
                ListarAlunos();
                Num_Aleatorio();
                limpartudo();
            }
        }

        private void bt_carregarfoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "PNG Files(*.png)|*.png|JPG Files(*.jpg)|*.jpg|All Files(*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string picPath = dlg.FileName.ToString();
                lb_Imagem.Text = picPath;
                pictureBox1.ImageLocation = picPath;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) //Clicar na datagrid e aparecer as inf's
        {
         

            txt_numAluno.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txt_PrimeiroNome.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txt_Email.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            txt_telemovel.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            lb_genero.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            dtp_dataNascimento.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            txt_morada.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            lb_Imagem.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();

            if (lb_genero.Text == "Feminino")
            {
                rdb_feminino.Checked = true;
            }
            else
            {
                rbd_masculino.Checked = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                lb_valuecombo.Text = "";
                lb_valuecombo.Text = comboBox1.SelectedValue.ToString(); //por valor da combo na label para guardar
            }
        }

        private void txtpesquisa_TextChanged(object sender, EventArgs e) //Pesquisa dgv
        {
            string searchValue = txtpesquisa.Text;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[1].Value.ToString().Contains(searchValue) || row.Cells[2].Value.ToString().Contains(searchValue) || row.Cells[9].Value.ToString().Contains(searchValue))
                    {
                        dataGridView1.Refresh();
                        dataGridView1.Rows.RemoveAt(row.Index); //remover da posição atual
                        dataGridView1.Rows.Insert(row.Index + 1, row); //Adicionar na primeira posição
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
            limpartudo();
        }

        private void limpartudo() //limpar todos os campos
        {
            txt_PrimeiroNome.Text = "";
            pictureBox1.Image = null;
            txt_Email.Text = "";
            txt_morada.Text = "";
            txt_telemovel.Text = "";
            lb_genero.Text = "";
            lb_Imagem.Text = "";
            lb_valuecombo.Text = "";
            comboBox1.SelectedIndex = -1;
            rdb_feminino.Checked = false;
            rbd_masculino.Checked = false;
            dtp_dataNascimento.ResetText();
        }

        public void ListarAlunos() //Adicionar todos os alunos na BD na datagridView
        {
            dataGridView1.Rows.Clear();
            BaseDeDados.CriarTabelas();
            ListaAlunos = BaseDeDados.GetDataAlunos();
            foreach (Alunos alunoL in ListaAlunos)
            {
                foreach (Turmas turmasl in ListaTurmas)
                {
                    if (alunoL.Cod_Turma == turmasl.Num_Turma)
                    {
                        string path = @"" + alunoL.imagem;
                        dataGridView1.Rows.Add(alunoL.Num_Aluno, alunoL.NomeCompleto, alunoL.Email, alunoL.Telemovel, alunoL.Genero, alunoL.Data_Nascimento, alunoL.morada, alunoL.imagem, alunoL.Cod_Turma, turmasl.Nome_Turma);
                    }
                }
            }
        }


        #region validações
        private bool Validaform() //Validações
        {
            bool output = true;
            //Validação campo vazio
            if ((string.IsNullOrEmpty(txt_PrimeiroNome.Text) || (string.IsNullOrEmpty(txt_Email.Text) || (string.IsNullOrEmpty(txt_morada.Text) || (string.IsNullOrEmpty(txt_telemovel.Text) || (string.IsNullOrEmpty(lb_genero.Text) || (string.IsNullOrEmpty(lb_Imagem.Text) || (string.IsNullOrEmpty(lb_valuecombo.Text)))))))))
            {
                MessageBox.Show("Preencha todos os campos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
            }
            //Validação NºTelemóvel
            Regex r = new Regex(@"^([9][1236])[0-9]*$"); ///Validação número.

            if (!r.IsMatch(txt_telemovel.Text) || txt_telemovel.Text.Length < 9 || txt_telemovel.Text.Length > 9)
            {
                MessageBox.Show("Esse número não é válido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
            }
            //Validação Email
            Regex a = new Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");
            if (!a.IsMatch(txt_Email.Text))
            {
                MessageBox.Show("Esse email não é válido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
            }
            return output;
        }

        private void txt_PrimeiroNome_TextChanged(object sender, EventArgs e) //Validação de Apenas Letras
        {
            foreach (char car in txt_PrimeiroNome.Text)
            {
                if (!(char.IsLetter(car) || car == ' '))
                {
                    MessageBox.Show("Atenção! Insira apenas letras!");
                    txt_PrimeiroNome.ResetText();
                    break;
                }
            }
        }

        private void txt_Email_TextChanged(object sender, EventArgs e)
        {
        }

      

        private void txt_telemovel_TextChanged(object sender, EventArgs e) //Validação apenas números
        {
            foreach (char car in txt_telemovel.Text)
            {
                if (!char.IsDigit(car))
                {
                    MessageBox.Show("Atenção! Insira apenas números");
                    txt_telemovel.ResetText();
                    break;
                }
            }


        }
        #endregion
  
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
        }
    }
}
