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

namespace PortalProModelo	
{
	public partial class Progresos
	{
		private int _progresoId;
		public virtual int ProgresoId
		{
			get
			{
				return this._progresoId;
			}
			set
			{
				this._progresoId = value;
			}
		}
		
		private string _code;
		public virtual string Code
		{
			get
			{
				return this._code;
			}
			set
			{
				this._code = value;
			}
		}
		
		private int _numReg;
		public virtual int NumReg
		{
			get
			{
				return this._numReg;
			}
			set
			{
				this._numReg = value;
			}
		}
		
		private int _totReg;
		public virtual int TotReg
		{
			get
			{
				return this._totReg;
			}
			set
			{
				this._totReg = value;
			}
		}
		
	}
}
#pragma warning restore 1591