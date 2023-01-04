using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Escola.Classes;
namespace Escola.ChildForms
{
    public partial class AdicionarProfessores : Form
    {
        List<Professores> professores = new List<Professores>();
        List<Professores> ListaProfessores;
        Professores Professores = new Professores();
        BaseDeDados BaseDeDados = new BaseDeDados();

        public AdicionarProfessores()
        {
            InitializeComponent();
            timer1.Start();
            ListarProfessores();
            Num_Aleatorio();
            lb_data.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            lb_horas.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void label4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text File (.txt)|*.txt|Word File (.docx ,.doc)|*.docx;*.doc|PDF (.pdf)|*.pdf";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string picPath = dlg.FileName.ToString();
                lb_curriculo.Text = picPath;
               
            }
        }

        private void bt_adicionarProfessor_Click(object sender, EventArgs e) //Adicionar Professores
        {
            Professores NovoProfessor;
           

            if (rdb_feminino.Checked)
            {
                lb_genero.Text = "Feminino";


            }
            else if (rbd_masculino.Checked)
            {
                lb_genero.Text = "Masculino";
            }

            if (Validaform())
            {
                NovoProfessor = new Professores()
                {
                    Num_Professor = int.Parse(txt_numProf.Text),
                    Primeiro_Nome = txt_PrimeiroNome.Text,
                    Ultimo_Nome = txt_UltNome.Text,
                    Genero = lb_genero.Text,
                    curriculo = lb_curriculo.Text
                };

                professores.Add(NovoProfessor); //Adicionar na lista
                Professores.AdicionarProfessorBD(professores); //Adicionar na bd o que está na lista
                professores.Clear();
                limpacampos();
                Num_Aleatorio();
                ListarProfessores();
            }
        
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) //Clicar na dgv e aparecer as inf's
        {
            var senderGrid = (DataGridView)sender;
             txt_numProf.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txt_PrimeiroNome.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txt_UltNome.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            lb_curriculo.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
        
            lb_genero.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            if (lb_genero.Text == "Feminino")
            {
                rdb_feminino.Checked = true;
            }
            else
            {
                rbd_masculino.Checked =true;
            }
            if (lb_curriculo.Text != "") //curriculo abrir fora da app
            {
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
       e.RowIndex >= 0)
                {

                    Process p = new Process();
                    ProcessStartInfo ps = new ProcessStartInfo();
                    ps.FileName = lb_curriculo.Text;
                    ps.Arguments = @"" + lb_curriculo.Text;
                    p.StartInfo = ps;
                    p.Start();
                }
            }
   
        }
        private void bt_limpar_Click(object sender, EventArgs e)
        {
            limpacampos();
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
        private void limpacampos() //limpar os campos
        { 
            txt_PrimeiroNome.Text = "";
            txt_UltNome.Text = "";
            lb_curriculo.Text = "";
            lb_genero.Text = "";
        }

        private void Num_Aleatorio() //num aleatorio para os professores
        {
            Random rnd = new Random();
            String r = rnd.Next(1, 1000000).ToString("D6");

            txt_numProf.Text = r.ToString();
        }
        private void ListarProfessores() //Adicionar na dgv todos os professores
        {
            dataGridView1.Rows.Clear();
            BaseDeDados.CriarTabelas();
            ListaProfessores = BaseDeDados.GetDataProfessores();
            foreach (Professores professoresL in ListaProfessores)
            {
                dataGridView1.Rows.Add(professoresL.Num_Professor, professoresL.Primeiro_Nome, professoresL.Ultimo_Nome, professoresL.Genero, "Ver", professoresL.curriculo);
            }
        }
        #region validações
        private bool Validaform() //Validações
        {
            bool output = true;
            //validação campos vazios
            if ((string.IsNullOrEmpty(txt_numProf.Text) || (string.IsNullOrEmpty(txt_PrimeiroNome.Text) || (string.IsNullOrEmpty(txt_UltNome.Text)) || (string.IsNullOrEmpty(lb_genero.Text) || (string.IsNullOrEmpty(lb_curriculo.Text) )))))
            {
                MessageBox.Show("Preencha todos os campos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
            }
            return output;
        }

        private void txt_PrimeiroNome_TextChanged(object sender, EventArgs e) //Validação apenas Letras
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

        private void txt_UltNome_TextChanged(object sender, EventArgs e) //Validação apenas Letras
        {
            foreach (char car in txt_UltNome.Text)
            {
                if (!(char.IsLetter(car) || car == ' '))
                {
                    MessageBox.Show("Atenção! Insira apenas letras!");
                    txt_PrimeiroNome.ResetText();
                    break;
                }
            }
        }
        #endregion
    
    }
}
