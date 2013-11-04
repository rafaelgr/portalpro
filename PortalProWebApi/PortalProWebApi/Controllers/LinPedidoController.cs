using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;
using Telerik.OpenAccess.FetchOptimization;

namespace PortalProWebApi.Controllers
{
    public class LinPedidoController : ApiController
    {
        /// <summary>
        /// Devuelve las líneas que corresponden a un pedido determinado
        /// </summary>
        /// <param name="idFac">Identificador del pedido al que pertencen las líneas</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<LinPedido> GetLineas(int idFac, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    // comprobamos si existe la fcatura de referencia.
                    Pedido pedido = (from f in ctx.Pedidos
                                     where f.PedidoId == idFac
                                     select f).FirstOrDefault<Pedido>();
                    if (pedido != null)
                    {
                        IEnumerable<LinPedido> lineas = pedido.LinPedidos;
                        FetchStrategy fs = new FetchStrategy();
                        fs.LoadWith<LinPedido>(x => x.Pedido);
                        lineas = ctx.CreateDetachedCopy<IEnumerable<LinPedido>>(lineas, fs);
                        return lineas;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un pedido con el id proporcionado (LinPedido)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (LinPedido)"));
                }
            }
        }

        /// <summary>
        /// Devuelve la línea de pedido que coincide con el id proporcionado
        /// </summary>
        /// <param name="id">Identificador único de la línea</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual LinPedido Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    LinPedido linea = (from l in ctx.LinPedidos
                                       where l.LinPedidoId == id
                                       select l).FirstOrDefault<LinPedido>();
                    if (linea != null)
                    {
                        linea = ctx.CreateDetachedCopy<LinPedido>(linea, x => x.Pedido);
                        return linea;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un linea con el id proporcionado (LinPedido)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (LinPedido)"));
                }
            }
        }

        /// <summary>
        /// Crear una nueva línea de pedido
        /// </summary>
        /// <param name="LinPedido">Objeto a crear, el atributo LinPedidoId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorización (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual LinPedido Post(LinPedido linea, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (LinPedido)"));
                }
                // comprobar las precondiciones
                if (linea == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                int pedidoId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (linea.Pedido != null)
                {
                    pedidoId = linea.Pedido.PedidoId;
                    linea.Pedido = null;
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(linea);
                if (pedidoId != 0)
                {
                    linea.Pedido = (from f in ctx.Pedidos
                                    where f.PedidoId == pedidoId
                                    select f).FirstOrDefault<Pedido>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<LinPedido>(linea, x => x.Pedido);
            }
        }

        public virtual bool Put(int idPed, IEnumerable<LinPedido> lineas, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (LinPedido)"));
                }
                // comprobamos que las lineas no son nulas
                if (lineas == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // comprobamos que la factura a la que se asociarán las líneas existe
                Pedido pedido = (from f in ctx.Pedidos
                                  where f.PedidoId == idPed
                                  select f).FirstOrDefault<Pedido>();
                if (pedido == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No existe una factura con el id proporcionado (LinPedido)"));
                }
                // eliminamos las líneas de fcatura anteriores para solo dar de alta estas
                ctx.Delete(pedido.LinPedidos);
                // ahora damos de alta las nuevas lineas 
                decimal totalPedido = 0;
                foreach (LinPedido linea in lineas)
                {
                    LinPedido l = new LinPedido()
                    {
                        Descripcion = linea.Descripcion,
                        Importe = linea.Importe,
                        PorcentajeIva = linea.PorcentajeIva,
                        Pedido = pedido
                    };
                    totalPedido += linea.Importe;
                    ctx.Add(l);
                }
                pedido.TotalPedido = totalPedido;
                ctx.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// Modificar una linea de pedido. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único de la linea de pedido</param>
        /// <param name="linea">Linea de pedido con los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual LinPedido Put(int id, LinPedido linea, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (LinPedido)"));
                }
                // comprobar los formatos
                if (linea == null || id != linea.LinPedidoId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si uan linea con ese id existe
                LinPedido lin = (from l in ctx.LinPedidos
                                 where l.LinPedidoId == id
                                 select l).FirstOrDefault<LinPedido>();
                // existe?
                if (lin == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una linea con el id proporcionado (LinPedido)"));
                }
                int pedidoId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (lin.Pedido != null)
                {
                    pedidoId = lin.Pedido.PedidoId;
                    linea.Pedido = null;
                }
                // modificar el objeto
                ctx.AttachCopy<LinPedido>(linea);
                // volvemos a leer el objecto para que lo maneje este contexto.
                linea = (from l in ctx.LinPedidos
                         where l.LinPedidoId == id
                         select l).FirstOrDefault<LinPedido>();
                if (pedidoId != 0)
                {
                    linea.Pedido = (from f in ctx.Pedidos
                                    where f.PedidoId == pedidoId
                                    select f).FirstOrDefault<Pedido>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<LinPedido>(linea, x => x.Pedido);
            }
        }

        /// <summary>
        /// Elimina la linea que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificador de la linea a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (LinPedido)"));
                }
                // primero buscamos si un grupo con ese id existe
                LinPedido lin = (from l in ctx.LinPedidos
                                 where l.LinPedidoId == id
                                 select l).FirstOrDefault<LinPedido>();
                // existe?
                if (lin == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una linea con el id proporcionado (LinPedido)"));
                }
                ctx.Delete(lin);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}