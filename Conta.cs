using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text;

namespace ContaBancariaSql
{
    internal class Conta
    {
        static string Conexao = "server=localhost;database=Conta;user=root;password=Sua_Senha";
        public static void Deposito(int idConta, double valor)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Conexao))
                {
                    conn.Open();

                    string sql = @"UPDATE Conta_Bancaria 
                           SET Saldo = Saldo + @valor
                           WHERE id_Conta = @id";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@valor", valor);
                    cmd.Parameters.AddWithValue("@id", idConta);

                    int linhas = cmd.ExecuteNonQuery();

                    if (linhas > 0)
                        Console.WriteLine("Depósito realizado com sucesso!");
                    else
                        Console.WriteLine("Conta não encontrada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao depositar: " + ex.Message);
            }
        }
        public static void Caixinha(int idConta, double valor)
        {
            using (MySqlConnection conn = new MySqlConnection(Conexao))
            {
                try
                {
                    conn.Open();

                    string sql = @"UPDATE Conta_Bancaria 
                           SET Saldo = Saldo - @valor,
                               Caixinha = Caixinha + @valor
                           WHERE id_Conta = @id";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@valor", valor);
                    cmd.Parameters.AddWithValue("@id", idConta);

                    int linhas = cmd.ExecuteNonQuery();

                    if (linhas > 0)
                        Console.WriteLine("Valor transferido para caixinha com sucesso!");
                    else
                        Console.WriteLine("Conta não encontrada.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }
            }

        }
        public static void Transferencia(int idContaOrigem, double valor, int idcontareceptora)
        {
            using (MySqlConnection conn = new MySqlConnection(Conexao))
            {
                conn.Open();
                MySqlTransaction transacao = conn.BeginTransaction();

                try
                {
                    string debito = @"UPDATE Conta_Bancaria 
                      SET Saldo = Saldo - @valor
                      WHERE id_Conta = @origem";

                    MySqlCommand comanddbt = new MySqlCommand(debito, conn, transacao);
                    comanddbt.Parameters.AddWithValue("@valor", valor);
                    comanddbt.Parameters.AddWithValue("@origem", idContaOrigem);

                    int linhasdbt = comanddbt.ExecuteNonQuery();

                    string credito = @"UPDATE Conta_Bancaria
                       SET Saldo = Saldo + @valor
                       WHERE id_Conta = @destino";

                    MySqlCommand comandcred = new MySqlCommand(credito, conn, transacao);
                    comandcred.Parameters.AddWithValue("@valor", valor);
                    comandcred.Parameters.AddWithValue("@destino", idcontareceptora);

                    int linhascred = comandcred.ExecuteNonQuery();

                    if (linhasdbt > 0 && linhascred > 0)
                    {
                        transacao.Commit();
                        
                    }
                    else
                    {
                        transacao.Rollback();
                       
                    }
                }
                catch (Exception)
                {
                    transacao.Rollback();
                   
                }
            }
        }
    }
}
         
        


