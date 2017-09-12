using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MinorShift.Emuera.Sub;
using System.Text.RegularExpressions;
using MinorShift.Emuera.GameView;

// Created by Bartoum.
// Class that is initialized one time in Constant data.
// Create a dictionary of dictionary for each *_TR.csv that is there
namespace MinorShift.Emuera.GameData.Variable
{
    internal class Translation // JVN: making it internal allows to pass the Emueraconsole by reference here
    {

        public static Dictionary<string, Dictionary<string, string>> nameTlDictionnary;
        public static string[] fileArray;
		private static ArrayList memoryArray;
		private static string lastRet;
        private static int nbrMemory = 20;
        private static bool canLoadParam;
        private static Dictionary<Int64, TRCharaTemplate> TRCharaList; // JVN: A simple dictionary, only needs the character number as a key.
        private static EmueraConsole output; // JVN: Console access is a must, debug is life, debug is <3

        public enum CharaData : int { Name = 0, Callname, Nickname, Mastername, CSTR}; // JVN: enums are noice.

        // JVN: Templates are good too. Make it private too, because it's only used interally
        private class TRCharaTemplate
        {
            public Int64 No;
            public string Name;
            public string Callname;
            public string Nickname;
            public string Mastername;
            public Dictionary<Int32, string> CStr;

            // JVN: Construct
            public TRCharaTemplate(Int64 aNo = 0, string aName ="", string aCall ="", string aNick = "", string aMaster = "")
            {
                this.No = aNo;
                this.Name = aName;
                this.Callname = aCall;
                this.Nickname = aNick;
                this.Mastername = aMaster;
                this.CStr = new Dictionary<Int32, string>();
            }

            // JVN: Cloning is important for what we need to do
            public TRCharaTemplate Clone()
            {
                TRCharaTemplate tmp = new TRCharaTemplate(this.No, this.Name, this.Callname, this.Nickname, this.Mastername);
                foreach (KeyValuePair<Int32, string> cloneData in this.CStr)
                {
                    tmp.CStr.Add(cloneData.Key, cloneData.Value);
                }
                return tmp;
            }
        }

        //Constructor
        public Translation(string csvDir, ref EmueraConsole console, bool pcanLoadParam)
		{

            output = console; // JVN: Debugging is important, and the console allows for that
			canLoadParam = pcanLoadParam;
			this.loadTranslationData(csvDir);
            this.loadCharacterTranslationData(csvDir);
        }

        //JVN: I just wants it
        public static bool isCanLoadParam() { return canLoadParam; }

        private const bool translateCSVAnyways = true; // TODO: make this configurable, somehow, probably via CSV setting?

		//Normal way to translate original is the string of the variable we are working with and name is the file where it lives ex: Talent or Source. It can also be ALL.
		public static string translate(string original, string name, bool tryTranslate){
			string ret;
            ret = original;
            if (name == "Palam" && canLoadParam)
                name = "Param";

			
			
            //Change the original term to the translated one;
			if (original != null){		
				if(Translation.nameTlDictionnary.ContainsKey(name) && Translation.nameTlDictionnary[name].ContainsKey(original)){
					if(tryTranslate || translateCSVAnyways)
                    {
						ret = Translation.nameTlDictionnary[name][original];
                        lastRet = ret;
                    }
					else{
                        if (memoryArray.IndexOf(original + "," + name) == -1)
                            memoryArray.Add(original + "," + name);

                        if (memoryArray.Count >= nbrMemory)
                            memoryArray.RemoveAt(0);
					}							
				}
			}	
            return ret;
		}

        // JVN: Character string translation
        public static string translateChara(Int64 character, CharaData varname, string oriString, Int32 index = 0) {
            switch (varname)
            {
                case CharaData.Name:        // "NAME","名前"
                    if (TRCharaList.ContainsKey(character)) return TRCharaList[character].Name;
                    break;
                case CharaData.Callname:    // "CALLNAME","呼び名"
                    if (TRCharaList.ContainsKey(character)) return TRCharaList[character].Callname;
                    break;
                case CharaData.Nickname:    // "NICKNAME","あだ名"
                    if (TRCharaList.ContainsKey(character)) return TRCharaList[character].Nickname;
                    break;
                case CharaData.Mastername:  // "MASTERNAME","主人の呼び方"
                    if (TRCharaList.ContainsKey(character)) return TRCharaList[character].Mastername;
                    break;
                case CharaData.CSTR:        // "CSTR"
                    if (TRCharaList.ContainsKey(character))
                        if (TRCharaList[character].CStr.ContainsKey(index)) return TRCharaList[character].CStr[index];
                    break;
            }
            return oriString;
        }

        //The last way to translate something.
        public static string translateALL(string original, string pname ="ALL")
        {
            string ret = original;

            if (!String.IsNullOrEmpty(original))
            {
                bool found = false;
                string[] temp;

                int i = memoryArray.Count - 1;
                if (!String.IsNullOrEmpty(lastRet) && original.Contains(lastRet))
                {
                    //output.Print("Did it brah\n");
                    found = true;
                }

                while (i >= 0 && !found)
                {
                    temp = memoryArray[i].ToString().Split(',');
                   // output.Print("Checked: " + original + " against: " + temp[0] + " \\ ");
                    if (original.Contains(temp[0]))
                    {
                        //output.Print("FOUND IT with:" +temp[1]+ " " + temp[0] + " \n");
                        ret = original.Replace(temp[0], Translation.nameTlDictionnary[temp[1]][temp[0]]);
                        found = true;
                        //memoryArray.Remove(temp[0] + "," + temp[1]);         
                    }
                    i--;
                }
                //output.Print("\n");


                if (!found)
                {
                    if (Translation.nameTlDictionnary.ContainsKey(pname))
                    {
                        //We try to strip all the stuff around the csv variable name
                        //Remove all numbers, remove LV and all special characters bellow
                        //Parantheses are only remove if only numbers where inside them because some variable name use them

                        bool numPresent = true;


                        //We remove all the numbers
                        while (numPresent)
                        {
                            if (Regex.Match(original, @"\d+").Value != "")
                            {
                                original = original.Replace(Regex.Match(original, @"\d+").Value, "");
                            }
                            else
                            {
                                numPresent = false;
                            }
                        }
                        //original = original.Replace("-", "");
                        //original = original.Replace("()", "");
                        original = original.Replace("【", "");
                        original = original.Replace("】", "");
                        original = original.Replace("[", "");
                        original = original.Replace("]", "");
                        original = original.Replace("LV", "");
                        original = original.Replace("『", "");
                        original = original.Replace("』", "");
                        /*
                        original = original.Replace("1Lv", "");
                        original = original.Replace("2Lv", "");
                        original = original.Replace("3Lv", "");
                        original = original.Replace("4Lv", "");
                        original = original.Replace("5Lv", "");		
                        */
                        //output.Print("Did it the old stupid way\n");
                        if (Translation.nameTlDictionnary[pname].ContainsKey(original))
                        {
                            ret = ret.Replace(original, Translation.nameTlDictionnary[pname][original]);
                            //output.Print("Found it the stupid way");
                        }
                    }
                }
            }
            return ret;
        }

        private void loadTranslationData(string csvDir)
		{
			fileArray = new string[] {"Abl", "Talent", "Exp", "Mark", "Palam", "Param", "Item","Train", "Base","Source", "Ex", "EQUIP", "TEQUIP", "Flag", "TFLAG", "Cflag", "Tcvar", "CSTR", "Stain","Cdflag1", "Cdflag2", "Str", "TSTR","SaveStr", "GLOBAL", "GLOBALS","ALL"};
			nameTlDictionnary = new Dictionary<string, Dictionary<string, string> >();
			memoryArray = new ArrayList();
            string temp;

            for (int i = 0; i < fileArray.Length;i++){
				Dictionary<string,string> tempDictionnary = new Dictionary<string,string>();
				
				string csvPath = csvDir + fileArray[i] + "_TR.csv";
				if (!File.Exists(csvPath))
					continue;
				
				EraStreamReader eReader = new EraStreamReader(false);
				
				if (!eReader.Open(csvPath))
					throw new Exception(eReader.Filename + " can't be opened");

				ScriptPosition position = null;
				try
				{
					StringStream st = null;
					while ((st = eReader.ReadEnabledLine()) != null)
					{

						position = new ScriptPosition(eReader.Filename, eReader.LineNo, st.RowString);
                        temp = st.Substring();
                        if (temp[0] == ';')
                            continue;
                        else if (temp.Contains(";"))
                            temp = temp.Substring(0, temp.IndexOf(';'));

                        string[] tokens = temp.Split(',');

                        //Make it crash with a good message if there's not a ","
                        if(tokens.Length != 2)
                            throw new Exception("A line must have one \",\" to separate the original from the new string. The line with the problem begins by : " + tokens[0]);

                        //Make it crash with a good message if the same key is present two times
                        if (tempDictionnary.ContainsKey(tokens[0]))
							throw new Exception("You can only have one occurence of the same key. " + tokens[0] + " is already linked to : " + tempDictionnary[tokens[0]]);
						
						tempDictionnary.Add(tokens[0], tokens[1]);		
					}
					nameTlDictionnary.Add(fileArray[i], tempDictionnary);
				}
				catch(Exception e)
				{
                    throw new Exception(e.Message);
				}
				finally
				{
					eReader.Close();
                }
            }
        }

        //Receive the line that is being interpreted as a string.
        //if there's a TALENTXXX  in this string it will return the XXX
        //if not it will return ALL.
        public static string searchParentFile(string name)
        {
            string ret = "ALL";
            int i = 0;
            bool found = false;

            try
            {
				 if (name.Contains("NAME")){
					 while (i < Translation.fileArray.Length - 1 && !found)
					 {
						 if (name.Contains(Translation.fileArray[i].ToUpper() + "NAME"))
						 {
							 ret = Translation.fileArray[i];
							 found = true;
							 }
							 i++;
					 }
			}
				


            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }

            return ret;
        }

        // JVN: Loading fuctions for TRCHARA*.CSV
        private void loadCharacterTranslationData(string csvDir, bool useCompatiName = false, bool disp = false)
        {
            if (!Directory.Exists(csvDir))
                return;
            TRCharaList = new Dictionary<long, TRCharaTemplate>();
            // JVN: Search the CSV and subdirectories for TRCHARA files and send that to a function for futher loading
                List<KeyValuePair<string, string>> csvPaths = Config.GetFiles(csvDir, "TRCHARA*.CSV");
            for (int i = 0; i < csvPaths.Count; i++)
                loadTRCharacterDataFile(csvPaths[i].Value, csvPaths[i].Key, disp);

            if (useCompatiName) // check for the compatibility flag, just in case
            {
                foreach (KeyValuePair<Int64, TRCharaTemplate> tmpl in TRCharaList)
                    if (string.IsNullOrEmpty(tmpl.Value.Callname))
                        tmpl.Value.Callname = tmpl.Value.Name;
            }
        }

        // JVN: Loads individual character files and data
        private void loadTRCharacterDataFile(string csvPath, string csvName, bool disp)
        {
            bool NextCharacter = false;
            EraStreamReader eReader = new EraStreamReader(false);
            if (!eReader.Open(csvPath, csvName))
            {
                output.PrintError("File " + eReader.Filename + " has failed to open");
                return;
            }
            ScriptPosition position = null;
            if (disp)
                output.PrintSystemLine(eReader.Filename + " is being read・・・");
            try
            {
                TRCharaTemplate tmpl = null;
                Int64 index = -1;
                StringStream st = null;
                bool Alive = true;
                while (Alive || NextCharacter)
                {
                    Alive = (st = eReader.ReadEnabledLine()) != null;
                    if (NextCharacter)
                    {
                        if (tmpl != null) { 
                        TRCharaTemplate tmp = tmpl.Clone();
                        TRCharaList.Add(index, tmp);
                        tmpl = null;
                        NextCharacter = false;
                        }
                        if (!Alive) break;
                    }
                    position = new ScriptPosition(eReader.Filename, eReader.LineNo, st.RowString);
                    string[] tokens = st.Substring().Split(',');
                    if (tokens.Length < 2)
                    {
                        ParserMediator.Warn("\",\" is needed", position, 1);
                        continue;
                    }
                    if (tokens[0].Length == 0)
                    {
                        ParserMediator.Warn("Begins in \",\"", position, 1);
                        continue;
                    }
                    if ((tokens[0].Equals("NO", Config.SCVariable)) || (tokens[0].Equals("番号", Config.SCVariable)))
                    {
                        if (tmpl != null)
                        {
                            ParserMediator.Warn("Character number has been defined twice", position, 1);
                            continue;
                        }
                        if (!Int64.TryParse(tokens[1].TrimEnd(), out index))
                        {
                            ParserMediator.Warn(tokens[1] + " cannot be changed into an integer", position, 1);
                            continue;
                        }
                        tmpl = new TRCharaTemplate(index);
                        continue;
                    }
                    if (tmpl == null)
                    {
                        ParserMediator.Warn("Data started before the character number is defined", position, 1);
                        continue;
                    }
                    NextCharacter = toTRCharaTemplate(position, tmpl, tokens);
                }

            }
            catch (Exception e)
            {
                System.Media.SystemSounds.Hand.Play();
                if (position != null)
                {
                    output.PrintError(e.ToString());
                    ParserMediator.Warn("An unexpected error has occurred", position, 3);
                }
                else
                {
                    output.PrintError("An unexpected error has occurred");
                }
                output.PrintError("");
                return;
            }
            finally
            {
                eReader.Dispose();
            }
        }

        // JVN: Copy of the ConstantData's tryTo64
        private bool tryToInt64(string str, out Int64 p)
        {
            p = -1;
            if (string.IsNullOrEmpty(str))
                return false;
            StringStream st = new StringStream(str);
            int sign = 1;
            if (st.Current == '+')
                st.ShiftNext();
            else if (st.Current == '-')
            {
                sign = -1;
                st.ShiftNext();
            }
            //1803beta005 char.IsDigitは全角数字とかまでひろってしまうので･･･
            //if (!char.IsDigit(st.Current))
            // return false;
            switch (st.Current)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    break;
                default:
                    return false;
            }
            try
            {
                p = LexicalAnalyzer.ReadInt64(st, false);
                p = p * sign;
            }
            catch
            {
                return false;
            }
            return true;
        }


        // JVN: Takes care of each line being read and sends result back. That works too. Returns false if 'ENDCHR' is found.
        private bool toTRCharaTemplate(ScriptPosition position, TRCharaTemplate chara, string[] tokens)
        {
            if (chara == null)
                return true;
            int length = -1;
            Int64 p1 = -1;
            //Int64 p2 = -1;
            //Dictionary<int, Int64> intArray = null;
            Dictionary<int, string> strArray = null;
            Dictionary<string, int> namearray = null;

            string errPos = null;
            string varname = tokens[0].ToUpper();
            switch (varname)
            {
                case "NAME":
                case "名前":
                    chara.Name = tokens[1];
                    return false;
                case "CALLNAME":
                case "呼び名":
                    chara.Callname = tokens[1];
                    return false;
                case "NICKNAME":
                case "あだ名":
                    chara.Nickname = tokens[1];
                    return false;
                case "MASTERNAME":
                case "主人の呼び方":
                    chara.Mastername = tokens[1];
                    return false;
                case "CSTR":
                    strArray = chara.CStr;
                    errPos = "cstr.csv";
                    break;
                case "ENDCHR":
                case "ENDCHAR":
                    return true;
                default:
                    ParserMediator.Warn("\"" + tokens[0] + "\" cannot be interpreted", position, 1);
                    return false;
            }
            bool p1isNumeric = tryToInt64(tokens[1].TrimEnd(), out p1);
            int index = (int)p1;
            if ((!p1isNumeric) && (namearray != null))
            {
                if (!namearray.TryGetValue(tokens[1], out index))
                {
                    ParserMediator.Warn(errPos + ": \"" + tokens[1] + "\" is undefined", position, 1);
                    return false;
                }
                else if (index >= length)
                {
                    ParserMediator.Warn("\"" + tokens[1] + "\" is out of bounds of the array", position, 1);
                    return false;
                }
            }

            if (strArray != null)
            {
                if (tokens.Length < 3)
                    ParserMediator.Warn("There's no third identifier", position, 1);
                if (strArray.ContainsKey(index))
                    ParserMediator.Warn(varname + "'s #" + index.ToString() + " element is already denied (Overwritting it)", position, 1);
                strArray[index] = tokens[2];
            }
            return false;
        }

    }
}
