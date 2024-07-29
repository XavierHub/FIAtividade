using System.Collections.Generic;

namespace FI.AtividadeEntrevista.Utils
{
    public class Resultado<T>
    {
        public T Dados { get; set; }
        public List<Parametro> Parametros { get; set; }
    }
}
