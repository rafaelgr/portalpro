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
	public partial class Usuario
	{
		private int _usuarioId;
		public virtual int UsuarioId 
		{ 
		    get
		    {
		        return this._usuarioId;
		    }
		    set
		    {
		        this._usuarioId = value;
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
		
		private string _login;
		public virtual string Login 
		{ 
		    get
		    {
		        return this._login;
		    }
		    set
		    {
		        this._login = value;
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
		
		private string _password;
		public virtual string Password 
		{ 
		    get
		    {
		        return this._password;
		    }
		    set
		    {
		        this._password = value;
		    }
		}
		
		private GrupoUsuario _grupoUsuario;
		public virtual GrupoUsuario GrupoUsuario 
		{ 
		    get
		    {
		        return this._grupoUsuario;
		    }
		    set
		    {
		        this._grupoUsuario = value;
		    }
		}
		
		private IList<WebApiTicket> _webApiTickets = new List<WebApiTicket>();
		public virtual IList<WebApiTicket> WebApiTickets 
		{ 
		    get
		    {
		        return this._webApiTickets;
		    }
		}
		
		private IList<SolicitudLog> _solicitudLogs = new List<SolicitudLog>();
		public virtual IList<SolicitudLog> SolicitudLogs 
		{ 
		    get
		    {
		        return this._solicitudLogs;
		    }
		}
		
	}
}
