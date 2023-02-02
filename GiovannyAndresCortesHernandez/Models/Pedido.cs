using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiovannyAndresCortesHernandez.Models {
    public class Pedido {

        public string CodigoPedido { get; set; }
        public string CodigoCliente { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string FormaEnvio { get; set; }
        public int Importe { get; set; }

        public Pedido() {
            this.CodigoPedido = null;
            this.CodigoCliente = null;
            this.FechaEntrega = new DateTime();
            this.FormaEnvio = null;
            this.Importe = -1;
        }

        public Pedido(string codigoPedido, string codigoCliente, DateTime fechaEntrega, string formaEnvio, int importe) {
            this.CodigoPedido = codigoPedido;
            this.CodigoCliente = codigoCliente;
            this.FechaEntrega = fechaEntrega;
            this.FormaEnvio = formaEnvio;
            this.Importe = importe;
        }
    }
}
