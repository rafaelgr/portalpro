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
	public partial class LinPedido
	{
		private int _linPedidoId;
		public virtual int LinPedidoId
		{
			get
			{
				return this._linPedidoId;
			}
			set
			{
				this._linPedidoId = value;
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
		
		private string _numPedido;
		public virtual string NumPedido
		{
			get
			{
				return this._numPedido;
			}
			set
			{
				this._numPedido = value;
			}
		}
		
		private int _numLinea;
		public virtual int NumLinea
		{
			get
			{
				return this._numLinea;
			}
			set
			{
				this._numLinea = value;
			}
		}
		
		private decimal _facturado;
		public virtual decimal Facturado
		{
			get
			{
				return this._facturado;
			}
			set
			{
				this._facturado = value;
			}
		}
		
		private DateTime? _fechaRecepcion;
		public virtual DateTime? FechaRecepcion
		{
			get
			{
				return this._fechaRecepcion;
			}
			set
			{
				this._fechaRecepcion = value;
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
		
		private Pedido _pedido;
		public virtual Pedido Pedido
		{
			get
			{
				return this._pedido;
			}
			set
			{
				this._pedido = value;
			}
		}
		
	}
}
#pragma warning restore 1591
