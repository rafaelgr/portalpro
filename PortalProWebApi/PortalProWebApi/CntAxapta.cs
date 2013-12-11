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
            ImportEmpresa impEmpresa = new ImportEmpresa();
            AsyncLaunchEmpresa caller = new AsyncLaunchEmpresa(impEmpresa.LaunchEmpresa);
            IAsyncResult result = caller.BeginInvoke(out threadId, null, null);
            return true;
        }

        public static bool ImportarProveedors()
        {
            int threadId;
            ImportProveedor impProveedor = new ImportProveedor();
            //impProveedor.LaunchProveedor(out threadId);
            AsyncLaunchProveedor caller = new AsyncLaunchProveedor(impProveedor.LaunchProveedor);
            IAsyncResult result = caller.BeginInvoke(out threadId, null, null);
            return true;
        }
        public static bool ImportarResponsables()
        {
            int threadId;
            ImportResponsable impResponsable = new ImportResponsable();
            AsyncLaunchResponsable caller = new AsyncLaunchResponsable(impResponsable.LaunchResponsable);
            IAsyncResult result = caller.BeginInvoke(out threadId, null, null);
            return true;
        }
    }
}