using System;

class StableMarriage
{
    const int N = 8; // número de homens e mulheres
    static int[,] wmr = new int[N, N]; // preferências das mulheres em relação aos homens
    static int[,] mwr = new int[N, N]; // preferências dos homens em relação às mulheres
    static int[,] rwm = new int[N, N]; // ranking das mulheres para os homens
    static int[,] rwmr = new int[N, N]; // ranking dos homens para as mulheres
    static int[] x = new int[N]; // mulher que está com cada homem
    static int[] y = new int[N]; // homem que está com cada mulher
    static bool[] single = new bool[N]; // status de solteira das mulheres

    static void Print()
    {
        int rm = 0, rw = 0;
        for (int m = 0; m < N; m++)
        {
            Console.Write($"{x[m] + 1,4}"); // Corrigido para exibir 1-based indexing
            rm += rwm[m, x[m]];
            rw += rwmr[x[m], m];
        }
        Console.WriteLine($"{rm,8}{rw,4}");
    }

    static bool Stable(int m)
    {
        int pm = x[m];
        int w = y[pm];
        int lim = mwr[m, w];
        for (int i = 0; i < lim; i++)
        {
            int pw = mwr[m, i];
            if (!single[pw] && rwm[pw, m] > rwm[pw, y[pw]])
            {
                return false;
            }
        }
        lim = rwmr[pm, w];
        for (int i = 0; i < lim; i++)
        {
            int pmW = wmr[pm, i];
            if (pmW < w && rwm[pm, w] > rwm[pm, y[pm]])
            {
                return false;
            }
        }
        return true;
    }

    static void Try(int m)
    {
        for (int r = 0; r < N; r++)
        {
            int w = wmr[m, r];
            if (single[w])
            {
                if (Stable(m))
                {
                    x[m] = w;
                    y[w] = m;
                    single[w] = false;
                    if (m < N - 1) Try(m + 1);
                    else Print();
                    single[w] = true;
                }
            }
        }
    }

    static void Main()
    {
        Console.WriteLine("Digite as preferências dos homens (Números separados por espaço):");
        for (int m = 0; m < N; m++)
        {
            string[] inputs = Console.ReadLine().Split();
            for (int r = 0; r < N; r++)
            {
                if (int.TryParse(inputs[r], out int val))
                {
                    wmr[m, r] = val - 1; // Corrigido para indexação baseada em 0
                    rwm[m, wmr[m, r]] = r;
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Certifique-se de digitar apenas números inteiros.");
                    return;
                }
            }
        }

        Console.WriteLine("Digite as preferências das mulheres (Números separados por espaço):");
        for (int w = 0; w < N; w++)
        {
            string[] inputs = Console.ReadLine().Split();
            for (int r = 0; r < N; r++)
            {
                if (int.TryParse(inputs[r], out int val))
                {
                    mwr[w, r] = val - 1; // Corrigido para indexação baseada em 0
                    rwmr[w, mwr[w, r]] = r;
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Certifique-se de digitar apenas números inteiros.");
                    return;
                }
            }
        }

        for (int w = 0; w < N; w++)
        {
            single[w] = true;
        }

        Try(0);
    }
}
