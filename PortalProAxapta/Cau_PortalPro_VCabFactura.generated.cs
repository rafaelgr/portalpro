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

namespace PortalProAxapta	
{
	public partial class Cau_PortalPro_VCabFactura
	{
		private string _aCCOUNTNUM;
		public virtual string ACCOUNTNUM
		{
			get
			{
				return this._aCCOUNTNUM;
			}
			set
			{
				this._aCCOUNTNUM = value;
			}
		}
		
		private string _iDEMPRESA;
		public virtual string IDEMPRESA
		{
			get
			{
				return this._iDEMPRESA;
			}
			set
			{
				this._iDEMPRESA = value;
			}
		}
		
		private string _iNVOICEID;
		public virtual string INVOICEID
		{
			get
			{
				return this._iNVOICEID;
			}
			set
			{
				this._iNVOICEID = value;
			}
		}
		
		private DateTime _iNVOICEDATE;
		public virtual DateTime INVOICEDATE
		{
			get
			{
				return this._iNVOICEDATE;
			}
			set
			{
				this._iNVOICEDATE = value;
			}
		}
		
		private decimal _iNVOICEAMOUNT;
		public virtual decimal INVOICEAMOUNT
		{
			get
			{
				return this._iNVOICEAMOUNT;
			}
			set
			{
				this._iNVOICEAMOUNT = value;
			}
		}
		
		private DateTime? _fECHAPAGO;
		public virtual DateTime? FECHAPAGO
		{
			get
			{
				return this._fECHAPAGO;
			}
			set
			{
				this._fECHAPAGO = value;
			}
		}
		
		private string _eSTADI;
		public virtual string ESTADI
		{
			get
			{
				return this._eSTADI;
			}
			set
			{
				this._eSTADI = value;
			}
		}
		
		private DateTime? _fECHAVENCIMIENTO;
		public virtual DateTime? FECHAVENCIMIENTO
		{
			get
			{
				return this._fECHAVENCIMIENTO;
			}
			set
			{
				this._fECHAVENCIMIENTO = value;
			}
		}
		
	}
}
#pragma warning restore 1591
