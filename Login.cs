using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContaBancariaSql
{
    internal class Login
    {
        static string conexao = "server=localhost;database=Conta;user=root;password=Sua_senha";
        public static bool ContaExistente(int idBanco)
        {
            using (MySqlConnection conn = new MySqlConnection(conexao))
            {
                conn.Open();
                string Sql = "SELECT id_Conta FROM Conta_Bancaria WHERE id_Conta=@id";
                MySqlCommand cmd = new MySqlCommand(Sql, conn);
                cmd.Parameters.AddWithValue("@id", idBanco);
                var resultado= cmd.ExecuteScalar();
                return resultado != null;
            }
        }
    }
}
