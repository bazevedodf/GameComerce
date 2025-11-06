namespace GameCommerce.Aplicacao.Extensions
{
    public static class StringExtensions
    {

        /// <summary>
        /// Remove todos os caracteres não numéricos de uma string
        /// </summary>
        /// <param name="str">String de entrada</param>
        /// <returns>String contendo apenas números</returns>
        public static string ApenasNumeros(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return new string(str.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Formata uma string numérica como CPF (XXX.XXX.XXX-XX)
        /// </summary>
        public static string FormatarCpf(this string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return cpf;

            var numeros = cpf.ApenasNumeros();

            if (numeros.Length != 11)
                return cpf; // Retorna original se não tiver 11 dígitos

            return $"{numeros.Substring(0, 3)}.{numeros.Substring(3, 3)}.{numeros.Substring(6, 3)}-{numeros.Substring(9, 2)}";
        }
    }
}
