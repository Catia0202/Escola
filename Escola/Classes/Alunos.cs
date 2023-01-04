
namespace Escola.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Data.SQLite;
    using System.IO;
    using System.Windows.Forms;
    public class Alunos
    {
        public int Num_Aluno { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public int Telemovel { get; set; }
        public string Genero { get; set; }
        public string Data_Nascimento { get; set; }
        public string morada { get; set; }
        public string imagem { get; set; }

        public int Cod_Turma { get; set; }

        private SQLiteConnection connection;
        private SQLiteCommand command;

        public override string ToString()
        {
            return $"|  {Num_Aluno} | {NomeCompleto} | {Email} | {Telemovel} | {Genero} | {Data_Nascimento} | {morada} | {imagem} | {Cod_Turma}| ";
        }





        public void AdicionarAlunoBD(List<Alunos> alunos) //Adicionar aluno à base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            try
            {
                foreach (var aluno in alunos)
                {
                    string sql = string.Format("insert into Alunos (NumAluno, NomeCompleto , Email, telemovel,genero,dataNascimento,morada,imagem,codTurma)" +
                        "values ({0},'{1}','{2}',{3},'{4}','{5}','{6}','{7}',{8})", aluno.Num_Aluno, aluno.NomeCompleto,aluno.Email,aluno.Telemovel,aluno.Genero,aluno.Data_Nascimento,aluno.morada,aluno.imagem,aluno.Cod_Turma);
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

        public void UpdateAlunoBD(Alunos AlunoAEditar) //Editar aluno na base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            string sql = string.Format("update Alunos SET  NomeCompleto = '{0}', Email = '{1}', telemovel = {2}, genero = '{3}'" +
                ", dataNascimento = '{4}',morada = '{5}' ,imagem = '{6}',codTurma = {7} where NumAluno = {8} ", AlunoAEditar.NomeCompleto, AlunoAEditar.Email, AlunoAEditar.Telemovel, AlunoAEditar.Genero, AlunoAEditar.Data_Nascimento, AlunoAEditar.morada, AlunoAEditar.imagem , AlunoAEditar.Cod_Turma, AlunoAEditar.Num_Aluno);
            command = new SQLiteCommand(sql,connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DeleteAlunoBD(Alunos AlunoADeletar) //Deletar Aluno na base de dados
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            string sql = string.Format("DELETE FROM Alunos WHERE NumAluno =" + AlunoADeletar.Num_Aluno);
            command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();

        }



        public void DeletaTudo() //Deletar todos os alunos (apenas caso seja necessário)
        {
            var path = @"Data\Escola.sqlite";
            connection = new SQLiteConnection("DataSource=" + path);
            connection.Open();
            try
            {
                string sql = "Delete from Alunos";

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
