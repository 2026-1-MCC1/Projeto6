using UnityEngine;

// Armazena os resultados finais do jogo para serem acessados entre cenas
public class GameResults : MonoBehaviour
{
    // Pontuação total final
    public static int ScoreFinal;

    // Quantidade de cada tipo de bolo produzido
    public static int BoloEspecial;
    public static int BoloChocolate;
    public static int BoloMorango;
    public static int BoloSimples;

    // Quantidade de ingredientes coletados
    public static int Trigo;
    public static int Ovo;
    public static int Leite;
    public static int Chocolate;
    public static int Morango;

    // Quantidade de ingredientes restantes após as receitas
    public static int TrigoRestante;
    public static int OvoRestante;
    public static int LeiteRestante;
    public static int ChocolateRestante;
    public static int MorangoRestante;
}