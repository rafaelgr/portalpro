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
	public partial class Documento
	{
		private int _documentoId;
		public virtual int DocumentoId
		{
			get
			{
				return this._documentoId;
			}
			set
			{
				this._documentoId = value;
			}
		}
		
		private string _nomFichero;
		public virtual string NomFichero
		{
			get
			{
				return this._nomFichero;
			}
			set
			{
				this._nomFichero = value;
			}
		}
		
		private string _extension;
		public virtual string Extension
		{
			get
			{
				return this._extension;
			}
			set
			{
				this._extension = value;
			}
		}
		
		private string _descargaUrl;
		public virtual string DescargaUrl
		{
			get
			{
				return this._descargaUrl;
			}
			set
			{
				this._descargaUrl = value;
			}
		}
		
		private string _comentario;
		public virtual string Comentario
		{
			get
			{
				return this._comentario;
			}
			set
			{
				this._comentario = value;
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
		
		private SolicitudProveedor _solicitudProveedor;
		public virtual SolicitudProveedor SolicitudProveedor
		{
			get
			{
				return this._solicitudProveedor;
			}
			set
			{
				this._solicitudProveedor = value;
			}
		}
		
		private IList<Pedido> _pedidos1 = new List<Pedido>();
		public virtual IList<Pedido> PedidosXml
		{
			get
			{
				return this._pedidos1;
			}
		}
		
		private IList<CabFactura> _cabFacturas = new List<CabFactura>();
		public virtual IList<CabFactura> CabFacturasXml
		{
			get
			{
				return this._cabFacturas;
			}
		}
		
		private IList<CabFactura> _cabFacturas1 = new List<CabFactura>();
		public virtual IList<CabFactura> CabFacturasPdf
		{
			get
			{
				return this._cabFacturas1;
			}
		}
		
		private IList<Pedido> _pedidos = new List<Pedido>();
		public virtual IList<Pedido> PedidosPdf
		{
			get
			{
				return this._pedidos;
			}
		}
		
	}
}
#pragma warning restore 1591
