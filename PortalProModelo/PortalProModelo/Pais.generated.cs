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
	public partial class Pais
	{
		private int _paisId;
		public virtual int PaisId
		{
			get
			{
				return this._paisId;
			}
			set
			{
				this._paisId = value;
			}
		}
		
		private string _codAx;
		public virtual string CodAx
		{
			get
			{
				return this._codAx;
			}
			set
			{
				this._codAx = value;
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
		
		private IList<Comunidad> _comunidads = new List<Comunidad>();
		public virtual IList<Comunidad> Comunidads
		{
			get
			{
				return this._comunidads;
			}
		}
		
	}
}
#pragma warning restore 1591