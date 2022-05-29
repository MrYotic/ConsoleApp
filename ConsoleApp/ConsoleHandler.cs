using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp;
using static ConsoleHandler.NativeMethods;

public static class ConsoleHandler
{
    public delegate void MouseHandler();
    public static event MouseHandler? MouseUpdate;
    public delegate void PressKeyHandler(bool down, char @char, ushort key, int state);
    public static event PressKeyHandler? PressKey;
    public static class Mouse
    {
        public static ushort X { get => record.MouseEvent.dwMousePosition.X; }
        public static ushort Y { get => record.MouseEvent.dwMousePosition.Y; }
        public static int State { get => record.MouseEvent.dwButtonState; }
        public static int KeyState { get => record.MouseEvent.dwControlKeyState; }
        public static int EventFlags { get => record.MouseEvent.dwEventFlags; }
    }
    public static class Keyboard
    {
        public static bool KeyDown { get => record.KeyEvent.bKeyDown; }
        public static ushort RepeatCount { get => record.KeyEvent.wRepeatCount; }
        public static ushort KeyCode { get => record.KeyEvent.wVirtualKeyCode; }
        public static char Char { get => record.KeyEvent.UnicodeChar; }
        public static int KeyState { get => record.KeyEvent.dwControlKeyState; }
    }
    public static INPUT_RECORD record;
    static ConsoleHandler()
    {
        var handle = GetStdHandle(STD_INPUT_HANDLE);
        int mode = 0;
        if (!GetConsoleMode(handle, ref mode)) throw new Win32Exception();
        mode |= ENABLE_MOUSE_INPUT;
        mode &= ~ENABLE_QUICK_EDIT_MODE;
        mode |= ENABLE_EXTENDED_FLAGS;
        if (!SetConsoleMode(handle, mode)) throw new Win32Exception();
        record = new INPUT_RECORD();
        uint recordLen = 0;
        new Thread(() =>
        {
            while (true)
            {
                if (!ReadConsoleInput(handle, ref record, 1, ref recordLen)) { throw new Win32Exception(); }
                if(record.EventType == 1)
                {
                    PressKey?.Invoke(Keyboard.KeyDown, Keyboard.Char, Keyboard.KeyCode, Keyboard.KeyState);
                }
                else if(record.EventType == 2)
                    MouseUpdate?.Invoke();
                Thread.Sleep(25);
            }
        }).Start();
    }
    public class NativeMethods
    {
        public const int STD_INPUT_HANDLE = -10;
        public const int ENABLE_MOUSE_INPUT = 0x0010;
        public const int ENABLE_QUICK_EDIT_MODE = 0x0040;
        public const int ENABLE_EXTENDED_FLAGS = 0x0080;
        public const int KEY_EVENT = 1;
        public const int MOUSE_EVENT = 2;

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT_RECORD
        {
            [FieldOffset(0)]
            public short EventType;
            [FieldOffset(4)]
            public KEY_EVENT_RECORD KeyEvent;
            [FieldOffset(4)]
            public MOUSE_EVENT_RECORD MouseEvent;
        }

        public struct MOUSE_EVENT_RECORD
        {
            public COORD dwMousePosition;
            public int dwButtonState;
            public int dwControlKeyState;
            public int dwEventFlags;
        }

        public struct COORD
        {
            public ushort X;
            public ushort Y;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct KEY_EVENT_RECORD
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.Bool)]
            public bool bKeyDown;
            [FieldOffset(4)]
            public ushort wRepeatCount;
            [FieldOffset(6)]
            public ushort wVirtualKeyCode;
            [FieldOffset(8)]
            public ushort wVirtualScanCode;
            [FieldOffset(10)]
            public char UnicodeChar;
            [FieldOffset(10)]
            public byte AsciiChar;
            [FieldOffset(12)]
            public int dwControlKeyState;
        };

        public class ConsoleHandle : SafeHandleMinusOneIsInvalid
        {
            public ConsoleHandle() : base(false) { }
            protected override bool ReleaseHandle() => true;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetConsoleMode(ConsoleHandle hConsoleHandle, ref int lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern ConsoleHandle GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReadConsoleInput(ConsoleHandle hConsoleInput, ref INPUT_RECORD lpBuffer, uint nLength, ref uint lpNumberOfEventsRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetConsoleMode(ConsoleHandle hConsoleHandle, int dwMode);

    }
}
