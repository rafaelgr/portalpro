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


namespace PortalProModelo	
{
	public partial class Plantilla
	{
		private int _plantillaId;
		public virtual int PlantillaId 
		{ 
		    get
		    {
		        return this._plantillaId;
		    }
		    set
		    {
		        this._plantillaId = value;
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
		
		private string _asunto;
		public virtual string Asunto 
		{ 
		    get
		    {
		        return this._asunto;
		    }
		    set
		    {
		        this._asunto = value;
		    }
		}
		
		private string _cuerpo;
		public virtual string Cuerpo 
		{ 
		    get
		    {
		        return this._cuerpo;
		    }
		    set
		    {
		        this._cuerpo = value;
		    }
		}
		
	}
}
