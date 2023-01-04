using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace Escola.Classes
{
    public class Professores
    {
        public int Num_Professor { get; set; }
        public string Primeiro_Nome { get; set; }
        public string Ultimo_Nome { get; set; }
        public string Genero { get; set; }
        public string curriculo { get; set; }

        public override string ToString()
        {
            return $"|  {Num_Professor} | {Primeiro_Nome} | {Ultimo_Nome}  | {Genero} |  {curriculo}| ";
        }


        

        private SQLiteConnection connection;
        private SQLiteCommand command;

        public void AdicionarProfessorBD(List<Professores> professores) //Adicionar professor na base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            try
            {
                foreach (var professor in professores)
                {
                    string sql = string.Format("insert into Professores (NumProfessor, PrimeiroNome,UltNome,Genero,Curriculo)" +
                        "values ({0},'{1}','{2}','{3}','{4}')", professor.Num_Professor, professor.Primeiro_Nome, professor.Ultimo_Nome,professor.Genero,professor.curriculo);
                    command = new SQLiteCommand(sql, connection);
                    command.ExecuteNonQuery();

                }

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro");
            }
        }

        public void UpdateProfessoresBD(Professores ProfessorAEditar) //Editar professor na base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            string sql = string.Format("update Professores SET  NumProfessor = {0},PrimeiroNome = '{1}',UltNome = '{2}',Genero = '{3}',Curriculo = '{4}' where NumProfessor = {5}"
                , ProfessorAEditar.Num_Professor, ProfessorAEditar.Primeiro_Nome, ProfessorAEditar.Ultimo_Nome, ProfessorAEditar.Genero, ProfessorAEditar.curriculo, ProfessorAEditar.Num_Professor);
            command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();

        }


        public void DeleteProfessorBD(Professores ProfessorAEditar) //Deleta professor na base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            string sql = string.Format("DELETE FROM Professores WHERE NumProfessor =" + ProfessorAEditar.Num_Professor);
            command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void DeletaTudo() //Deleta todos os professores na base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            try
            {
                string sql = "Delete from Professores";

                command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro");
            }
        }

    }




}
