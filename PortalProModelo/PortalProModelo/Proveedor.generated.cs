#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the ClassGenerator.ttinclude code generation file.
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
	public partial class Proveedor
	{
		private int _proveedorId;
		public virtual int ProveedorId
		{
			get
			{
				return this._proveedorId;
			}
			set
			{
				this._proveedorId = value;
			}
		}
		
		private string _nombreComercial;
		public virtual string NombreComercial
		{
			get
			{
				return this._nombreComercial;
			}
			set
			{
				this._nombreComercial = value;
			}
		}
		
		private string _direccion;
		public virtual string Direccion
		{
			get
			{
				return this._direccion;
			}
			set
			{
				this._direccion = value;
			}
		}
		
		private string _localidad;
		public virtual string Localidad
		{
			get
			{
				return this._localidad;
			}
			set
			{
				this._localidad = value;
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
		
		private string _razonSocial;
		public virtual string RazonSocial
		{
			get
			{
				return this._razonSocial;
			}
			set
			{
				this._razonSocial = value;
			}
		}
		
		private string _personaContacto;
		public virtual string PersonaContacto
		{
			get
			{
				return this._personaContacto;
			}
			set
			{
				this._personaContacto = value;
			}
		}
		
		private string _emailFacturas;
		public virtual string EmailFacturas
		{
			get
			{
				return this._emailFacturas;
			}
			set
			{
				this._emailFacturas = value;
			}
		}
		
		private string _iBAN;
		public virtual string IBAN
		{
			get
			{
				return this._iBAN;
			}
			set
			{
				this._iBAN = value;
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
		
		private string _actividadPrincipal;
		public virtual string ActividadPrincipal
		{
			get
			{
				return this._actividadPrincipal;
			}
			set
			{
				this._actividadPrincipal = value;
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
		
		private IList<Documento> _documentos = new List<Documento>();
		public virtual IList<Documento> Documentos
		{
			get
			{
				return this._documentos;
			}
		}
		
		private IList<CabFactura> _cabFacturas = new List<CabFactura>();
		public virtual IList<CabFactura> CabFacturas
		{
			get
			{
				return this._cabFacturas;
			}
		}
		
	}
}
#pragma warning restore 1591
