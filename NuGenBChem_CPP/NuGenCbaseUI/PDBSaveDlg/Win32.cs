
using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NGVChem;
namespace NGVChem
{
    internal static class NativeMethods
    {
        #region Delegates
        internal delegate bool EnumWindowsCallBack(IntPtr hWnd, int lParam);
        #endregion

        #region USER32

        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hwnd);
        [DllImport("user32.dll")]
        internal static extern IntPtr CreateWindowEx(
           uint dwExStyle,string lpClassName,string lpWindowName, uint dwStyle,int x,int y,        
           int nWidth,int nHeight,IntPtr hWndParent,IntPtr hMenu,IntPtr hInstance,IntPtr lpParam);
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll")]
        internal static extern bool EnableWindow(IntPtr hWnd, bool bEnable);
        [DllImport("user32.dll")]
        internal static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);
        [DllImport("user32.dll")]
        internal static extern bool SetDlgItemText(IntPtr hDlg, int nIDDlgItem, string lpString);
		[DllImport("User32.Dll")]
        public static extern int GetDlgCtrlID(IntPtr hWndCtl);
        [DllImport("user32.dll", SetLastError = true)][return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowInfo(IntPtr hwnd, out WINDOWINFO pwi);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowText(IntPtr hWnd, string lpString);
		[DllImport("User32.Dll")]
		public static extern void GetClassName(IntPtr hWnd, StringBuilder param, int length);
        [DllImport("user32.Dll")][return: MarshalAs(UnmanagedType.Bool)]        
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsCallBack lpEnumFunc, int lParam); 
        [DllImport("user32.dll")]
        internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        internal static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        internal static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, StringBuilder lParam);
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int Width, int Height, SetWindowPosFlags flags);
        [DllImport("user32.dll")][return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(IntPtr hwnd, ref RECT rect);
        [DllImport("user32.dll")][return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetClientRect(IntPtr hwnd, ref RECT rect);
        [DllImport("user32.dll")][return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr SetForegeoundWindow(IntPtr hWnd);
        #endregion
    }
}
