using System.Net;

namespace VillaMgic_API.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsExitoso  { get; set; }= true; 

        public List<string> ErrorMensaje{ get; set; }
        public object Resultado { get; set; }
    }
}
