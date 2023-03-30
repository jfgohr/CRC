using Util;

var dadosValidos = false;
var mensagemErro = "";
var dado = "";
var dadoSplitado = Array.Empty<string>();
var polinomio = "";
var valoresDoPolinomio = Array.Empty<string>();
var grauDoPolinomio = GrauDoPolinomio.invalido;

while (!dadosValidos)
{
    Console.Clear();
    if(mensagemErro.Length > 0)
    {
        Console.WriteLine(mensagemErro);
        mensagemErro = "";
    }

    Console.WriteLine("Informe o dado a ser verificado (deve ter entre 32 e 128 bits)");
    dado = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(dado))
    {
        mensagemErro = "Informe o dado a ser verificado!";
        continue;
    }
    
    var reiniciarCiclo = false;
    for (int i = 0; i < dado.Length; i++)
    {
        if (dado[i].Equals('1') || dado[i].Equals('0'))
            continue;

        else
        {
            mensagemErro = "O dado informado não é \"binário\". Informe-o novamente";
            reiniciarCiclo = true;
            break;
        }
    }

    if (reiniciarCiclo)
        continue;

    if (dado.Length < 32)
    {
        mensagemErro = "O dado informado tem menos de 32 bits";
        continue;
    }

    if (dado.Length > 128)
    {
        mensagemErro = "O dado informado tem mais de de 128 bits";
        continue;
    }


    dadoSplitado = dado.Select(x => x.ToString()).ToArray();
    mensagemErro = "";
    dadosValidos = true;
}

dadosValidos = false;

while (!dadosValidos)
{
    Console.Clear();
    if (mensagemErro.Length > 0)
    {
        Console.WriteLine(mensagemErro);
        mensagemErro = "";
    }

    Console.WriteLine("O dado a ser verificado é o: " + dado);
    Console.WriteLine("Informe o polinômio:");
    polinomio = Console.ReadLine().ToLower();

    if (string.IsNullOrWhiteSpace(polinomio))
    {
        mensagemErro = "Informe o polinomio de verificação!";
        continue;
    }

    var itensParaSplit = new char[] { '+', '-', '*', '/' };

    valoresDoPolinomio = polinomio.Split(itensParaSplit).Select(x =>
    {
        var posicaoDoX = x.IndexOf("x");

        if (!string.IsNullOrWhiteSpace(x) && posicaoDoX == -1)
            return "0";

        var valor = x[(posicaoDoX + 1)..];
        if (!string.IsNullOrEmpty(valor))
            return valor;

        return "1";
    }).ToArray();

    var primeiroItemDoPolinomio = valoresDoPolinomio.First();
    grauDoPolinomio = GrauDoPolinomioUtil.ObterGrauDoPolinomio(primeiroItemDoPolinomio);

    if (grauDoPolinomio.Equals(GrauDoPolinomio.invalido))
    {
        mensagemErro = "Polinômio inválido. Deve ser utilizado um polinômio que inicia em potência de 16, 32 ou 48.";
        continue;
    }

    mensagemErro = "";
    dadosValidos = true;
}

var verificador = new int[GrauDoPolinomioUtil.ObterTamanhoDoArray(grauDoPolinomio) + 1].ToList();

foreach (var valor in valoresDoPolinomio)
{
    if (int.TryParse(valor, out var resultado))
    {
        var index = (int.Parse(valor) - verificador.Count) * -1;
        verificador[index - 1] = 1;
    }
}

Console.WriteLine("Verificador:" + string.Join(" ", verificador));

var dadoComZerosDoPolinomio = ObterDadoComZerosDoPolinomio();

Queue<int> resultadoDoXor = new();
List<Queue<int>> resultadosDosXors = new(); 

resultadoDoXor = ObterXorDosCampos(dadoComZerosDoPolinomio.ToArray(), verificador);
resultadosDosXors.Add(resultadoDoXor);

for (int i = verificador.Count; i < dadoComZerosDoPolinomio.Count; i++)
{
    if (resultadoDoXor.First() == 0)
    {
        resultadoDoXor.Enqueue(dadoComZerosDoPolinomio[i]);
        resultadoDoXor.Dequeue();

        if (resultadoDoXor.First() != 0 && resultadoDoXor.Count == verificador.Count)
        {
            resultadoDoXor = ObterXorDosCampos(resultadoDoXor.ToArray(), verificador);
            resultadosDosXors.Add(resultadoDoXor);
        }
    }
}

var resultadoFinal = resultadoDoXor;

Console.WriteLine($"Dividir: {Environment.NewLine}{string.Join("", dadoComZerosDoPolinomio)} / {string.Join("", verificador)}");

resultadoFinal.Dequeue();
Console.WriteLine($"Resultado (mensagem que será enviada): {Environment.NewLine}{dado}{string.Join("", resultadoFinal)}");

Console.ReadLine();


Queue<int> ObterXorDosCampos(int[] dadoParaDividir, List<int> verificador)
{
    var retorno = new int[verificador.Count];

    for (int i = 0; i < verificador.Count; i++)
        retorno[i] = dadoParaDividir[i] ^ verificador[i];

    return new Queue<int>(retorno);
}

List<int> ObterDadoComZerosDoPolinomio()
{
    List<int> retorno = dadoSplitado.Select(x => int.Parse(x)).ToList();

    for (int i = 0; i < (int)grauDoPolinomio-1; i++)
    {
        retorno.Add(0);
    }
    return retorno;
}