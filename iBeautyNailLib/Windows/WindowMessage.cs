using System;

namespace iBeautyNail.Windows
{
    public class WindowMessage
    {
        public const int WM_USER = 0x0400;

        public const int WM_DEVICEMANAGER = WM_USER + 100;
        public const int WM_EPSONPRINTER = WM_USER + 1000;
        public const int WM_REXODPRINTER = WM_USER + 2000;

        public const int PCU_MSG_STATE = 0x4321;
        public const int PCU_MSG_STATE_MOTOR = 0x01;
        public const int PCU_MSG_STATE_PRINT = 0x02;
        public const int PCU_MSG_STATE_PRINTER = 0x03;
        public const int PCU_MSG_STATE_EMPTY_PAPER = 0x04;

        public const int PCU_MSG_INK_VALUE = 0x4322;

        public static string GetFieldNameByValue(int windowMessage)
        {
            Type type = typeof(WindowMessage);
            foreach (var field in type.GetFields())
            {
                int value = (int)field.GetValue(null);
                if (windowMessage.Equals(value))
                {
                    return field.Name;
                }
            }
            return "";
        }
    }
}