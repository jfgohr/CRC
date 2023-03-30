namespace CRC
{
    public static class GrauDoPolinomioUtil
    {
        public static int ObterTamanhoDoArray(GrauDoPolinomio grauDoPolinomio)
        {
            if (grauDoPolinomio.Equals(GrauDoPolinomio._48))
                return 48;

            if (grauDoPolinomio.Equals(GrauDoPolinomio._32))
                return 32;

            return 16;
        }

        public static GrauDoPolinomio ObterGrauDoPolinomio(string primeiroItemDoPolinomio)
        {
            return primeiroItemDoPolinomio.Equals("48")
                ? GrauDoPolinomio._48
                : primeiroItemDoPolinomio.Equals("32")
                    ? GrauDoPolinomio._32
                    : primeiroItemDoPolinomio.Equals("16")
                        ? GrauDoPolinomio._16
                        : GrauDoPolinomio.invalido;
        }
    }


    public enum GrauDoPolinomio
    {
        _16 = 16,
        _32 = 32,
        _48 = 48,
        invalido = -1
    };
}