using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortalProModelo;

namespace PortalProWebApi
{
    public static partial class PortalProWebUtility
    {
        /// <summary>
        /// Calcula el importe que se puede facturar en el mes de la fecha
        /// contra el pedido de suscripción pasado. Si este importe es cero
        /// no se puede facturar. Si el pedido no es de suscripción devuelve
        /// cero igualmente. Si hay fecha límite y se ha sobrepasado 
        /// devuelve cero también.
        /// </summary>
        /// <param name="pedido">Posible pedido de suscripción</param>
        /// <param name="fecha">Fecha de la factura</param>
        /// <param name="ctx">Contexto OpenAccess</param>
        /// <returns>Importe pediente de facturar para el mes de la fecha</returns>
        public static decimal PedidoSuscripcionImporteFacturable(Pedido pedido, DateTime fecha, PortalProContext ctx)
        {
            decimal r = 0;
            // si no es un pedido de suscripción no lo tratamos.
            if (pedido.TipoPedido != "SUSCRIPCION")
                return r;
            // si hemos superado la fecha límite, tampoco facturamos
            // solo comprobamos si la fecha no es nula
            if (pedido.FechaLimite != null && (fecha > pedido.FechaLimite))
                return r;
            // calcular el primer y último dia del mes
            DateTime primerDiaDelMes = new DateTime(fecha.Year, fecha.Month, 1);
            DateTime ultimoDiaDelMes = primerDiaDelMes.AddMonths(1).AddDays(-1);
            // comprobar el total facturado contra ese pedido
            // en ese mes
            var rs = from lf in ctx.LinFacturas
                     where lf.NumeroPedido == pedido.NumPedido &&
                           (lf.CabFactura.FechaEmision >= primerDiaDelMes && lf.CabFactura.FechaEmision <= ultimoDiaDelMes)
                     select lf;
            decimal totalFacturado = rs.AsEnumerable().Sum(x => x.Importe);
            decimal totalPedido = pedido.TotalPedido;
            // ahora la comprobación devolvemos pendiente
            if (totalFacturado < totalPedido)
                r = (totalPedido - totalFacturado);
            return r;
        }

        /// <summary>
        /// Devueleve lo que queda por facturar de un pedido de suscripción, excluyendo
        /// la factura pasada
        /// </summary>
        /// <param name="pedido">Pedido de suscripción</param>
        /// <param name="factura">Factura de referencia</param>
        /// <param name="ctx">Contexto OpenAccess</param>
        /// <returns>Importe pendiente o cero si no se puede facturar.</returns>
        public static decimal PedidoSuscripcionImporteFacturable(Pedido pedido, CabFactura factura, PortalProContext ctx)
        {
            decimal r = 0;
            DateTime fecha = (DateTime)factura.FechaEmision;
            // si no es un pedido de suscripción no lo tratamos.
            if (pedido.TipoPedido != "SUSCRIPCION")
                return r;
            // si hemos superado la fecha límite, tampoco facturamos
            // solo comprobamos si la fecha no es nula
            if (pedido.FechaLimite != null && (fecha > pedido.FechaLimite))
                return r;
            // calcular el primer y último dia del mes
            DateTime primerDiaDelMes = new DateTime(fecha.Year, fecha.Month, 1);
            DateTime ultimoDiaDelMes = primerDiaDelMes.AddMonths(1).AddDays(-1);
            // comprobar el total facturado contra ese pedido
            // en ese mes
            var rs = from lf in ctx.LinFacturas
                     where lf.NumeroPedido == pedido.NumPedido &&
                           (lf.FechaEmision >= primerDiaDelMes && lf.FechaEmision <= ultimoDiaDelMes)
                           && lf.CabFactura.CabFacturaId != factura.CabFacturaId
                     select lf;
            decimal totalFacturado = rs.AsEnumerable().Sum(x => x.Importe);
            decimal totalPedido = pedido.TotalPedido;
            // ahora la comprobación devolvemos pendiente
            if (totalFacturado < totalPedido)
                r = (totalPedido - totalFacturado);
            return r;
        }

        /// <summary>
        /// Verifica si en la lista pasado hay algun pedido
        /// de suscripción
        /// </summary>
        /// <param name="pedidos">Lista de pedidos</param>
        /// <returns>true = hay - false = no hay</returns>
        public static bool PedidoSuscripcionHayUno(IList<Pedido> pedidos)
        {
            bool r = false;
            // buscamos en la lista si hay alguno
            foreach (Pedido p in pedidos)
            {
                if (p.TipoPedido == "SUSCRIPCION") r = true;
            }
            return r;
        }

        /// <summary>
        /// A partir de los identificadores de una lista de pedidos
        /// verifica si en la lista hay alguno de suscripción
        /// </summary>
        /// <param name="pedidoIds">Lista de identificadores de pedidos</param>
        /// <param name="ctx">Contexto OpenAccess</param>
        /// <returns></returns>
        public static bool PedidoSuscripcionHayUno(int[] pedidoIds, PortalProContext ctx)
        {
            bool r = false;
            IList<Pedido> pedidos = new List<Pedido>();
            foreach (int id in pedidoIds)
            {
                Pedido pedido = (from p in ctx.Pedidos
                                 where p.PedidoId == id
                                 select p).FirstOrDefault<Pedido>();
                if (pedido != null) pedidos.Add(pedido);
            }
            r = PedidoSuscripcionHayUno(pedidos);
            return r;
        }

        /// <summary>
        /// Genera una factura de una sola linea con el importe
        /// pasado y a partir del pedido indicado.
        /// </summary>
        /// <param name="pedido">Pedido para el que se genera la factura</param>
        /// <param name="importe">Importe a facturar</param>
        /// <param name="ctx">Contexto OpenAccess</param>
        /// <returns></returns>
        public static CabFactura PedidoSuscripcionGenerarFactura(Pedido pedido, decimal importe, PortalProContext ctx)
        {
            CabFactura factura = null;
            // evitamos que se cuele un importe a 0
            if (importe == 0) return factura;
            // el pedido debe tener lineas
            if (pedido.LinPedidos.Count < 1) return factura;
            // creamos la factura.
            factura = new CabFactura();
            factura.FechaAlta = DateTime.Now;
            factura.TotalFactura = importe;
            factura.Estado = "RECIBIDA";
            factura.Generada = true;
            factura.Proveedor = pedido.Proveedor;
            factura.Empresa = pedido.Empresa;
            factura.Responsable = pedido.Responsable;
            ctx.Add(factura);
            ctx.SaveChanges();
            // creamos una línea única con el importe
            LinFactura lfactura = new LinFactura();
            lfactura.CabFactura = factura;
            lfactura.NumeroPedido = pedido.NumPedido;
            lfactura.Importe = importe;
            // usamos la primera línea que obtengamos del pedido
            // como modelo
            LinPedido lpedido = pedido.LinPedidos[0];
            lfactura.Descripcion = lpedido.Descripcion;
            lfactura.PorcentajeIva = lpedido.PorcentajeIva;
            lfactura.NumeroPedido = lpedido.NumPedido;
            lfactura.NumLineaPedido = lpedido.NumLinea;
            lpedido.Facturado += importe;
            if (factura.FechaEmision != null)
                lfactura.FechaEmision = (DateTime)factura.FechaEmision;
            ctx.Add(lfactura);
            ctx.SaveChanges();
            return factura;
        }

        public static string ComprobarLineaFacturaContraPedidoSuscripcion(CabFactura factura, LinFactura l, PortalProContext ctx)
        {
            string m = "";
            // (1) comprobar que el pedido existe
            Pedido p = (from ped in ctx.Pedidos
                        where ped.NumPedido == l.NumeroPedido
                        select ped).FirstOrDefault<Pedido>();
            if (p == null)
            {
                m = String.Format("El pedido {0} no existe.", l.NumeroPedido);
                return m;
            }
            // ahora se compara contra línea, luego hay que buscar la línea correspondiente
            LinPedido lp = (from linped in ctx.LinPedidos
                            where linped.NumPedido == l.NumeroPedido
                            && linped.NumLinea == l.NumLineaPedido
                            select linped).FirstOrDefault<LinPedido>();
            if (lp == null)
            {
                m = String.Format("El linea {0} del pedido {1} no existe.", l.NumLineaPedido, l.NumeroPedido);
                return m;
            }
            // Obtener parámetros y datos de proveedor para verificar márgenes
            Parametro parametro = (from par in ctx.Parametros1
                                   where par.ParametroId == 1
                                   select par).FirstOrDefault<Parametro>();
            Proveedor proveedor = p.Proveedor;
            // comprobamos las reglas para pedido de suscripcion
            decimal importeAFacturar = PedidoSuscripcionImporteFacturable(p, factura, ctx);
            if (importeAFacturar == 0)
            {
                m = String.Format("NO se puede facturar con esta fecha e importe contra el pedido de suscripción {0}", p.NumPedido);
                return m;
            }
            l.NumeroPedido = p.NumPedido;
            l.NumLineaPedido = lp.NumLinea;
            l.Importe = importeAFacturar;
            l.Descripcion = lp.Descripcion;
            l.PorcentajeIva = lp.PorcentajeIva;
            // actualizar empresa y responsables
            factura.Empresa = p.Empresa;
            factura.Responsable = p.Responsable;
            factura.TotalFactura += l.Importe;
            l.CabFactura = factura;
            // antes de salir conmprobamos si el total facturado supera
            // el margen de control PDF
            if (factura.TotalFactura > parametro.MaxImportePdf)
            {
                factura.Estado = "RECIBIDA2";
                factura.Historial += String.Format("{0:dd/MM/yyyy hh:mm:ss} La factura {1} pasa a estado INCIDENCIA debido a que su total {2} supera al margen de control PDF {4} <br/>",
                    DateTime.Now, factura.NumFactura, factura.TotalFactura, factura.Estado, parametro.MaxImportePdf);
            }
            ctx.Add(l);
            ctx.SaveChanges();
            return m;
        }
    }
}