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
	public partial class TipoDocumentoGrupoProveedor
	{
		private int _tipoDocumentoGrupoProveedorId;
		public virtual int TipoDocumentoGrupoProveedorId 
		{ 
		    get
		    {
		        return this._tipoDocumentoGrupoProveedorId;
		    }
		    set
		    {
		        this._tipoDocumentoGrupoProveedorId = value;
		    }
		}
		
		private TipoDocumento _tipoDocumento;
		public virtual TipoDocumento TipoDocumento 
		{ 
		    get
		    {
		        return this._tipoDocumento;
		    }
		    set
		    {
		        this._tipoDocumento = value;
		    }
		}
		
		private GrupoProveedor _grupoProveedor;
		public virtual GrupoProveedor GrupoProveedor 
		{ 
		    get
		    {
		        return this._grupoProveedor;
		    }
		    set
		    {
		        this._grupoProveedor = value;
		    }
		}
		
	}
}
