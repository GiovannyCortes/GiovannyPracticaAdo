using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiovannyAndresCortesHernandez.Models {
    public class Cliente {

        public string CodigoCliente { get; set; }
        public string Empresa { get; set; }
        public string Contacto { get; set; }
        public string Cargo { get; set; }
        public string Ciudad { get; set; }
        public int Telefono { get; set; }

        public Cliente () {
            this.CodigoCliente = null;
            this.Empresa = null;
            this.Contacto = null;
            this.Cargo = null;
            this.Ciudad = null;
            this.Telefono = -1;
        }

        public Cliente(string codigoCliente, string empresa, string contacto, string cargo, string ciudad, int telefono) {
            this.CodigoCliente = codigoCliente;
            this.Empresa = empresa;
            this.Contacto = contacto;
            this.Cargo = cargo;
            this.Ciudad = ciudad;
            this.Telefono = telefono;
        }
    }
}
