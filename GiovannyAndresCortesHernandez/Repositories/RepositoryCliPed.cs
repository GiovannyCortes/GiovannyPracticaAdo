using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using GiovannyAndresCortesHernandez.Helpers;
using GiovannyAndresCortesHernandez.Models;

#region PROCEDURES
/*
    CREATE PROCEDURE SP_CLIENTE_DATOS (@NOMBRE NVARCHAR(50)) AS 
	    SELECT * FROM CLIENTES WHERE EMPRESA = @NOMBRE
    GO

    CREATE PROCEDURE SP_PEDIDOS_CLIENTE (@CODCLIENTE NVARCHAR(50)) AS
	    SELECT PEDIDOS.* 
	    FROM PEDIDOS 
	    INNER JOIN CLIENTES 
	    ON PEDIDOS.CodigoCliente = CLIENTES.CodigoCliente
	    WHERE PEDIDOS.CodigoCliente = @CODCLIENTE
    GO

    CREATE PROCEDURE SP_UPDATE_CLIENTE 
    (@CODCLIENTE NVARCHAR(50), @EMPRESA NVARCHAR(50), @CONTACTO NVARCHAR(50), @CARGO NVARCHAR(50), @CIUDAD NVARCHAR(50), @TELEFONO INT) AS
	    UPDATE clientes 
	    SET Empresa = @EMPRESA, Contacto = @CONTACTO, Cargo = @CARGO, Ciudad = @CIUDAD, Telefono = @TELEFONO
	    WHERE CodigoCliente = @CODCLIENTE
    GO

    CREATE PROCEDURE SP_INSERT_PEDIDO 
    (@CODPEDIDO NVARCHAR(50), @CODCLIENTE NVARCHAR(50), @FECHA DATETIME, @FORMAENVIO NVARCHAR(50), @IMPORTE INT) AS 
	    INSERT INTO pedidos VALUES (@CODPEDIDO, @CODCLIENTE, @FECHA, @FORMAENVIO, @IMPORTE);
    GO

    CREATE PROCEDURE SP_DELETE_PEDIDO (@CODIGOPEDIDO NVARCHAR(50)) AS
	    DELETE FROM pedidos WHERE CodigoPedido = @CODIGOPEDIDO
    GO

 */
#endregion

#region VIEWS
/*
    CREATE VIEW V_EMPRESAS_CLIENTE AS
	    SELECT DISTINCT EMPRESA FROM CLIENTES
    GO
 */
#endregion

namespace GiovannyAndresCortesHernandez.Repositories {
    public class RepositoryCliPed {

        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;

        public RepositoryCliPed() {
            this.cn = new SqlConnection(HelperConfiguration.GetConnectionString());
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<string> GetEmpresas() {
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = "SELECT * FROM V_EMPRESAS_CLIENTE";

            this.cn.Open();
            List<string> listado_empresas = new List<string>();
            this.reader = this.com.ExecuteReader();
            while (this.reader.Read()) {
                listado_empresas.Add(this.reader["EMPRESA"].ToString());
            }

            this.cn.Close();
            this.reader.Close();
            return listado_empresas;
        }

        public Cliente GetDatosCliente(string nombre_cliente) {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTE_DATOS";

            SqlParameter pamname = new SqlParameter("@NOMBRE", nombre_cliente);
            this.com.Parameters.Add(pamname);

            this.cn.Open();
            Cliente newCliente = new Cliente();
            this.reader = this.com.ExecuteReader();
            while (this.reader.Read()) {
                newCliente = new Cliente(
                    this.reader["CodigoCliente"].ToString(),
                    this.reader["Empresa"].ToString(),
                    this.reader["Contacto"].ToString(),
                    this.reader["Cargo"].ToString(),
                    this.reader["Ciudad"].ToString(),
                    Convert.ToInt32(this.reader["Telefono"].ToString())
                );
            }

            this.cn.Close();
            this.reader.Close();
            this.com.Parameters.Clear();
            return newCliente;
        }

        public List<Pedido> GetPedidosCliente(string codcliente) {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_PEDIDOS_CLIENTE";

            SqlParameter pamcodcliente = new SqlParameter("@CODCLIENTE", codcliente);
            this.com.Parameters.Add(pamcodcliente);

            this.cn.Open();
            List<Pedido> listado_pedidos = new List<Pedido>();
            this.reader = this.com.ExecuteReader();
            while (this.reader.Read()) {
                Pedido newpedido = new Pedido(
                    this.reader["CodigoPedido"].ToString(),
                    this.reader["CodigoCliente"].ToString(),
                    DateTime.Parse(this.reader["FechaEntrega"].ToString()),
                    this.reader["FormaEnvio"].ToString(),
                    Convert.ToInt32(this.reader["Importe"].ToString())
                );
                listado_pedidos.Add(newpedido);
            }

            this.cn.Close();
            this.reader.Close();
            this.com.Parameters.Clear();
            return listado_pedidos;
        }

        public void UpdateCliente(string codcliente, string empresa, string contacto, string cargo, string ciudad, int telefono) {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_UPDATE_CLIENTE";

            SqlParameter pamcodcliente = new SqlParameter("@CODCLIENTE", codcliente);
            SqlParameter pamempresa = new SqlParameter("@EMPRESA", empresa);
            SqlParameter pamcontacto = new SqlParameter("@CONTACTO", contacto);
            SqlParameter pamcargo = new SqlParameter("@CARGO", cargo);
            SqlParameter pamciudad = new SqlParameter("@CIUDAD", ciudad);
            SqlParameter pamtelefono = new SqlParameter("@TELEFONO", telefono);

            this.com.Parameters.Add(pamcodcliente);
            this.com.Parameters.Add(pamempresa);
            this.com.Parameters.Add(pamcontacto);
            this.com.Parameters.Add(pamcargo);
            this.com.Parameters.Add(pamciudad);
            this.com.Parameters.Add(pamtelefono);

            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void InsertPedido(string codpedido, string codcliente, DateTime fecha, string formaenvio, int importe) {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_PEDIDO";

            SqlParameter pamcodpedido = new SqlParameter("@CODPEDIDO", codpedido);
            SqlParameter pamcodcliente = new SqlParameter("@CODCLIENTE", codcliente);
            SqlParameter pamfecha = new SqlParameter("@FECHA", fecha);
            SqlParameter pamformaenvio = new SqlParameter("@FORMAENVIO", formaenvio);
            SqlParameter pamimporte = new SqlParameter("@IMPORTE", importe);

            this.com.Parameters.Add(pamcodpedido);
            this.com.Parameters.Add(pamcodcliente);
            this.com.Parameters.Add(pamfecha);
            this.com.Parameters.Add(pamformaenvio);
            this.com.Parameters.Add(pamimporte);

            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void DeletePedido(string codpedido) {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_PEDIDO";

            SqlParameter pamcodpedido = new SqlParameter("@CODIGOPEDIDO", codpedido);
            this.com.Parameters.Add(pamcodpedido);

            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

    }
}
