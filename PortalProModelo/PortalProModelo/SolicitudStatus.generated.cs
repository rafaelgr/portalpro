#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Data.Common;
using Telerik.OpenAccess.Metadata.Fluent;
using Telerik.OpenAccess.Metadata.Fluent.Advanced;
using PortalProModelo;


namespace PortalProModelo	
{
	public partial class SolicitudStatus
	{
		private int _solicitudStatusId;
		public virtual int SolicitudStatusId 
		{ 
		    get
		    {
		        return this._solicitudStatusId;
		    }
		    set
		    {
		        this._solicitudStatusId = value;
		    }
		}
		
		private string _nombre;
		public virtual string Nombre 
		{ 
		    get
		    {
		        return this._nombre;
		    }
		    set
		    {
		        this._nombre = value;
		    }
		}
		
		private IList<SolicitudProveedor> _solicitudProveedors = new List<SolicitudProveedor>();
		public virtual IList<SolicitudProveedor> SolicitudProveedors 
		{ 
		    get
		    {
		        return this._solicitudProveedors;
		    }
		}
		
		private IList<SolicitudLog> _solicitudLogs = new List<SolicitudLog>();
		public virtual IList<SolicitudLog> SolicitudLogsIniciales 
		{ 
		    get
		    {
		        return this._solicitudLogs;
		    }
		}
		
		private IList<SolicitudLog> _solicitudLogs1 = new List<SolicitudLog>();
		public virtual IList<SolicitudLog> SolicitudLogsFinales 
		{ 
		    get
		    {
		        return this._solicitudLogs1;
		    }
		}
		
	}
}