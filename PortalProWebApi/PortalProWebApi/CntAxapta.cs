using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalProWebApi
{
    public static class CntAxapta
    {
        public static bool ImportarEmpresas()
        {
            int threadId;
            ImportSqlEmpresa impEmpresa = new ImportSqlEmpresa();
            AsyncLaunchSqlEmpresa caller = new AsyncLaunchSqlEmpresa(impEmpresa.LaunchEmpresa);
            IAsyncResult result = caller.BeginInvoke(out threadId, null, null);
            return true;
        }

        public static bool ImportarProveedors()
        {
            int threadId;
            ImportSqlProveedor impProveedor = new ImportSqlProveedor();
            //impProveedor.LaunchProveedor(out threadId);
            AsyncLaunchSqlProveedor caller = new AsyncLaunchSqlProveedor(impProveedor.LaunchProveedor);
            IAsyncResult result = caller.BeginInvoke(out threadId, null, null);
            return true;
        }
        public static bool ImportarResponsables()
        {
            int threadId;
            ImportSqlResponsable impResponsable = new ImportSqlResponsable();
            AsyncLaunchSqlResponsable caller = new AsyncLaunchSqlResponsable(impResponsable.LaunchResponsable);
            IAsyncResult result = caller.BeginInvoke(out threadId, null, null);
            return true;
        }
        public static bool ImportarPedidos()
        {
            int threadId;
            ImportSqlPedido impPedido = new ImportSqlPedido();
            AsyncLaunchSqlPedido caller = new AsyncLaunchSqlPedido(impPedido.LaunchPedido);
            IAsyncResult result = caller.BeginInvoke(out threadId, null, null);
            return true;
        }

        public static bool ImportarFacturas()
        {
            int threadId;
            ImportSqlFactura impFactura = new ImportSqlFactura();
            AsyncLaunchSqlFactura caller = new AsyncLaunchSqlFactura(impFactura.LaunchFactura);
            IAsyncResult result = caller.BeginInvoke(out threadId, null, null);
            return true;
        }
    }
}