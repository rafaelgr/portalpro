﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the ContextGenerator.ttinclude code generation file.
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
	public partial class PortalProContext : OpenAccessContext, IPortalProContextUnitOfWork
	{
		private static string connectionStringName = @"PortalproConnection";
			
		private static BackendConfiguration backend = GetBackendConfiguration();
				
		private static MetadataSource metadataSource = XmlMetadataSource.FromAssemblyResource("PortalProEntities.rlinq");
		
		public PortalProContext()
			:base(connectionStringName, backend, metadataSource)
		{ }
		
		public PortalProContext(string connection)
			:base(connection, backend, metadataSource)
		{ }
		
		public PortalProContext(BackendConfiguration backendConfiguration)
			:base(connectionStringName, backendConfiguration, metadataSource)
		{ }
			
		public PortalProContext(string connection, MetadataSource metadataSource)
			:base(connection, backend, metadataSource)
		{ }
		
		public PortalProContext(string connection, BackendConfiguration backendConfiguration, MetadataSource metadataSource)
			:base(connection, backendConfiguration, metadataSource)
		{ }
			
		public IQueryable<GrupoUsuario> GrupoUsuarios 
		{
			get
			{
				return this.GetAll<GrupoUsuario>();
			}
		}
		
		public IQueryable<Usuario> Usuarios 
		{
			get
			{
				return this.GetAll<Usuario>();
			}
		}
		
		public IQueryable<WebApiTicket> WebApiTickets 
		{
			get
			{
				return this.GetAll<WebApiTicket>();
			}
		}
		
		public IQueryable<TipoDocumento> TipoDocumentos 
		{
			get
			{
				return this.GetAll<TipoDocumento>();
			}
		}
		
		public IQueryable<GrupoProveedor> GrupoProveedors 
		{
			get
			{
				return this.GetAll<GrupoProveedor>();
			}
		}
		
		public IQueryable<TipoDocumentoGrupoProveedor> TipoDocumentoGrupoProveedors 
		{
			get
			{
				return this.GetAll<TipoDocumentoGrupoProveedor>();
			}
		}
		
		public IQueryable<Proveedor> Proveedors 
		{
			get
			{
				return this.GetAll<Proveedor>();
			}
		}
		
		public IQueryable<Documento> Documentos 
		{
			get
			{
				return this.GetAll<Documento>();
			}
		}
		
		public IQueryable<SolicitudProveedor> SolicitudProveedors 
		{
			get
			{
				return this.GetAll<SolicitudProveedor>();
			}
		}
		
		public IQueryable<Plantilla> Plantillas 
		{
			get
			{
				return this.GetAll<Plantilla>();
			}
		}
		
		public IQueryable<SolicitudStatus> SolicitudStatus 
		{
			get
			{
				return this.GetAll<SolicitudStatus>();
			}
		}
		
		public IQueryable<SolicitudLog> SolicitudLogs 
		{
			get
			{
				return this.GetAll<SolicitudLog>();
			}
		}
		
		public IQueryable<CabFactura> CabFacturas 
		{
			get
			{
				return this.GetAll<CabFactura>();
			}
		}
		
		public IQueryable<LinFactura> LinFacturas 
		{
			get
			{
				return this.GetAll<LinFactura>();
			}
		}
		
		public IQueryable<UsuarioProveedor> UsuarioProveedors 
		{
			get
			{
				return this.GetAll<UsuarioProveedor>();
			}
		}
		
		public IQueryable<Pedido> Pedidos 
		{
			get
			{
				return this.GetAll<Pedido>();
			}
		}
		
		public IQueryable<LinPedido> LinPedidos 
		{
			get
			{
				return this.GetAll<LinPedido>();
			}
		}
		
		public IQueryable<Parametro> Parametros1 
		{
			get
			{
				return this.GetAll<Parametro>();
			}
		}
		
		public IQueryable<Empresa> Empresas 
		{
			get
			{
				return this.GetAll<Empresa>();
			}
		}
		
		public IQueryable<Responsable> Responsables 
		{
			get
			{
				return this.GetAll<Responsable>();
			}
		}
		
		public IQueryable<Progresos> Progresos 
		{
			get
			{
				return this.GetAll<Progresos>();
			}
		}
		
		public static BackendConfiguration GetBackendConfiguration()
		{
			BackendConfiguration backend = new BackendConfiguration();
			backend.Backend = "MySql";
			backend.ProviderName = "MySql.Data.MySqlClient";
			backend.Logging.MetricStoreSnapshotInterval = 0;
			return backend;
		}
	}
	
	public interface IPortalProContextUnitOfWork : IUnitOfWork
	{
		IQueryable<GrupoUsuario> GrupoUsuarios
		{
			get;
		}
		IQueryable<Usuario> Usuarios
		{
			get;
		}
		IQueryable<WebApiTicket> WebApiTickets
		{
			get;
		}
		IQueryable<TipoDocumento> TipoDocumentos
		{
			get;
		}
		IQueryable<GrupoProveedor> GrupoProveedors
		{
			get;
		}
		IQueryable<TipoDocumentoGrupoProveedor> TipoDocumentoGrupoProveedors
		{
			get;
		}
		IQueryable<Proveedor> Proveedors
		{
			get;
		}
		IQueryable<Documento> Documentos
		{
			get;
		}
		IQueryable<SolicitudProveedor> SolicitudProveedors
		{
			get;
		}
		IQueryable<Plantilla> Plantillas
		{
			get;
		}
		IQueryable<SolicitudStatus> SolicitudStatus
		{
			get;
		}
		IQueryable<SolicitudLog> SolicitudLogs
		{
			get;
		}
		IQueryable<CabFactura> CabFacturas
		{
			get;
		}
		IQueryable<LinFactura> LinFacturas
		{
			get;
		}
		IQueryable<UsuarioProveedor> UsuarioProveedors
		{
			get;
		}
		IQueryable<Pedido> Pedidos
		{
			get;
		}
		IQueryable<LinPedido> LinPedidos
		{
			get;
		}
		IQueryable<Parametro> Parametros1
		{
			get;
		}
		IQueryable<Empresa> Empresas
		{
			get;
		}
		IQueryable<Responsable> Responsables
		{
			get;
		}
		IQueryable<Progresos> Progresos
		{
			get;
		}
	}
}
#pragma warning restore 1591
