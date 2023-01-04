
namespace Escola.Classes
{

    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public class BaseDeDados
    {

        private SQLiteConnection connection;
        private SQLiteCommand command;



        public void CriarBD()
        {
          
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            var path = @"Data\Escola.sqlite";
            try
            {
                connection = new SQLiteConnection("DataSource=" + path);
                connection.Open();
                //criar base de dados caso esta não esteja criada

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro");
            
            }
          
        }


        public void CriarTabelas() //Criar todas as tabelas necessárias para guardar as informações
        {
            var path = @"Data\Escola.sqlite";
            try
            {
                connection = new SQLiteConnection("DataSource=" + path);
                connection.Open();
                string sqlcommand1 = "create table if not exists Professores (NumProfessor int,PrimeiroNome varchar(50),UltNome varchar(50),Genero varchar(50),Curriculo varchar(100),PRIMARY KEY(NumProfessor));";

                string sqlcommand2 = "create table if not exists Disciplinas (NumDisciplina int,Nome_Disciplina varchar(50),Descricao_Disciplina varchar(50),codTurma int,codprofessor int,PRIMARY KEY(NumDisciplina),Foreign KEY (codprofessor) REFERENCES Professores(NumProfessor));";

                string sqlcommand3 = "create table if not exists Turmas (NumTurma int,Nome_Turma varchar(50),cod_disciplina int,descrição varchar(50),PRIMARY KEY(NumTurma),Foreign KEY (cod_disciplina) REFERENCES Disciplinas(NumDisciplina));";

                string sqlcommand4 = "create table if not exists Alunos(NumAluno int,NomeCompleto varchar(150),Email varchar(80), telemovel int,genero varchar(10),dataNascimento varchar(60),morada varchar(250),imagem varchar(150),codTurma int,PRIMARY KEY(NumAluno),Foreign KEY (codTurma) REFERENCES Turmas(NumTurma));";

                string sqlcommand5 = "create table if not exists Notas (NumNota int,id_aluno int ,id_disciplina int,nota double,anotacoes varchar(250),PRIMARY KEY(NumNota), Foreign KEY (id_aluno) REFERENCES Alunos(NumAluno),Foreign KEY (id_disciplina) REFERENCES Disciplinas(NumDisciplina));";

                command = new SQLiteCommand(sqlcommand1 + sqlcommand2 + sqlcommand3 + sqlcommand4 + sqlcommand5, connection);

                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro");
            }
        }



        public List<Alunos> GetDataAlunos() //Vai buscar todos os alunos existentes à Base de Dados
        {
            var path = @"Data\Escola.sqlite";
            List<Alunos> ListaAlunos = new List<Alunos>();
            try
            {
                connection = new SQLiteConnection("DataSource=" + path);
                connection.Open();
                string sql = "select * from Alunos";
                command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader(); //Lê cada registo

                while (reader.Read())
                
                   ListaAlunos.Add(new Alunos
                    {
                        Num_Aluno = (int)reader["NumAluno"],
                        NomeCompleto = (string)reader["NomeCompleto"],
                        
                        Email = (string)reader["Email"],
                        Telemovel= (int)reader["telemovel"],
                        Genero = (string)reader["genero"],
                        Data_Nascimento=(string)reader["dataNascimento"],
                        morada=(string)reader["morada"],
                        imagem=(string)reader["imagem"],
                        Cod_Turma=(int)reader["codTurma"]

                    });
                

                connection.Close();
                return ListaAlunos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro");
                return null;
            }
        }

        public List<Turmas> GetDataTurmas()  //Vai buscar todas as turmas existentes à Base de Dados
        {
            var path = @"Data\Escola.sqlite";
            List<Turmas> ListaTurmas = new List<Turmas>();
            try
            {
                connection = new SQLiteConnection("DataSource=" + path);
                connection.Open();
                string sql = "select * from Turmas";
                command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader(); //Lê cada registo

                while (reader.Read())

                    ListaTurmas.Add(new Turmas
                    {
                        Num_Turma = (int)reader["NumTurma"],
                        Nome_Turma =(string)reader["Nome_Turma"],
                        cod_Disciplina = (int)reader["cod_disciplina"],
                        Descrição = (string)reader["descrição"]
                     
                    });


                connection.Close();
                return ListaTurmas;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro");
                return null;
            }
        }

        public List<Disciplinas> GetDataDisciplinas()
        {
            var path = @"Data\Escola.sqlite";
            List<Disciplinas> ListaDisciplinas= new List<Disciplinas>();
            try
            {
                connection = new SQLiteConnection("DataSource=" + path);
                connection.Open();
                string sql = "select * from Disciplinas";
                command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader(); //Lê cada registo

                while (reader.Read())

                    ListaDisciplinas.Add(new Disciplinas
                    {
                        Num_Disciplinas = (int)reader["NumDisciplina"],
                        Nome_Disciplina = (string)reader["Nome_Disciplina"],
                        Descrição_Disciplina = (string)reader["Descricao_Disciplina"],
                        cod_Turma = (int)reader["codTurma"],
                        cod_Professor = (int)reader["codprofessor"]

                    });


                connection.Close();
                return ListaDisciplinas;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro");
                return null;
            }
        }  //Vai buscar todas as disciplinas existentes à Base de Dados

        public List<Professores> GetDataProfessores()
        {
            var path = @"Data\Escola.sqlite";
            List<Professores> ListaProfessores = new List<Professores>();
            try
            {
                connection = new SQLiteConnection("DataSource=" + path);
                connection.Open();
                string sql = "select * from Professores";
                command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader(); //Lê cada registo

                while (reader.Read())

                    ListaProfessores.Add(new Professores
                    {
                       Num_Professor = (int)reader["NumProfessor"],
                       Primeiro_Nome = (string)reader["PrimeiroNome"],
                       Ultimo_Nome = (string)reader["UltNome"],
                       Genero = (string)reader["Genero"],
                       curriculo = (string)reader["Curriculo"]

                    });


                connection.Close();
                return ListaProfessores;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro");
                return null;
            }
        }  //Vai buscar todos os professores existentes à Base de Dados

        public List<Notas> GetDataNotas()
        {
            var path = @"Data\Escola.sqlite";
            List<Notas> ListaNotas = new List<Notas>();
            try
            {
                connection = new SQLiteConnection("DataSource=" + path);
                connection.Open();
                string sql = "select * from Notas";
                command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader(); //Lê cada registo

                while (reader.Read())

                    ListaNotas.Add(new Notas
                    {
                        Num_Nota = (int)reader["NumNota"],
                        Id_Aluno = (int)reader["id_aluno"],
                        Id_Disciplina = (int)reader["id_disciplina"],
                        Nota = (double)reader["nota"],
                        Anotações = (string)reader["anotacoes"]

                    });


                connection.Close();
                return ListaNotas;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro");
                return null;
            }
        }  //Vai buscar todas as notas existentes à Base de Dados


    }




}

