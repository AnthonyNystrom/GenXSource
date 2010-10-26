/* -----------------------------------------------
 * EditorHelper.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Genetibase.WinApi;
using Genetibase.Windows.Controls.Editor;

namespace Genetibase.Windows.Controls
{
    internal static class EditorHelper
    {
        private static IntPtr _documentMgr;
        private static ITfThreadMgr _threadMgr;
        private static Boolean _threadMgrFailed;

        /// <summary>
        /// </summary>
        public static void AttachContext(HwndSource hwndSource)
        {
            if (hwndSource == null)
            {
                throw new ArgumentNullException("hwndSource");
            }
            IntPtr hIMC = Imm32.ImmCreateContext();

            if (hIMC != IntPtr.Zero)
            {
                Imm32.ImmAssociateContext(hwndSource.Handle, hIMC);
            }
        }

        /// <summary>
        /// </summary>
        public static void DetachContext(HwndSource hwndSource)
        {
            if (hwndSource == null)
            {
                throw new ArgumentNullException("hwndSource");
            }

            IntPtr immContext = Imm32.ImmAssociateContext(hwndSource.Handle, IntPtr.Zero);
            
            if (immContext != IntPtr.Zero)
            {
                Imm32.ImmDestroyContext(immContext);
            }
        }

        /// <summary>
        /// </summary>
        public static void DisableImmComposition(DispatcherObject dispatcher)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException("dispatcher");
            }

            if (!_threadMgrFailed)
            {
                if (_threadMgr == null)
                {
                    Msctr.TF_CreateThreadMgr(out _threadMgr);

                    if (_threadMgr == null)
                    {
                        _threadMgrFailed = true;
                        return;
                    }
                }

                dispatcher.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background
                    , new DispatcherOperationCallback(
                        delegate
                        {
                            IntPtr ptr;
                            _threadMgr.GetFocus(out ptr);

                            if (ptr == IntPtr.Zero)
                            {
                                _threadMgr.SetFocus(_documentMgr);
                            }

                            return null;
                        }
                    )
                    , null
                );
            }
        }

        /// <summary>
        /// </summary>
        public static void EnableImmComposition(DispatcherObject dispatcher)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException("dispatcher");
            }

            if (!_threadMgrFailed)
            {
                if (_threadMgr == null)
                {
                    Msctr.TF_CreateThreadMgr(out _threadMgr);

                    if (_threadMgr == null)
                    {
                        _threadMgrFailed = true;
                        return;
                    }
                }

                dispatcher.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background
                    , new DispatcherOperationCallback(
                        delegate
                        {
                            _threadMgr.GetFocus(out _documentMgr);
                            _threadMgr.SetFocus(IntPtr.Zero);
                            return null;
                        })
                    , null
                );
            }
        }

        /// <summary>
        /// </summary>
        public static void FlushImmCompositionString(IntPtr immContext)
        {
            if (immContext != IntPtr.Zero)
            {
                StringBuilder lpComp = new StringBuilder();
                Imm32.ImmSetCompositionStringW(immContext, 9, lpComp, 0, lpComp, 0);
            }
        }

        /// <summary>
        /// </summary>
        public static HwndSource GetHwndSource(Visual visual)
        {
            if (visual == null)
            {
                throw new ArgumentNullException("visual");
            }

            return (HwndSource)PresentationSource.FromVisual(GetRootVisual(visual));
        }

        /// <summary>
        /// </summary>
        public static String GetImmCompositionString(IntPtr immContext, Int32 dwIndex)
        {
            if (immContext == IntPtr.Zero)
            {
                return null;
            }

            Int32 dwBufLen = Imm32.ImmGetCompositionStringW(immContext, dwIndex, null, 0);
            
            if (dwBufLen < 0)
            {
                return null;
            }
            
            StringBuilder lpBuf = new StringBuilder(dwBufLen / 2);
            dwBufLen = Imm32.ImmGetCompositionStringW(immContext, dwIndex, lpBuf, dwBufLen);
            
            if (dwBufLen < 0)
            {
                return null;
            }
            
            return lpBuf.ToString().Substring(0, dwBufLen / 2);
        }

        /// <summary>
        /// </summary>
        public static IntPtr GetImmContext(Visual visual)
        {
            if (visual == null)
            {
                throw new ArgumentNullException("visual");
            }
            
            HwndSource hwndSource = GetHwndSource(visual);
            
            if (hwndSource != null)
            {
                return Imm32.ImmGetContext(hwndSource.Handle);
            }
            
            return IntPtr.Zero;
        }

        /// <summary>
        /// </summary>
        public static IntPtr GetKeyboardLayout()
        {
            return User32.GetKeyboardLayout(0);
        }

        /// <summary>
        /// </summary>
        public static Visual GetRootVisual(Visual visual)
        {
            if (visual == null)
            {
                throw new ArgumentNullException("visual");
            }

            DependencyObject reference = visual;
            Visual visual2 = visual;
            
            while ((reference = VisualTreeHelper.GetParent(reference)) != null)
            {
                Visual visual3 = reference as Visual;
                
                if (visual3 != null)
                {
                    visual2 = visual3;
                }
            }

            return visual2;
        }

        /// <summary>
        /// </summary>
        public static Point GetScreenCoordinates(Point point, Visual relativeTo)
        {
            if (relativeTo == null)
            {
                throw new ArgumentNullException("relativeTo");
            }
            
            Visual rootVisual = GetRootVisual(relativeTo);
            Point point2 = relativeTo.TransformToAncestor(rootVisual).Transform(point);
            HwndSource hwndSource = GetHwndSource(relativeTo);
            
            if (hwndSource != null)
            {
                POINT point3 = new POINT((Int32)point2.X, (Int32)point2.Y);
                User32.ClientToScreen(hwndSource.Handle, ref point3);
                return new Point((Double)point3.x, (Double)point3.y);
            }

            return point2;
        }

        /// <summary>
        /// </summary>
        public static Boolean HanjaConversion(Visual visual, IntPtr keyboardLayout, Char selection)
        {
            IntPtr immContext = GetImmContext(visual);

            if (immContext != IntPtr.Zero)
            {
                IntPtr lpBuf = Marshal.StringToHGlobalUni(new String(selection, 1));
                IntPtr ptr3 = Imm32.ImmEscapeW(keyboardLayout, immContext, 0x1008, lpBuf);
                Marshal.FreeHGlobal(lpBuf);
                ReleaseImmContext(visual, immContext);
                
                if (ptr3 != IntPtr.Zero)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// </summary>
        public static Boolean ImmNotifyIME(IntPtr immContext, Int32 dwAction, Int32 dwIndex, Int32 dwValue)
        {
            return Imm32.ImmNotifyIME(immContext, dwAction, dwIndex, dwValue);
        }

        /// <summary>
        /// </summary>
        public static Boolean ReleaseImmContext(Visual visual, IntPtr immContext)
        {
            if (visual == null)
            {
                throw new ArgumentNullException("visual");
            }

            if (immContext != IntPtr.Zero)
            {
                HwndSource hwndSource = GetHwndSource(visual);

                if (hwndSource != null)
                {
                    return Imm32.ImmReleaseContext(hwndSource.Handle, immContext);
                }
            }

            return false;
        }

        /// <summary>
        /// </summary>
        public static Boolean SetCompositionWindowPosition(IntPtr immContext, Point pt, Visual relativeTo)
        {
            if (immContext == IntPtr.Zero)
            {
                throw new ArgumentNullException("immContext");
            }
            
            if (relativeTo == null)
            {
                throw new ArgumentNullException("relativeTo");
            }
            
            Visual rootVisual = GetRootVisual(relativeTo);
            
            if (rootVisual == null)
            {
                return false;
            }
            
            Point point = relativeTo.TransformToAncestor(rootVisual).Transform(pt);
            HwndSource hwndSource = GetHwndSource(rootVisual);
            
            if (hwndSource != null)
            {
                point = hwndSource.CompositionTarget.TransformToDevice.Transform(point);
            }
            
            COMPOSITIONFORM structure = new COMPOSITIONFORM();
            structure.dwStyle = 2;
            structure.ptCurrentPos = new POINT((Int32)point.X, (Int32)point.Y);
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, ptr, true);
            Boolean flag = Imm32.ImmSetCompositionWindow(immContext, ptr);
            Marshal.FreeHGlobal(ptr);
            return flag;
        }

        /// <summary>
        /// </summary>
        public static Boolean SetImmFontHeight(IntPtr immContext, Int32 fontHeight)
        {
            if (immContext == IntPtr.Zero)
            {
                return false;
            }
            LOGFONT structure = new LOGFONT();
            structure.lfHeight = fontHeight;
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, ptr, true);
            Boolean flag = Imm32.ImmSetCompositionFontW(immContext, ptr);
            Marshal.FreeHGlobal(ptr);
            return flag;
        }
    }
}
