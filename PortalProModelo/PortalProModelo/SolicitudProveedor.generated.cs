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
	public partial class SolicitudProveedor
	{
		private int _solicitudProveedorId;
		public virtual int SolicitudProveedorId 
		{ 
		    get
		    {
		        return this._solicitudProveedorId;
		    }
		    set
		    {
		        this._solicitudProveedorId = value;
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
		
		private string _calle;
		public virtual string Calle 
		{ 
		    get
		    {
		        return this._calle;
		    }
		    set
		    {
		        this._calle = value;
		    }
		}
		
		private string _ciudad;
		public virtual string Ciudad 
		{ 
		    get
		    {
		        return this._ciudad;
		    }
		    set
		    {
		        this._ciudad = value;
		    }
		}
		
		private string _codPostal;
		public virtual string CodPostal 
		{ 
		    get
		    {
		        return this._codPostal;
		    }
		    set
		    {
		        this._codPostal = value;
		    }
		}
		
		private string _provincia;
		public virtual string Provincia 
		{ 
		    get
		    {
		        return this._provincia;
		    }
		    set
		    {
		        this._provincia = value;
		    }
		}
		
		private string _comunidad;
		public virtual string Comunidad 
		{ 
		    get
		    {
		        return this._comunidad;
		    }
		    set
		    {
		        this._comunidad = value;
		    }
		}
		
		private string _pais;
		public virtual string Pais 
		{ 
		    get
		    {
		        return this._pais;
		    }
		    set
		    {
		        this._pais = value;
		    }
		}
		
		private string _telefono;
		public virtual string Telefono 
		{ 
		    get
		    {
		        return this._telefono;
		    }
		    set
		    {
		        this._telefono = value;
		    }
		}
		
		private string _fax;
		public virtual string Fax 
		{ 
		    get
		    {
		        return this._fax;
		    }
		    set
		    {
		        this._fax = value;
		    }
		}
		
		private string _movil;
		public virtual string Movil 
		{ 
		    get
		    {
		        return this._movil;
		    }
		    set
		    {
		        this._movil = value;
		    }
		}
		
		private string _url;
		public virtual string Url 
		{ 
		    get
		    {
		        return this._url;
		    }
		    set
		    {
		        this._url = value;
		    }
		}
		
		private string _email;
		public virtual string Email 
		{ 
		    get
		    {
		        return this._email;
		    }
		    set
		    {
		        this._email = value;
		    }
		}
		
		private string _nif;
		public virtual string Nif 
		{ 
		    get
		    {
		        return this._nif;
		    }
		    set
		    {
		        this._nif = value;
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
		
		private SolicitudStatus _solicitudStatus;
		public virtual SolicitudStatus SolicitudStatus 
		{ 
		    get
		    {
		        return this._solicitudStatus;
		    }
		    set
		    {
		        this._solicitudStatus = value;
		    }
		}
		
		private IList<Documento> _documentos = new List<Documento>();
		public virtual IList<Documento> Documentos 
		{ 
		    get
		    {
		        return this._documentos;
		    }
		}
		
	}
}
