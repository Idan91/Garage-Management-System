using System;
using System.Collections.Generic;
using System.Text;
using Ex03.GarageLogic;

namespace B19_Ex03
{
    public static class UIUtils
    {
        public static string PadBothSides(string i_SourceStr, int i_Length)
        {
            int spaces = i_Length - i_SourceStr.Length;
            int padLeft = spaces / 2 + i_SourceStr.Length;
            return i_SourceStr.PadLeft(padLeft).PadRight(i_Length);

        }

        public static string EnumDisplayName(object i_EnumValue)
        {
            string enumValueName = i_EnumValue.ToString();
            StringBuilder displayName = new StringBuilder();
            displayName.Append(enumValueName[0]);
            for (int i = 1; i < enumValueName.Length; i++)
            {
                if(Char.IsUpper(enumValueName[i]) || (Char.IsDigit(enumValueName[i]) && !Char.IsDigit(enumValueName[i-1])))
                {
                    displayName.Append(" ");
                    displayName.Append(enumValueName[i]);
                }
                else if(enumValueName[i] == '_')
                {
                    displayName.Append(" ");
                    displayName.Append("-");

                }
                else
                {
                    displayName.Append(enumValueName[i]);
                }
            }

            return displayName.ToString();
        }

        public static void PrintEnumSelection<T>(T i_Value)
        {
            int numOfEnumerations = Enum.GetNames(typeof(T)).Length;
            string[] enumArr = Enum.GetNames(typeof(T));
            int printedIndex = 0;

            for (int i = 0; i < numOfEnumerations; i++)
            {
                printedIndex++;
                if(i==0 && Convert.ToInt32(Enum.GetValues(typeof(T)).GetValue(0)) > 1)
                {
                    printedIndex--;
                    printedIndex += Convert.ToInt32(Enum.GetValues(typeof(T)).GetValue(0));
                }
                if (i != 0)
                {
                    Console.Write("|  ");
                }
                Console.Write(printedIndex + ". " + UIUtils.EnumDisplayName(enumArr[i]) + "  ");
            }
            Console.Write(Environment.NewLine + "  Select type number: ");
        }

    }

}
