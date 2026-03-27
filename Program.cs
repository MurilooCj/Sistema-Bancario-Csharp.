using ContaBancariaSql;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;


class Program
{
    static void Main(string[] args)
    {
        int idDigitado = 0;
        Console.Write("Já possui uma conta?");
        string opcaoLog=Console.ReadLine();
        if (opcaoLog.ToLower() == "s")
        {
            Console.WriteLine("Digite o id da conta:");
            if (!int.TryParse(Console.ReadLine(), out idDigitado))
            {
                Console.WriteLine("id inválido");
                return;
            }
            try
            {
                Login.ContaExistente(idDigitado);
                Console.WriteLine("login efetuado com sucesso");
            }
            catch (Exception ex) {
                Console.WriteLine("conta não encontrada");
                return;
            }
        }
        else if (opcaoLog.ToLower() == "n")
        {
            Console.WriteLine("ok, vamos criar uma!");
            Console.Write("Insira onome do titular da conta bancaria:");
            string nome = Console.ReadLine();
            Console.Write("digite o cpf do titular:");
            string cpf = Console.ReadLine();
            DateTime DataNascimento;
            while (true)
            {
                Console.Write("digite a data de nascimento do titular:(yyyy/MM/dd)");
                string datanascimentoStr = Console.ReadLine();
                try
                {
                    DataNascimento = DateTime.Parse(datanascimentoStr);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("data inválida digite novamente!");
                }
            }
            Console.Write("digite o Saldo inicial em sua conta:");
            if(!double.TryParse(Console.ReadLine(),out double saldoinicial))
            {
                Console.WriteLine("valor inválido");
                return;
            }

            Console.Write("deseja guardar parte do seu saldo?");
            string opcao1 = Console.ReadLine();
            double caixinha = 0;

            if (opcao1.ToLower() == "s")
            {
                Console.Write("quanto deseja guardar?:");
                if (!double.TryParse(Console.ReadLine(), out  caixinha))
                {
                    Console.WriteLine("Valor inválido");
                    return;
                }
            }
            Inserir.Insercao(nome, cpf, DataNascimento, saldoinicial, caixinha);
            Console.WriteLine("conta criada com sucesso!faça login para acessar!");
            return;
        }

             Console.Write("Deseja realizar um deposito?(S/N):"); 
             string opcao=Console.ReadLine();
        if (opcao.ToLower() == "s")
        {
                Console.Write("quanto será depositado em sua conta?");
                if (!double.TryParse(Console.ReadLine(), out double valor))
                {
                    Console.WriteLine("Valor inválido!");
                return;
                }  
            
            try { 
                Conta.Deposito(idDigitado, valor);
                Console.WriteLine("deposito realizado com sucesso!");
            }
            catch (Exception ex) {
                Console.WriteLine("Não foi possivel realizar o deposito"); }
        }
        Console.Write("deseja realizar uma transferencia?(S/N):");
        string opcao2 = Console.ReadLine();
        if (opcao2.ToLower() == "s")
        {
            
            Console.Write("Digite o valor a ser transferido:");
            if  (!double.TryParse(Console.ReadLine(),out double valortransf))
            {
                Console.WriteLine("Valor inválido");
                return;
            }
            Console.Write("digite o id que irá receber a transferencia:");
            if(!int.TryParse(Console.ReadLine(),out int idreceptor))
            {
                Console.WriteLine("id receptor inválido");
                return;
            }
            try
            {
                Conta.Transferencia(idDigitado, valortransf, idreceptor);
                Console.WriteLine("transferencia realizada com sucesso");
            }catch(Exception ex)
            {
                Console.WriteLine("erro transferencia não foi realizada" + ex.Message);
                return;
            }
            Console.Write("deseja guardar parte do seu dinheiro(S/N)");
            string opcao3= Console.ReadLine();  
            if (opcao3.ToLower() == "s")
            {
                Console.Write("qual o valor que irá ser guardado:");
                if (!double.TryParse(Console.ReadLine(),out double valorcaixinha))
                {
                    Console.WriteLine("valor inválido");
                    return;
                }
                Conta.Caixinha(idDigitado, valorcaixinha);
            }

        }
    }
}
