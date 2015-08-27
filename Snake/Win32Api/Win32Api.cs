// Win32API: оболочка для избранных функций Win32 API

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32API
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public POINT(int xx, int yy) { x = xx; y = yy; }
        public int x;
        public int y;
        public override string ToString()
        {
            var s = String.Format("({0},{1})", x, y);
            return s;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public SIZE(int cxx, int cyy) { cx = cxx; cy = cyy; }
        public int cx;
        public int cy;
        public override string ToString()
        {
            var s = String.Format("({0},{1})", cx, cy);
            return s;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
        public int Width() { return right-left; }
        public int Height() { return top - bottom; }
        public POINT TopLeft() { return new POINT(left, top); }
        public SIZE Size() { return new SIZE(Width(), Height()); }
        public override string ToString()
        {
            var s = String.Format("{0}x{1}", TopLeft(), Size());
            return s;
        }
    }



    public class Win32
    {


        const string DllPath = "user32.dll";
        // Опpеделяет, сделано ли окно видимым функцией ShowWindow.
        [DllImport(DllPath)]
        public static extern bool IsWindowVisible(int hwnd);

        // Копиpует в Str заголовок окна или текст оpгана упpавления.
        [DllImport(DllPath)]
        public static extern int GetWindowText(int hwnd,
        StringBuilder buf, int nMaxCount);

        // Считывает имя класса окна.
        [DllImport(DllPath)]
        public static extern int GetClassName(int hwnd,
        [MarshalAs(UnmanagedType.LPStr)] StringBuilder buf,
        int nMaxCount);

        // Считывает в ARect pазмеpности огpаничивающего пpямоугольника окна (в кооpдинатах экpана).
        [DllImport(DllPath)]
        public static extern int GetWindowRect(int hwnd, ref Rect rc);
        [DllImport(DllPath)]
        public static extern int GetWindowRect(int hwnd, ref Rectangle rc);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndPrnt, IntPtr hwndChildAfter, string className, string windowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateWindow
            (
            string lpClassName,		// указатель на зарегистрированное имя класса
            string lpWindowName,		// указатель на имя окна
            UInt32 dwStyle,			// стиль окна
            int x,				// горизонтальная позиция окна
            int y,				// вертикальная позиция окна 
            int nWidth,			// ширина окна
            int nHeight,			// высота окна
            IntPtr hWndParent,			// дескриптор родительского или окна владельца
            IntPtr hMenu,			// дескриптор меню или идентификатор дочернего окна
            IntPtr hInstance,			// дескриптор экземпляра приложения
            IntPtr lpParam 			// указатель на данные создания окна
            );

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 message, IntPtr w, IntPtr l);
    }
}
