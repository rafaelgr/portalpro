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
	public partial class LinFactura
	{
		private int _linFacturaId;
		public virtual int LinFacturaId
		{
			get
			{
				return this._linFacturaId;
			}
			set
			{
				this._linFacturaId = value;
			}
		}
		
		private string _numeroPedido;
		public virtual string NumeroPedido
		{
			get
			{
				return this._numeroPedido;
			}
			set
			{
				this._numeroPedido = value;
			}
		}
		
		private string _descripcion;
		public virtual string Descripcion
		{
			get
			{
				return this._descripcion;
			}
			set
			{
				this._descripcion = value;
			}
		}
		
		private decimal _importe;
		public virtual decimal Importe
		{
			get
			{
				return this._importe;
			}
			set
			{
				this._importe = value;
			}
		}
		
		private decimal _porcentajeIva;
		public virtual decimal PorcentajeIva
		{
			get
			{
				return this._porcentajeIva;
			}
			set
			{
				this._porcentajeIva = value;
			}
		}
		
		private int _numLineaPedido;
		public virtual int NumLineaPedido
		{
			get
			{
				return this._numLineaPedido;
			}
			set
			{
				this._numLineaPedido = value;
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
		
		private int _numLineaFactura;
		public virtual int NumLineaFactura
		{
			get
			{
				return this._numLineaFactura;
			}
			set
			{
				this._numLineaFactura = value;
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
		
		private string _inventTransId;
		public virtual string InventTransId
		{
			get
			{
				return this._inventTransId;
			}
			set
			{
				this._inventTransId = value;
			}
		}
		
		private CabFactura _cabFactura;
		public virtual CabFactura CabFactura
		{
			get
			{
				return this._cabFactura;
			}
			set
			{
				this._cabFactura = value;
			}
		}
		
	}
}
#pragma warning restore 1591
