﻿using System;
using System.Collections.Generic;
using System.Text;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameProc;
using MinorShift.Emuera.GameData.Variable;


namespace MinorShift.Emuera.GameData.Function
{
	internal static partial class FunctionMethodCreator
	{
		static FunctionMethodCreator()
		{
			methodList = new Dictionary<string, FunctionMethod>();
            //キャラクタデータ系
			methodList["GETCHARA"] = new GetcharaMethod();
			methodList["GETSPCHARA"] = new GetspcharaMethod();
			methodList["CSVNAME"] = new CsvStrDataMethod(CharacterStrData.NAME);
			methodList["CSVCALLNAME"] = new CsvStrDataMethod(CharacterStrData.CALLNAME);
			methodList["CSVNICKNAME"] = new CsvStrDataMethod(CharacterStrData.NICKNAME);
			methodList["CSVMASTERNAME"] = new CsvStrDataMethod(CharacterStrData.MASTERNAME);
			methodList["CSVCSTR"] = new CsvcstrMethod();
			methodList["CSVBASE"] = new CsvDataMethod(CharacterIntData.BASE);
			methodList["CSVABL"] = new CsvDataMethod(CharacterIntData.ABL);
			methodList["CSVMARK"] = new CsvDataMethod(CharacterIntData.MARK);
			methodList["CSVEXP"] = new CsvDataMethod(CharacterIntData.EXP);
			methodList["CSVRELATION"] = new CsvDataMethod(CharacterIntData.RELATION);
			methodList["CSVTALENT"] = new CsvDataMethod(CharacterIntData.TALENT);
			methodList["CSVCFLAG"] = new CsvDataMethod(CharacterIntData.CFLAG);
			methodList["CSVEQUIP"] = new CsvDataMethod(CharacterIntData.EQUIP);
			methodList["CSVJUEL"] = new CsvDataMethod(CharacterIntData.JUEL);
            methodList["FINDCHARA"] = new FindcharaMethod(false);
            methodList["FINDLASTCHARA"] = new FindcharaMethod(true);
            methodList["EXISTCSV"] = new ExistCsvMethod();

            //汎用処理系
			methodList["VARSIZE"] = new VarsizeMethod();
			methodList["CHKFONT"] = new CheckfontMethod();
			methodList["CHKDATA"] = new CheckdataMethod("CHKDATA", EraSaveFileType.Normal);
            methodList["ISSKIP"] = new IsSkipMethod();
			methodList["MOUSESKIP"] = new MesSkipMethod(true);
			methodList["MESSKIP"] = new MesSkipMethod(false);
            methodList["GETCOLOR"] = new GetColorMethod(false);
            methodList["GETDEFCOLOR"] = new GetColorMethod(true);
            methodList["CHKFONTEDGE"] = new CheckFontEdgeMethod();
            methodList["GETFONTEDGECOLOR"] = new GetFontEdgeColorMethod(false);
            methodList["GETDEFFONTEDGECOLOR"] = new GetFontEdgeColorMethod(true);
            methodList["GETFOCUSCOLOR"] = new GetFocusColorMethod();
            methodList["GETBGCOLOR"] = new GetBGColorMethod(false);
            methodList["GETDEFBGCOLOR"] = new GetBGColorMethod(true);
            methodList["GETSTYLE"] = new GetStyleMethod();
            methodList["GETFONT"] = new GetFontMethod();
            methodList["BARSTR"] = new BarStringMethod();
            methodList["CURRENTALIGN"] = new CurrentAlignMethod();
			methodList["CURRENTREDRAW"] = new CurrentRedrawMethod();
			methodList["COLOR_FROMNAME"] = new ColorFromNameMethod();
			methodList["COLOR_FROMRGB"] = new ColorFromRGBMethod();

			//TODO:1810
			//methodList["CHKVARDATA"] = new CheckdataStrMethod("CHKVARDATA", EraSaveFileType.Var);
			methodList["CHKCHARADATA"] = new CheckdataStrMethod("CHKCHARADATA", EraSaveFileType.CharVar);
			//methodList["CHKGLOBALDATA"] = new CheckdataMethod("CHKGLOBALDATA", EraSaveFileType.Global);
			//methodList["FIND_VARDATA"] = new FindFilesMethod("FIND_VARDATA", EraSaveFileType.Var);
			methodList["FIND_CHARADATA"] = new FindFilesMethod("FIND_CHARADATA", EraSaveFileType.CharVar);

            //定数取得
            methodList["MONEYSTR"] = new MoneyStrMethod();
            methodList["PRINTCPERLINE"] = new GetPrintCPerLineMethod();
			methodList["PRINTCLENGTH"] = new PrintCLengthMethod(); 
            methodList["SAVENOS"] = new GetSaveNosMethod();
            methodList["GETTIME"] = new GettimeMethod();
            methodList["GETTIMES"] = new GettimesMethod();
            methodList["GETMILLISECOND"] = new GetmsMethod();
            methodList["GETSECOND"] = new GetSecondMethod();

            //数学関数
			methodList["RAND"] = new RandMethod();
			methodList["MIN"] = new MaxMethod(false);
			methodList["MAX"] = new MaxMethod(true);
			methodList["ABS"] = new AbsMethod();
			methodList["POWER"] = new PowerMethod();
			methodList["SQRT"] = new SqrtMethod();
			methodList["CBRT"] = new CbrtMethod();
			methodList["LOG"] = new LogMethod();
			methodList["LOG10"] = new LogMethod(10.0d);
			methodList["EXPONENT"] = new ExpMethod();
			methodList["SIGN"] = new SignMethod();
            methodList["LIMIT"] = new GetLimitMethod();

            //変数操作系
			methodList["SUMARRAY"] = new SumArrayMethod();
			methodList["SUMCARRAY"] = new SumArrayMethod(true);
			methodList["MATCH"] = new MatchMethod();
			methodList["CMATCH"] = new MatchMethod(true);
			methodList["GROUPMATCH"] = new GroupMatchMethod();
			methodList["NOSAMES"] = new NosamesMethod();
			methodList["ALLSAMES"] = new AllsamesMethod();
			methodList["MAXARRAY"] = new MaxArrayMethod();
			methodList["MAXCARRAY"] = new MaxArrayMethod(true);
			methodList["MINARRAY"] = new MaxArrayMethod(false, false);
			methodList["MINCARRAY"] = new MaxArrayMethod(true, false);
			methodList["GETBIT"] = new GetbitMethod();
			methodList["GETNUM"] = new GetnumMethod();
			methodList["GETPALAMLV"] = new GetPalamLVMethod();
			methodList["GETEXPLV"] = new GetExpLVMethod();
            methodList["FINDELEMENT"] = new FindElementMethod(false);
            methodList["FINDLASTELEMENT"] = new FindElementMethod(true);
            methodList["INRANGE"] = new InRangeMethod();
            methodList["INRANGEARRAY"] = new InRangeArrayMethod();
			methodList["INRANGECARRAY"] = new InRangeArrayMethod(true);
			methodList["GETNUMB"] = new GetnumMethod();


            //文字列操作系
            methodList["STRLENS"] = new StrlenMethod();
            methodList["STRLENSU"] = new StrlenuMethod();
            methodList["SUBSTRING"] = new SubstringMethod();
            methodList["SUBSTRINGU"] = new SubstringuMethod();
            methodList["STRFIND"] = new StrfindMethod(false);
            methodList["STRFINDU"] = new StrfindMethod(true);
            methodList["STRCOUNT"] = new StrCountMethod();
			methodList["TOSTR"] = new ToStrMethod();
			methodList["TOINT"] = new ToIntMethod();
			methodList["TOUPPER"] = new StrChangeStyleMethod(StrFormType.Upper);
			methodList["TOLOWER"] = new StrChangeStyleMethod(StrFormType.Lower);
			methodList["TOHALF"] = new StrChangeStyleMethod(StrFormType.Half);
			methodList["TOFULL"] = new StrChangeStyleMethod(StrFormType.Full);
			methodList["LINEISEMPTY"] = new LineIsEmptyMethod();
			methodList["REPLACE"] = new ReplaceMethod();
            methodList["UNICODE"] = new UnicodeMethod();
            methodList["UNICODEBYTE"] = new UnicodeByteMethod();
			methodList["CONVERT"] = new ConvertIntMethod();
            methodList["ISNUMERIC"] = new IsNumericMethod();
            methodList["ESCAPE"] = new EscapeMethod();
            methodList["ENCODETOUNI"] = new EncodeToUniMethod();
            methodList["CHARATU"] = new CharAtMethod();
			methodList["GETLINESTR"] = new GetLineStrMethod();
			methodList["STRFORM"] = new StrFormMethod();
			
			methodList["GETCONFIG"] = new GetConfigMethod(true);
			methodList["GETCONFIGS"] = new GetConfigMethod(false);

			//html系
			methodList["HTML_GETPRINTEDSTR"] = new HtmlGetPrintedStrMethod();
			methodList["HTML_POPPRINTINGSTR"] = new HtmlPopPrintingStrMethod();
			methodList["HTML_TOPLAINTEXT"] = new HtmlToPlainTextMethod();
			methodList["HTML_ESCAPE"] = new HtmlEscapeMethod();

            // JVN: TR Commands here
            methodList["TR_NAME"] = new TRNameMethod();
            //methodList["TR_CSTR"] = new TRCSTRMethod();
        }

		private static Dictionary<string, FunctionMethod> methodList;
		public static Dictionary<string, FunctionMethod> GetMethodList()
		{
			return methodList;
		}
	}
}