using System;
using System.IO;
using Microsoft.Win32;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Escolha uma opção de inventário:");
        Console.WriteLine("1. Inventário de Hardware");
        Console.WriteLine("2. Inventário de Aplicativos Instalados");
        Console.WriteLine("3. Inventário de Hardware e Aplicativos Instalados");

        string opcao = Console.ReadLine();

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
                    Console.WriteLine("Opção selecionada: Inventário de Aplicativos Instalados");
                    inventario = ObterInventarioAplicativos();
                    break;

                case "3":
                    Console.WriteLine("Opção selecionada: Inventário de Hardware e Aplicativos Instalados");
                    inventario = ObterInventarioHardware() + "\n\n" + ObterInventarioAplicativos();
                    break;

                default:
                    Console.WriteLine("Opção inválida.");
                    return;
            }

            Console.WriteLine("\n--- Inventário ---\n");
            Console.WriteLine(inventario);

            File.WriteAllText(caminhoTxt, inventario);
            Console.WriteLine($"\nInventário salvo em: {caminhoTxt}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex.Message}\nDetalhes: {ex.StackTrace}");
        }
    }

    static string ObterInventarioHardware()
    {
        return "Funcionalidade de inventário de hardware não implementada neste exemplo.";
    }

    static string ObterInventarioAplicativos()
    {
        string aplicativos = "Inventário de Aplicativos Instalados:\n";
        try
        {
            // Consultar aplicativos de 64 bits no registro
            aplicativos += ConsultarRegistro(Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");

            // Consultar aplicativos de 32 bits no registro
            aplicativos += ConsultarRegistro(Registry.LocalMachine, @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall");

            // Consultar aplicativos do usuário atual
            aplicativos += ConsultarRegistro(Registry.CurrentUser, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
        }
        catch (Exception ex)
        {
            aplicativos += $"Erro ao acessar informações de aplicativos: {ex.Message}\nDetalhes: {ex.StackTrace}\n";
        }

        return aplicativos;
    }

    static string ConsultarRegistro(RegistryKey baseKey, string subKeyPath)
    {
        string result = "";
        try
        {
            using (RegistryKey subKey = baseKey.OpenSubKey(subKeyPath))
            {
                if (subKey != null)
                {
                    foreach (var appName in subKey.GetSubKeyNames())
                    {
                        using (RegistryKey appKey = subKey.OpenSubKey(appName))
                        {
                            if (appKey != null)
                            {
                                string displayName = appKey.GetValue("DisplayName")?.ToString() ?? "Nome indisponível";
                                string version = appKey.GetValue("DisplayVersion")?.ToString() ?? "Versão indisponível";
                                string publisher = appKey.GetValue("Publisher")?.ToString() ?? "Fornecedor indisponível";

                                result += $"Nome: {displayName}, Versão: {version}, Fornecedor: {publisher}\n";
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            result += $"Erro ao acessar o registro: {ex.Message}\n";
        }

        return result;
    }
}
