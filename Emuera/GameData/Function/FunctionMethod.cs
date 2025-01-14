﻿using System;
using System.Collections.Generic;
using System.Text;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.Sub;

namespace MinorShift.Emuera.GameData.Function
{
	internal abstract class FunctionMethod
	{
		public Type ReturnType { get; protected set; }
		protected Type[] argumentTypeArray;

		//引数の数・型が一致するかどうかのテスト
		//正しくない場合はエラーメッセージを返す。
		//引数の数が不定である場合や引数の省略を許す場合にはoverrideすること。
		public virtual string CheckArgumentType(string name, IOperandTerm[] arguments)
		{
			if (arguments.Length != argumentTypeArray.Length)
				return name + "関数の引数の数が正しくありません";
			for (int i = 0; i < argumentTypeArray.Length; i++)
			{
				if (arguments[i] == null)
					return name + "関数の" + (i + 1).ToString() + "番目の引数は省略できません";
				if (argumentTypeArray[i] != arguments[i].GetOperandType())
					return name + "関数の" +(i+1).ToString()+ "番目の引数の型が正しくありません";
			}
			return null;
		}
		
		//Argumentが全て定数の時にMethodを解体してよいかどうか。RANDやCharaを参照するものなどは不可
		public bool CanRestructure { get; protected set; }

		//FunctionMethodが固有のRestructure()を持つかどうか
		public bool HasUniqueRestructure { get; protected set; }

		//実際の計算。
		public virtual Int64 GetIntValue(ExpressionMediator exm, IOperandTerm[] arguments) { throw new ExeEE("戻り値の型が違う or 未実装"); }
		public virtual string GetStrValue(ExpressionMediator exm, IOperandTerm[] arguments, bool tryTranslate = false) { throw new ExeEE("戻り値の型が違う or 未実装"); }
		public virtual SingleTerm GetReturnValue(ExpressionMediator exm, IOperandTerm[] arguments, bool tryTranslate)
		{
			if (ReturnType == typeof(Int64))
				return new SingleTerm(GetIntValue(exm, arguments));
			else
				return new SingleTerm(GetStrValue(exm, arguments));
		}

		/// <summary>
		/// 戻り値は全体をRestructureできるかどうか
		/// </summary>
		/// <param name="exm"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public virtual bool UniqueRestructure(ExpressionMediator exm, IOperandTerm[] arguments)
		{ throw new ExeEE("未実装？"); }
		
	}
}
