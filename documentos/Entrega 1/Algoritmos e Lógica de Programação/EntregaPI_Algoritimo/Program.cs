using System;

class Program
{
    static void Main(string[] args)
    {
        int energia = 3;        // Quantidade de erros permitidos
        int energiaMax = 3;     // Valor máximo da barra de vida
        int pontos = 0;         // Pontuação acumulada
        int fase = 1;           // Controle de progressão do jogo
        bool jogoRodando = true; // Controla o loop principal

        Console.WriteLine("=== LABORATÓRIO TRANCADO ===\n");

        // HISTÓRIA
        Console.WriteLine("Você é um estudante de TI que acabou dormindo no laboratório...");
        Console.WriteLine("Ao acordar, percebe que tudo está trancado");
        Console.WriteLine("Resolva os desafios para sobreviver e escapar.\n");

        Console.WriteLine("Pressione ENTER para começar...");
        Console.ReadLine(); // pausa inicial para o jogador ler

        // Executa continuamente até vitória, derrota ou saída manual
        while (jogoRodando)
        {
            // Atualiza o HUD
            MostrarHUD(energia, energiaMax, pontos, fase);

            // CONDIÇÃO DE DERROTA
            if (energia <= 0)
            {
                Console.WriteLine("\nGAME OVER!");
                break; // encerra o jogo
            }

            Console.WriteLine("\nDigite 'sair' ou ENTER para continuar:");
            string escolha = Console.ReadLine();

            // Evita erro de null e trata saída do jogador
            if (escolha != null && escolha.ToLower() == "sair")
            {
                Console.WriteLine("Você desistiu...");
                break;
            }

            // CONTROLE DAS FASES
            switch (fase)
            {
                case 1:
                    // Executa fase 1 e aplica regra de acerto/erro
                    if (Fase1()) { pontos += 10; fase++; }
                    else energia--;
                    break;

                case 2:
                    if (Fase2()) { pontos += 20; fase++; }
                    else energia--;
                    break;

                case 3:
                    if (Fase3()) { pontos += 30; fase++; }
                    else energia--;
                    break;

                case 4:
                    if (Fase4()) { pontos += 40; fase++; }
                    else energia--;
                    break;

                case 5:
                    if (Fase5()) { pontos += 50; fase++; }
                    else energia--;
                    break;

                default:
                    // CONDIÇÃO DE VITÓRIA
                    Console.WriteLine("\nA porta se destranca lentamente...");
                    Console.WriteLine("Você finalmente escapa do laboratório!");
                    Console.WriteLine($"Pontuação final: {pontos}");
                    jogoRodando = false; // encerra o loop
                    break;
            }
        }

        Console.WriteLine("\nJogo encerrado.");
    }

    // INTERFACE DO JOGADOR
    // Exibe vida, pontos e fase atual
    static void MostrarHUD(int energia, int energiaMax, int pontos, int fase)
    {
        Console.Clear(); // limpa a tela para simular atualização

        string barra = "[";

        // Monta barra de vida dinamicamente
        for (int i = 0; i < energiaMax; i++)
        {
            if (i < energia)
                barra += "█"; // vida restante
            else
                barra += " "; // vida perdida
        }

        barra += "]";

        // Exibição do HUD
        Console.WriteLine("====== HUD ======");
        Console.WriteLine($"Vida   : {barra}");
        Console.WriteLine($"Pontos : {pontos}");
        Console.WriteLine($"Fase   : {fase}");
        Console.WriteLine("=================\n");
    }

    static bool Fase1()
    {
        // Contexto narrativo da fase
        Console.WriteLine("\n[Terminal iniciando...]");
        Console.WriteLine("Uma tela antiga acende com um desafio básico...");
        Console.WriteLine("Quanto é 2 + 2?");

        // Retorna true (acerto) ou false (erro)
        return Console.ReadLine() == "4";
    }

    static bool Fase2()
    {
        Console.WriteLine("\nVocê caminha até outro computador...");
        Console.WriteLine("Uma mensagem aparece na tela:");
        Console.WriteLine("Linguagem do Unity?");

        string r = Console.ReadLine().ToLower();

        // Aceita variações de resposta
        return r == "c#" || r == "csharp";
    }

    static bool Fase3()
    {
        Console.WriteLine("\nAs luzes piscam e um alerta soa...");
        Console.WriteLine("O sistema exige conhecimento em lógica:");
        Console.WriteLine("Estrutura de repetição condicional?");

        return Console.ReadLine().ToLower() == "while";
    }

    static bool Fase4()
    {
        Console.WriteLine("\nVocê encontra o servidor principal...");
        Console.WriteLine("Para avançar, precisa validar uma condição:");
        Console.WriteLine("Operador de igualdade em C#?");

        return Console.ReadLine() == "==";
    }

    static bool Fase5()
    {
        Console.WriteLine("\nA porta final está à sua frente...");
        Console.WriteLine("Último desafio do sistema:");
        Console.WriteLine("Qual tipo de dado representa verdadeiro ou falso em C#?");
        return Console.ReadLine().ToLower() == "bool";
    }
}