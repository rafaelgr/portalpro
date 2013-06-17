﻿#pragma warning disable 1591
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

	}
}
#pragma warning restore 1591

