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
	public partial class CabFactura
	{
		private int _cabFacturaId;
		public virtual int CabFacturaId
		{
			get
			{
				return this._cabFacturaId;
			}
			set
			{
				this._cabFacturaId = value;
			}
		}
		
		private DateTime _fechaAlta;
		public virtual DateTime FechaAlta
		{
			get
			{
				return this._fechaAlta;
			}
			set
			{
				this._fechaAlta = value;
			}
		}
		
		private DateTime _fechaEmision;
		public virtual DateTime FechaEmision
		{
			get
			{
				return this._fechaEmision;
			}
			set
			{
				this._fechaEmision = value;
			}
		}
		
		private decimal _totalFactura;
		public virtual decimal TotalFactura
		{
			get
			{
				return this._totalFactura;
			}
			set
			{
				this._totalFactura = value;
			}
		}
		
		private string _numFactura;
		public virtual string NumFactura
		{
			get
			{
				return this._numFactura;
			}
			set
			{
				this._numFactura = value;
			}
		}
		
		private string _estado;
		public virtual string Estado
		{
			get
			{
				return this._estado;
			}
			set
			{
				this._estado = value;
			}
		}
		
		private Proveedor _proveedor;
		public virtual Proveedor Proveedor
		{
			get
			{
				return this._proveedor;
			}
			set
			{
				this._proveedor = value;
			}
		}
		
		private Documento _documento;
		public virtual Documento DocumentoPdf
		{
			get
			{
				return this._documento;
			}
			set
			{
				this._documento = value;
			}
		}
		
		private Documento _documento1;
		public virtual Documento DocumentoXml
		{
			get
			{
				return this._documento1;
			}
			set
			{
				this._documento1 = value;
			}
		}
		
		private IList<LinFactura> _linFacturas = new List<LinFactura>();
		public virtual IList<LinFactura> LinFacturas
		{
			get
			{
				return this._linFacturas;
			}
		}
		
	}
}
#pragma warning restore 1591
