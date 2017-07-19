using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.GameData.Variable;
using MinorShift.Emuera.Sub;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinorShift.Emuera.GameData.Function
{
    internal static partial class FunctionMethodCreator
    {
        // str TR_NAME(str,str)
        // Input 1: string with csv key or 
        // Input 2: string with csv value or int identifier
        // Output: String with TR-translated if it's successful found, or the original if it fails
        public sealed class TRNameMethod : FunctionMethod
        {

            public TRNameMethod()
            {
                ReturnType = typeof(string);
                argumentTypeArray = null;
                CanRestructure = true;
            }

            public override string CheckArgumentType(string name, IOperandTerm[] arguments)
            {
                if (arguments.Length < 2)
                    return name + " needs 2 arguments";
                if (arguments.Length > 2)
                    return name + " has too many arguments";
                if (arguments[0] == null)
                    return "1st argument of " + name + " cannot be ommited";
                if (!(arguments[0].GetOperandType() == typeof(string) || arguments[0] is VariableTerm))
                    return "1st argument of " + name + " must be a string or term";
                if (arguments[1] == null)
                    return "2nd argument of " + name + " cannot be ommited";
                if (!(arguments[1].GetOperandType() == typeof(Int64) || arguments[1].GetOperandType() == typeof(string)))
                    return "2nd argument of " + name + " must be a string or int";
                return null;
            }

            public override string GetStrValue(ExpressionMediator exm, IOperandTerm[] arguments, bool tryTranslate = false)
            {
                string sdic = "";
                VariableCode varCode;
                if (arguments[0].GetOperandType() == typeof(string)) { // JVN: If first argument is a string...
                    sdic = arguments[0].GetStrValue(exm);
                    switch (sdic)
                    { // JVN: Prepares the first argument
                        case "ABL":
                        case "ABLNAME":
                            sdic = "Abl";
                            varCode = VariableCode.ABL;
                            break;
                        case "BASE":
                        case "BASENAME":
                            sdic = "Base";
                            varCode = VariableCode.BASE;
                            break;
                        case "CDFLAG1":
                        case "CDFLAGNAME1":
                            sdic = "Cdflag1";
                            varCode = VariableCode.CDFLAGNAME1;
                            break;
                        case "CDFLAG2":
                        case "CDFLAGNAME2":
                            sdic = "Cdflag2";
                            varCode = VariableCode.CDFLAGNAME2;
                            break;
                        case "CFLAG":
                        case "CFLAGNAME":
                            sdic = "Cflag";
                            varCode = VariableCode.CFLAG;
                            break;
                        case "CSTR":
                        case "CSTRNAME":
                            sdic = "CSTR";
                            varCode = VariableCode.CSTR;
                            break;
                        case "EQUIP":
                        case "EQUIPNAME":
                            sdic = "EQUIP";
                            varCode = VariableCode.EQUIP;
                            break;
                        case "EX":
                        case "EXNAME":
                            sdic = "Ex";
                            varCode = VariableCode.EX;
                            break;
                        case "EXP":
                        case "EXPNAME":
                            sdic = "Exp";
                            varCode = VariableCode.EXP;
                            break;
                        case "FLAG":
                        case "FLAGNAME":
                            sdic = "Flag";
                            varCode = VariableCode.FLAG;
                            break;
                        case "GLOBAL":
                        case "GLOBALNAME":
                            sdic = "GLOBAL";
                            varCode = VariableCode.GLOBAL;
                            break;
                        case "GLOBALS":
                        case "GLOBALSNAME":
                            sdic = "GLOBALS";
                            varCode = VariableCode.GLOBALS;
                            break;
                        case "ITEM":
                        case "ITEMNAME":
                            sdic = "Item";
                            varCode = VariableCode.ITEM;
                            break;
                        case "MARK":
                        case "MARKNAME":
                            sdic = "Mark";
                            varCode = VariableCode.MARK;
                            break;
                        case "PALAM":
                        case "PALAMNAME":
                        case "PARAM":
                        case "PARAMNAME":
                            sdic = (Translation.isCanLoadParam() ? "Param" : "Palam");
                            varCode = VariableCode.PALAM;
                            break;
                        case "SAVESTR":
                        case "SAVESTRNAME":
                            sdic = "SaveStr";
                            varCode = VariableCode.SAVESTR;
                            break;
                        case "SOURCE":
                        case "SOURCENAME":
                            sdic = "Source";
                            varCode = VariableCode.SOURCE;
                            break;
                        case "STAIN":
                        case "STAINNAME":
                            sdic = "Stain";
                            varCode = VariableCode.STAIN;
                            break;
                        case "STR":
                        case "STRNAME":
                            sdic = "Str";
                            varCode = VariableCode.STR;
                            break;
                        case "TALENT":
                        case "TALENTNAME":
                            sdic = "Talent";
                            varCode = VariableCode.TALENT;
                            break;
                        case "TCVAR":
                        case "TCVARNAME":
                            sdic = "Tcvar";
                            varCode = VariableCode.TCVAR;
                            break;
                        case "TEQUIP":
                        case "TEQUIPNAME":
                            sdic = "TEQUIP";
                            varCode = VariableCode.TEQUIP;
                            break;
                        case "TFLAG":
                        case "TFLAGNAME":
                            sdic = "TFLAG";
                            varCode = VariableCode.TFLAG;
                            break;
                        case "TRAIN":
                        case "TRAINNAME":
                            sdic = "Train";
                            varCode = VariableCode.TRAINNAME;
                            break;
                        case "TSTR":
                        case "TSTRNAME":
                            sdic = "TSTR";
                            varCode = VariableCode.TSTR;
                            break;
                        default:
                            throw new CodeEE("Invalid first argument: " + arguments[0].GetStrValue(exm));
                    }
                }
                else { //JVN: first argument must be a variable term
                    VariableTerm vToken = (VariableTerm)arguments[0];
                    varCode = vToken.Identifier.Code;
                    switch (varCode)
                    {
                        case VariableCode.ABL:
                            sdic = "Abl";
                            break;
                        case VariableCode.BASE:
                            sdic = "Base";
                            break;
                        case VariableCode.CDFLAGNAME1:
                            sdic = "Cdflag1";
                            break;
                        case VariableCode.CDFLAGNAME2:
                            sdic = "Cdflag2";
                            break;
                        case VariableCode.CFLAG:
                            sdic = "Cflag";
                            break;
                        case VariableCode.CSTR:
                            sdic = "CSTR";
                            break;
                        case VariableCode.EQUIP:
                            sdic = "EQUIP";
                            break;
                        case VariableCode.EX:
                            sdic = "Ex";
                            break;
                        case VariableCode.EXP:
                            sdic = "Exp";
                            break;
                        case VariableCode.FLAG:
                            sdic = "Flag";
                            break;
                        case VariableCode.GLOBAL:
                            sdic = "GLOBAL";
                            break;
                        case VariableCode.GLOBALS:
                            sdic = "GLOBALS";
                            break;
                        case VariableCode.ITEM:
                            sdic = "Item";
                            break;
                        case VariableCode.MARK:
                            sdic = "Mark";
                            break;
                        case VariableCode.PALAM:
                            sdic = (Translation.isCanLoadParam() ? "Param" : "Palam");
                            break;
                        case VariableCode.SAVESTR:
                            sdic = "SaveStr";
                            break;
                        case VariableCode.SOURCE:
                            sdic = "Source";
                            break;
                        case VariableCode.STAIN:
                            sdic = "Stain";
                            break;
                        case VariableCode.STR:
                            sdic = "Str";
                            break;
                        case VariableCode.TALENT:
                            sdic = "Talent";
                            break;
                        case VariableCode.TCVAR:
                            sdic = "Tcvar";
                            break;
                        case VariableCode.TEQUIP:
                            sdic = "TEQUIP";
                            break;
                        case VariableCode.TFLAG:
                            sdic = "TFLAG";
                            break;
                        case VariableCode.TRAINNAME:
                            sdic = "Train";
                            break;
                        case VariableCode.TSTR:
                            sdic = "TSTR";
                            break;
                        default:
                            throw new CodeEE("Invalid first argument: " + vToken.ToString());
                    }
                }
                if (arguments[1].GetOperandType() == typeof(string)) { // JVN: Deal with the string
                    string key = arguments[1].GetStrValue(exm);
                    if (exm.VEvaluator.Constant.isDefined(varCode, key)) return Translation.translate(key, sdic, true);
                    else throw new CodeEE("Invalid second argument: " + arguments[1].GetStrValue(exm).ToString());
                }
                else { // JVN: Deal with the integer
                    string errPos;
                    Dictionary<string, int> dic = exm.VEvaluator.Constant.GetKeywordDictionary(out errPos, varCode, -1);
                    if (dic != null) {
                        foreach (var entry in dic) 
                            if (entry.Value == arguments[1].GetIntValue(exm)) return Translation.translate(entry.Key,sdic,true);

                        throw new CodeEE("Invalid second argument: " + arguments[1].GetIntValue(exm).ToString());
                    }
                    else throw new CodeEE("Invalid second argument: " + arguments[1].GetIntValue(exm).ToString());
                }

                // JVN: Should never get here
                throw new CodeEE("Unknown Error");
            }
        }

        /*public sealed class TRCSTRMethod : FunctionMethod
        {
            //return exm.VEvaluator.GetCharacterStrfromCSVData(x, varCode, false, 0);
        }*/
    }
}
