using MySql.Data.MySqlClient;
using Mysqlx.Cursor;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContaBancariaSql
{
    internal class Inserir
    {
            static string Conexao = "server=localhost;database=Conta;user=root;password=Sua_senha";
        public static void Insercao(string nome, string cpf, DateTime dataDeNascimento, double saldoInicial, double caixinha)
        {
            using (MySqlConnection conn = new MySqlConnection(Conexao))
            {
                try
                {
                    conn.Open();
                    string sqlCliente = "INSERT INTO Cliente (Nome_Titular, Cpf, Data_Nascimento) " +
                                        "VALUES (@nome, @cpf, @dataDeNascimento); SELECT LAST_INSERT_ID();";

                    int clienteId;
                    using (MySqlCommand cmd = new MySqlCommand(sqlCliente, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@cpf", cpf);
                        cmd.Parameters.AddWithValue("@dataDeNascimento", dataDeNascimento);

                        clienteId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    string sqlVerifica = "SELECT COUNT(*) FROM Conta_Bancaria WHERE Clienteid=@clienteId";
                    using (MySqlCommand cmdVerifica = new MySqlCommand(sqlVerifica, conn))
                    {
                        cmdVerifica.Parameters.AddWithValue("@clienteId", clienteId);
                        int qtd = Convert.ToInt32(cmdVerifica.ExecuteScalar());

                        if (qtd > 0)
                        {
                            Console.WriteLine("Esse cliente já possui uma conta!");
                            return;
                        }
                    }
                    string sqlConta = "INSERT INTO Conta_Bancaria (Saldo, Caixinha, Clienteid) " +
                                      "VALUES (@saldo, @caixinha, @clienteId)";
                    using (MySqlCommand cmdConta = new MySqlCommand(sqlConta, conn))
                    {
                        cmdConta.Parameters.AddWithValue("@saldo", saldoInicial);
                        cmdConta.Parameters.AddWithValue("@caixinha", caixinha);
                        cmdConta.Parameters.AddWithValue("@clienteId", clienteId);

                        cmdConta.ExecuteNonQuery();
                    }
                    Console.WriteLine("Cliente e conta cadastrados com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao cadastrar: " + ex.Message);
                }
            }
        }
    }
}
