using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Escola.ChildForms;
using Escola.Classes;
using System.Data.SQLite;
namespace Escola
{

    using Escola.Classes;
    public partial class MenuPrincipal : Form
    {
        private SQLiteConnection connection;
        public MenuPrincipal()
        {
            InitializeComponent();
            MenuDesign();
            timer1.Start();
            lb_data.Text = DateTime.Now.ToString("dd/MM/yyyy");
            BaseDeDados baseDeDados = new BaseDeDados();
            baseDeDados.CriarBD();
            baseDeDados.CriarTabelas();
            totalalunos();
            totalturmas();
            medianotas();
        }





        private void timer1_Tick(object sender, EventArgs e)
        {
            lb_horas.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        public void totalalunos()
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);

            connection.Open();
            SQLiteCommand comm = new SQLiteCommand("SELECT COUNT(*) FROM Alunos ", connection);
            using (SQLiteDataReader read = comm.ExecuteReader())
            {
                while (read.Read())
                {
                    lb_totalalunos.Text = read.GetValue(0).ToString();

                };
            }
        }


        public void totalturmas()
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);

            connection.Open();
            SQLiteCommand comm = new SQLiteCommand("SELECT COUNT(*) FROM Turmas ", connection);
            using (SQLiteDataReader read = comm.ExecuteReader())
            {
                while (read.Read())
                {
                    lb_totalturmas.Text = read.GetValue(0).ToString();

                };
            }
        }

        public void medianotas()
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);

            connection.Open();
            SQLiteCommand comm = new SQLiteCommand("SELECT AVG(nota) FROM Notas ", connection);
            using (SQLiteDataReader read = comm.ExecuteReader())
            {
                while (read.Read())
                {
                    lb_notas.Text = read.GetValue(0).ToString();

                };
            }
        }




        private void MenuDesign()
        {
            panel_SubMenuAlunos.Visible=false;
            panel_SubmenuDisciplinas.Visible=false;
            panel_subMenuTurmas.Visible=false;
            panel_SubmenuNotas.Visible=false;
        }

        private void EscondeSubmenus()
        {
            if (panel_SubMenuAlunos.Visible == true)
            {
                panel_SubMenuAlunos.Visible = false;
            }
            if(panel_subMenuTurmas.Visible== true)
            {
                panel_subMenuTurmas.Visible = false;
            }
            if(panel_SubmenuDisciplinas.Visible == true)
            {
                panel_SubmenuDisciplinas.Visible = false;
            }
            if(panel_SubmenuNotas.Visible == true)
            {
                panel_SubmenuNotas.Visible = false;
            }
        }

        private void MostrarSubmenus(Panel SubMenu)
        {
          if (SubMenu.Visible == false)
            {
                EscondeSubmenus();
                SubMenu.Visible = true;
            }else
            {
                SubMenu.Visible = false;
            }
        }

        #region Menu e SubmenuAlunos
        private void bt_Alunos_Click_1(object sender, EventArgs e)
        {
            MostrarSubmenus(panel_SubMenuAlunos);
        }

        private void bt_adicionarAlunos_Click(object sender, EventArgs e)
        {
            AbrirChildForm(new AdicionarAluno());
            //codigo
            EscondeSubmenus();
        }

        private void bt_EditarAlunos_Click(object sender, EventArgs e)
        {
            AbrirChildForm(new EditarAluno());
            //codigo
            EscondeSubmenus();
        }

        private void bt_StatusAlunos_Click(object sender, EventArgs e)
        {
            //codigo
            AbrirChildForm(new ImprimirAlunos());
            EscondeSubmenus();
        }
        #endregion

        #region Menu e SubmenuTurmas
        private void bt_Turmas_Click(object sender, EventArgs e)
        {
            MostrarSubmenus(panel_subMenuTurmas);
        }

        private void bt_AdicionarTurmas_Click(object sender, EventArgs e)
        {
            //codigo
            AbrirChildForm(new AdicionarTurma());
            EscondeSubmenus();
        }

        private void bt_EditarTurmas_Click(object sender, EventArgs e)
        {
            AbrirChildForm(new EditarTurma());
         

            //codigo
            EscondeSubmenus();
        }

        private void bt_StatusTurmas_Click(object sender, EventArgs e)
        {
            AbrirChildForm(new ImprimirTurmas());
            //codigo
            EscondeSubmenus();
        }
        #endregion

        #region Menu e SubmenusNotas
        private void bt_Notas_Click(object sender, EventArgs e)
        {
            MostrarSubmenus(panel_SubmenuDisciplinas);
        }

        private void bt_AdicionarNotas_Click(object sender, EventArgs e) //disciplinas **
        {
            AbrirChildForm(new AdicionarDisciplina());
            //codigo
            EscondeSubmenus();
        }

        private void bt_EditarNotas_Click(object sender, EventArgs e)
        {
            AbrirChildForm(new EditarDisciplinas());
            //codigo
            EscondeSubmenus();
        }

        private void bt_statusNotas_Click(object sender, EventArgs e)
        {
            AbrirChildForm(new ImprimirDisciplinas());
            //codigo
            EscondeSubmenus();
        }
        #endregion

        #region Menu e SubmenusProfessores
        private void bt_professores_Click(object sender, EventArgs e)
        {
            MostrarSubmenus(panel_SubmenuNotas);
        }

        private void bt_adicionarProfessores_Click(object sender, EventArgs e) //Notaass*****
        {
            AbrirChildForm(new AdicionarNota());
            //codigo
            EscondeSubmenus();
        }

        private void bt_EditarProfessores_Click(object sender, EventArgs e)
        {
            AbrirChildForm(new EditarNota());
            //codigo
            EscondeSubmenus();
        }

        private void bt_StatusProfessores_Click(object sender, EventArgs e)
        {
            AbrirChildForm(new ImprimirNotas());
            //codigo
            EscondeSubmenus();
        }
        #endregion

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        
        }

        private Form activeForm = null;
        private void AbrirChildForm(Form childform)
        {
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm = childform;
            }
                childform.TopLevel = false;
                childform.FormBorderStyle = FormBorderStyle.None;
                childform.Dock = DockStyle.Fill;
                main_panel.Controls.Add(childform);
                main_panel.Tag = childform;
                childform.BringToFront();
                childform.Show();
            
        }


        private void FecharChildForm(Form childform)
        {
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm = childform;
            }
       
        }

        private void bt_professores_Click_1(object sender, EventArgs e)
        {
            MostrarSubmenus(panel_submenuProfessores);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AbrirChildForm(new AdicionarProfessores());

            EscondeSubmenus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AbrirChildForm(new ImprimirProfessores());

            EscondeSubmenus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AbrirChildForm(new EditarProfessores());

            EscondeSubmenus();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AbrirChildForm(new Créditos());

            EscondeSubmenus();
        }

        private void bt_Sair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
