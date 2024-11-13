using System;
using System.Diagnostics;
using System.IO;
using System.Management;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Escolha uma opção de inventário:");
        Console.WriteLine("1. Inventário de Hardware");
        Console.WriteLine("2. Inventário de Software");
        Console.WriteLine("3. Inventário de Hardware e Software");

        // Lê a opção escolhida pelo usuário
        string opcao = Console.ReadLine();
        
        // Define o caminho de destino para os arquivos
        string caminhoDat = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "inventario.dat");
        string caminhoTxt = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "inventario.txt");

        try
        {
            string inventario = "";

            switch (opcao)
            {
                case "1":
                    Console.WriteLine("Opção selecionada: Inventário de Hardware");
                    inventario = ObterInventarioHardware();
                    break;
                
                case "2":
                    Console.WriteLine("Opção selecionada: Inventário de Software");
                    inventario = ObterInventarioSoftware();
                    break;

                case "3":
                    Console.WriteLine("Opção selecionada: Inventário de Hardware e Software");
                    inventario = ObterInventarioHardware() + "\n\n" + ObterInventarioSoftware();
                    break;

                default:
                    Console.WriteLine("Opção inválida.");
                    return;
            }

            // Exibe o inventário no console
            Console.WriteLine("\n--- Inventário ---\n");
            Console.WriteLine(inventario);

            // Salva o inventário nos arquivos .dat e .txt
            File.WriteAllText(caminhoDat, inventario);
            File.WriteAllText(caminhoTxt, inventario);

            // Mensagem de sucesso
            Console.WriteLine($"\nInventário salvo em: {caminhoDat} e {caminhoTxt}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Erro de acesso: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex.Message}");
        }
    }

    // Método para obter o inventário de hardware (processador, memória RAM e sistema operacional)
    static string ObterInventarioHardware()
    {
        string hardwareInfo = "Inventário de Hardware:\n";

        // Obter informações sobre o processador
        var cpu = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
        foreach (var item in cpu.Get())
        {
            hardwareInfo += $"Processador: {item["Name"]}\n";
        }

        // Obter informações sobre a memória RAM
        var ram = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
        foreach (var item in ram.Get())
        {
            hardwareInfo += $"Memória RAM: {item["Capacity"]} bytes\n";
        }

        // Obter informações sobre o sistema operacional
        var sistema = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
        foreach (var item in sistema.Get())
        {
            hardwareInfo += $"Sistema Operacional: {item["Caption"]}\n";
        }

        return hardwareInfo;
    }

    // Método para obter o inventário de software (aplicativos em execução)
    static string ObterInventarioSoftware()
    {
        string softwareInfo = "Inventário de Software (Aplicativos principais em execução):\n";

        // Obter aplicativos em execução que possuam uma janela principal
        Process[] processos = Process.GetProcesses();
        foreach (var processo in processos)
        {
            // Verifica se o processo tem uma janela principal, que indica que é um aplicativo com interface de usuário
            if (!string.IsNullOrEmpty(processo.MainWindowTitle))
            {
                softwareInfo += $"{processo.ProcessName}\n"; // Apenas o nome do aplicativo
            }
        }

        return softwareInfo;
    }
}
