using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Escola.Classes
{
    public class Disciplinas
    {
        public int Num_Disciplinas { get; set; }
        public string Nome_Disciplina { get; set; }
        public string Descrição_Disciplina { get; set; }
        public int cod_Turma { get; set; }
        public int cod_Professor { get; set; }

        private SQLiteConnection connection;
        private SQLiteCommand command;

        public override string ToString()
        {
            return $"|  {Num_Disciplinas} | {Nome_Disciplina} | {Descrição_Disciplina} |{cod_Turma}| {cod_Professor} | ";
        }

        public void AdicionarDisciplinasBD(List<Disciplinas> disciplinas) //Adicionar disciplina na base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            try
            {
                foreach (var disciplina in disciplinas)
                {
                    string sql = string.Format("insert into Disciplinas (NumDisciplina, Nome_Disciplina,Descricao_Disciplina,codTurma,codprofessor)" +
                        "values ({0},'{1}','{2}',{3},{4})", disciplina.Num_Disciplinas, disciplina.Nome_Disciplina, disciplina.Descrição_Disciplina,disciplina.cod_Turma, disciplina.cod_Professor);
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

        public void UpdateDisciplinasBD(Disciplinas DisciplinaAEditar) //Editar disciplina na base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            string sql = string.Format("update Disciplinas SET  NumDisciplina = '{0}',Nome_Disciplina = '{1}',Descricao_Disciplina = '{2}',codTurma = {3},codprofessor = {4} where NumDisciplina = {5}"
                , DisciplinaAEditar.Num_Disciplinas, DisciplinaAEditar.Nome_Disciplina, DisciplinaAEditar.Descrição_Disciplina, DisciplinaAEditar.cod_Turma, DisciplinaAEditar.cod_Professor, DisciplinaAEditar.Num_Disciplinas);
            command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();

        }

        public void DeleteDisciplinasBD(Disciplinas DisciplinasADeletar) //Elimina disciplina na base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            string sql = string.Format("DELETE FROM Disciplinas WHERE NumDisciplina =" + DisciplinasADeletar.Num_Disciplinas);
            command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void DeletaTudo() //Deleta todas as disciplinas (caso seja necessário)
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            try
            {
                string sql = "Delete from Disciplinas";

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
