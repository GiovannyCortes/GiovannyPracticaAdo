using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GiovannyAndresCortesHernandez.Repositories;
using GiovannyAndresCortesHernandez.Models;

namespace GiovannyAndresCortesHernandez {
    public partial class FormPractica : Form {

        #region VARIABLES
        private RepositoryCliPed repo;
        private List<Pedido> listado_pedidos;
        private string current_cod_cliente;
        #endregion

        public FormPractica() {
            InitializeComponent();
            this.repo = new RepositoryCliPed();
            this.LoadClientes();
        }

        public void LoadClientes() {
            this.cmbclientes.Items.Clear();
            List<string> listado_empresas = this.repo.GetEmpresas();
            foreach (string empresa in listado_empresas) {
                this.cmbclientes.Items.Add(empresa);
            }
            this.cmbclientes.SelectedIndex = 0;
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e) {
            string nombre_cliente = this.cmbclientes.SelectedItem.ToString();
            Cliente select_cliente = this.repo.GetDatosCliente(nombre_cliente);
            this.current_cod_cliente = select_cliente.CodigoCliente.ToString();
            this.CargaDatosCliente(select_cliente);
            this.CargaPedidosCliente(select_cliente.CodigoCliente);
        }

        private void CargaDatosCliente(Cliente select_cliente) {
            this.txtempresa.Text = select_cliente.Empresa.ToString();
            this.txtcontacto.Text = select_cliente.Contacto.ToString();
            this.txtcargo.Text = select_cliente.Cargo.ToString();
            this.txtciudad.Text = select_cliente.Ciudad.ToString();
            this.txttelefono.Text = select_cliente.Telefono.ToString();
        }

        private void CargaPedidosCliente(string cod_cliente) {
            this.lstpedidos.Items.Clear();
            this.listado_pedidos = this.repo.GetPedidosCliente(cod_cliente);
            foreach (Pedido ped in this.listado_pedidos) {
                this.lstpedidos.Items.Add(ped.CodigoPedido.ToString());
            }
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.lstpedidos.SelectedItems.Count > 0) {
                int position = this.lstpedidos.SelectedIndex;
                Pedido current_pedido = this.listado_pedidos[position];
                if (current_pedido != null) {
                    this.txtcodigopedido.Text = current_pedido.CodigoPedido.ToString();
                    this.txtfechaentrega.Text = current_pedido.FechaEntrega.ToString();
                    this.txtformaenvio.Text = current_pedido.FormaEnvio.ToString();
                    this.txtimporte.Text = current_pedido.Importe.ToString();
                }
            }
        }

        private void btnmodificarcliente_Click(object sender, EventArgs e) {
            string codcliente = this.current_cod_cliente.ToString();
            string empresa = this.txtempresa.Text.ToString();
            string contacto = this.txtcontacto.Text.ToString();
            string cargo = this.txtcargo.Text.ToString();
            string ciudad = this.txtciudad.Text.ToString();
            int telefono = int.Parse(this.txttelefono.Text.ToString());

            this.repo.UpdateCliente(codcliente, empresa, contacto, cargo, ciudad, telefono);
            this.LoadClientes();
        }

        private void btnnuevopedido_Click(object sender, EventArgs e) {
            string codpedido = this.txtcodigopedido.Text.ToString();
            string codcliente = this.current_cod_cliente.ToString();
            DateTime fecha = DateTime.Parse(this.txtfechaentrega.Text.ToString());
            string formaenvio = this.txtformaenvio.Text.ToString();
            int importe = int.Parse(this.txtimporte.Text.ToString());

            this.repo.InsertPedido(codpedido, codcliente, fecha, formaenvio, importe);
            this.CargaPedidosCliente(this.current_cod_cliente);
        }

        private void btneliminarpedido_Click(object sender, EventArgs e) {
            string codpedido = this.lstpedidos.SelectedItem.ToString();
            this.repo.DeletePedido(codpedido);
            this.CargaPedidosCliente(this.current_cod_cliente);
        }
    }
}
