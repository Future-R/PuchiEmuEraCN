using System;
using System.Collections.Generic;
using System.Text;
using MinorShift.Emuera.GameData.Expression;

namespace MinorShift.Emuera.GameData.Function
{
	internal sealed class FunctionMethodTerm : IOperandTerm
	{
		public FunctionMethodTerm(FunctionMethod meth, IOperandTerm[] args)
			: base(meth.ReturnType)
		{
			method = meth;
			arguments = args;
		}

		private FunctionMethod method;
		private IOperandTerm[] arguments;

        public override long GetIntValue(ExpressionMediator exm)
        {
			return method.GetIntValue(exm, arguments);
        }
        public override string GetStrValue(ExpressionMediator exm, bool translate=false)
        {
			return method.GetStrValue(exm, arguments, translate);
        }
		public override SingleTerm GetValue(ExpressionMediator exm, bool tryTranslate =false)
		{
			return method.GetReturnValue(exm, arguments, tryTranslate);
		}
		
        public override IOperandTerm Restructure(ExpressionMediator exm, bool tryTranslate=false)
        {
			if (method.HasUniqueRestructure)
			{
				if (method.UniqueRestructure(exm, arguments) && method.CanRestructure)
					return GetValue(exm);
				return this;
			}
			bool argIsConst = true;
			for(int i = 0; i< arguments.Length;i++)
			{
				if(arguments[i] == null)
					continue;
				//Changes by Bartoum
				
				//This is a place to rework. This is where constant are translated
				arguments[i] = arguments[i].Restructure(exm, true);
				argIsConst &= arguments[i] is SingleTerm;
			}
			if ((method.CanRestructure) && (argIsConst))
				return GetValue(exm);
			return this;
			
        }
        
	}
}
