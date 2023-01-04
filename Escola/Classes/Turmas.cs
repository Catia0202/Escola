using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Escola.Classes
{
    public class Turmas
    {
        public int Num_Turma { get; set; }
        public string Nome_Turma { get; set; }
        public string Descrição { get; set; }
        public int cod_Disciplina{ get; set; }


        private SQLiteConnection connection;
        private SQLiteCommand command;


        public override string ToString()
        {
            return $"|  {Num_Turma} | {Nome_Turma}  | {Descrição} | {cod_Disciplina} | ";
        }


        public void AdicionarTurmaBD(List<Turmas> turmas) //Adicionar turma na base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            try
            {
                foreach (var turma in turmas)
                {
                    string sql = string.Format("insert into Turmas (NumTurma, Nome_Turma, cod_disciplina, descrição)" +
                        "values ({0},'{1}',{2},'{3}')", turma.Num_Turma,turma.Nome_Turma,turma.cod_Disciplina,turma.Descrição);
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


        public void UpdateTurmaBD(Turmas TurmaAEditar) //Editar turma na base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            string sql = string.Format("update Turmas SET  NumTurma  = {0}, Nome_Turma = '{1}', cod_disciplina = {2}, descrição = '{3}' where NumTurma = {4} ", TurmaAEditar.Num_Turma, TurmaAEditar.Nome_Turma, TurmaAEditar.cod_Disciplina, TurmaAEditar.Descrição, TurmaAEditar.Num_Turma);
            command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DeleteTurmaBD(Turmas TurmaADeletar) //Deleta turma na base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            string sql = string.Format("DELETE FROM Turmas WHERE NumTurma =" + TurmaADeletar.Num_Turma);
            command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();

        }



    }
}
