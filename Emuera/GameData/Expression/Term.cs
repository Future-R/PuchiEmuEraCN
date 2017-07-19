using System;
using System.Collections.Generic;
using System.Text;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameProc;

using MinorShift.Emuera.GameData.Variable;

namespace MinorShift.Emuera.GameData.Expression
{

    internal sealed class NullTerm : IOperandTerm 
    {
        public NullTerm(Int64 i)
            : base(typeof(Int64))
        {
        }

        public NullTerm(string s)
            : base(typeof(string))
        {
        }
    }

	/// <summary>
	/// 項。一単語だけ。
	/// </summary>
	internal sealed class SingleTerm : IOperandTerm
	{

        public SingleTerm(bool i)
            : base(typeof(Int64))
		{
			if (i)
				iValue = 1;
			else
				iValue = 0;
		}
        public SingleTerm(Int64 i)
            : base(typeof(Int64))
		{
			iValue = i;
		}
        public SingleTerm(string s)
            : base(typeof(string))
		{
			sValue = s;
		}
		readonly Int64 iValue;
		string sValue;

        public override long GetIntValue(ExpressionMediator exm)
        {
            return iValue;
        }
        public override string GetStrValue(ExpressionMediator exm, bool translate=false)
        {
            //Modified by Bartoum
            //PRINTS go through here.
            if (exm != null && exm.Process != null && exm.Process.getCurrentLine != null)
            {
                string name = exm.Process.getCurrentLine.ToString();
                name = Translation.searchParentFile(name);
                sValue = Translation.translate(sValue, name, translate);
            }
            return sValue;
        }
        public override SingleTerm GetValue(ExpressionMediator exm, bool tryTranslate =false)
        {
            return this;
        }
		public string Str
		{
			get
			{
                //チェック済みの上での呼び出し
                //if (type != typeof(string))
                //    throw new ExeEE("項の種別が異常");
				return sValue;
			}
            set // JVN: Set method needed to make things work smoother for PARAM stuff
            {
                this.sValue = value;
            }
		}

		public Int64 Int
		{
			get
			{
                //チェック済みの上での呼び出し
                //if (type != typeof(Int64))
                //    throw new ExeEE("項の種別が異常");
				return iValue;
			}
		}
		public override string ToString()
		{
			if (GetOperandType() == typeof(Int64))
				return iValue.ToString();
            if (GetOperandType() == typeof(string))
				return sValue.ToString();
			return base.ToString();
		}
		
        public override IOperandTerm Restructure(ExpressionMediator exm, bool tryTranslate=false)
        {
			return this;
        }
	}
	/// <summary>
	/// 項。一単語だけ。
	/// </summary>
	internal sealed class StrFormTerm : IOperandTerm
	{
		public StrFormTerm(StrForm sf)
			: base(typeof(string))
		{
			sfValue = sf;
		}
		readonly StrForm sfValue;

		public StrForm StrForm
		{
			get
			{
				return sfValue;
			}
		}

		public override string GetStrValue(ExpressionMediator exm, bool translate=false)
		{
            // Bartoum: If the current line containts <nonbutton (for HTML) we translate it.
            if (exm.Process.getCurrentLine.ToString().Contains("<nonbutton"))
            {
                translate = true;
            }
            return sfValue.GetString(exm, translate);
		}
		
		public override SingleTerm GetValue(ExpressionMediator exm, bool tryTranslate =false)
		{
			return new SingleTerm(sfValue.GetString(exm, tryTranslate));
		}
		
        public override IOperandTerm Restructure(ExpressionMediator exm, bool tryTranslate=false)
        {
			sfValue.Restructure(exm);
			if(sfValue.IsConst)
				return new SingleTerm(sfValue.GetString(exm, tryTranslate));
			IOperandTerm term = sfValue.GetIOperandTerm();
			if(term != null)
				return term;
			return this;
        }
	}

}
