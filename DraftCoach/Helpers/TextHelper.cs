using System;
using System.Linq;

namespace DraftCoach.Helpers
{
    public static class TextHelper
    {
        public const int LineLength = 120;

        #region Public methods - Input

        public static string RemoveSpecialCharactersAndLowerCase(string text)
        {
            return new string(text.Where(Char.IsLetter).ToArray()).ToLower();
        }

        #endregion

        #region Public methods - Output formatting

        public static void InitializeConsoleSettings()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WindowWidth = LineLength;
        }

        public static string PadTextCentre(string text)
        {
            int paddingLength = LineLength - text.Length;
            int padLeft = paddingLength / 2 + text.Length;
            string paddedText = text.PadLeft(padLeft).PadRight(LineLength);

            return paddedText;
        }
        public static string PadTextRightHalf(string text)
        {

            return text.PadRight(LineLength / 2);
        }

        public static string PadTextLeftHalf(string text)
        {

            return text.PadLeft(LineLength / 2);
        }

        public static void PrintHorizontalBorderLine()
        {
            Console.WriteLine(new string('-', LineLength));
        }

        #endregion

        #region Public methods - Output colouring

        public static void PrintInformationLine(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintErrorLine(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintBlue(string text)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintRed(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintCyan(string text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        #endregion
    }
}
