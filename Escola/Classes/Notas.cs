using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escola.Classes
{
    public class Notas
    {
        public int Num_Nota { get; set; }
        public int Id_Aluno { get; set; }
        public int Id_Disciplina { get; set; }
        public double Nota { get; set; }
        public string Anotações { get; set; }

        private SQLiteConnection connection;
        private SQLiteCommand command;

        public override string ToString()
        {
            return $"|  {Num_Nota} | {Id_Aluno} | {Id_Disciplina} |{Nota}| {Anotações} | ";
        }

        public void AdicionarNotasBD(List<Notas> notas)  //Adicionar Nota à base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            try
            {
                foreach (var notasl in notas)
                {
                    string sql = string.Format("insert into Notas (NumNota, id_aluno ,id_disciplina,nota,anotacoes)" +
                        "values ({0},{1},{2},'{3}','{4}')", notasl.Num_Nota, notasl.Id_Aluno,notasl.Id_Disciplina,notasl.Nota,notasl.Anotações);
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

        public void UpdateNotasBD(Notas NotaAEditar) //Editar nota na base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            string sql = string.Format("update Notas SET  NumNota = {0}, id_aluno = {1},id_disciplina = {2},nota = '{3}', anotacoes = '{4}' where NumNota = {5}"
                , NotaAEditar.Num_Nota, NotaAEditar.Id_Aluno,NotaAEditar.Id_Disciplina, NotaAEditar.Nota,NotaAEditar.Anotações,NotaAEditar.Num_Nota);
            command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();

        }

        public void DeleteNotasBD(Notas NotasADeletar) //Deletar nota na base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            string sql = string.Format("DELETE FROM Notas WHERE NumNota =" + NotasADeletar.Num_Nota);
            command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DeletaTudo() //Deletar todas as notas(apenas se for necessário)
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            try
            {
                string sql = "Delete from Nota";

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
