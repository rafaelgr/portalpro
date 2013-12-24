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
	public partial class Pedido
	{
		private int _pedidoId;
		public virtual int PedidoId
		{
			get
			{
				return this._pedidoId;
			}
			set
			{
				this._pedidoId = value;
			}
		}
		
		private DateTime? _fechaAlta;
		public virtual DateTime? FechaAlta
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
		
		private decimal _totalPedido;
		public virtual decimal TotalPedido
		{
			get
			{
				return this._totalPedido;
			}
			set
			{
				this._totalPedido = value;
			}
		}
		
		private decimal _totalFacturado;
		public virtual decimal TotalFacturado
		{
			get
			{
				return this._totalFacturado;
			}
			set
			{
				this._totalFacturado = value;
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
		
		private string _tipoPedido;
		public virtual string TipoPedido
		{
			get
			{
				return this._tipoPedido;
			}
			set
			{
				this._tipoPedido = value;
			}
		}
		
		private DateTime? _fechaLimite;
		public virtual DateTime? FechaLimite
		{
			get
			{
				return this._fechaLimite;
			}
			set
			{
				this._fechaLimite = value;
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
		
		private Empresa _empresa;
		public virtual Empresa Empresa
		{
			get
			{
				return this._empresa;
			}
			set
			{
				this._empresa = value;
			}
		}
		
		private Responsable _responsable;
		public virtual Responsable Responsable
		{
			get
			{
				return this._responsable;
			}
			set
			{
				this._responsable = value;
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
		
		private IList<LinPedido> _linPedidos = new List<LinPedido>();
		public virtual IList<LinPedido> LinPedidos
		{
			get
			{
				return this._linPedidos;
			}
		}
		
	}
}
#pragma warning restore 1591
