﻿using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Monitor
{
    internal class VlcPlayer
    {
        private IntPtr libvlc_instance;
        private IntPtr libvlc_media_player;
        private Control control;
        private int wndHandle;
        private IntPtr hWndC;

        private double duration_;

        public VlcPlayer(string pluginPath)
        {
            string plugin_arg = "--plugin-path=" + pluginPath;
            string[] arguments = { "-I", "dummy", "--ignore-config", "--no-video-title", plugin_arg };
            libvlc_instance = LibVlcAPI.libvlc_new(arguments);

            libvlc_media_player = LibVlcAPI.libvlc_media_player_new(libvlc_instance);
        }

        public void SetRenderWindow(Control control)
        {
            if (libvlc_instance != IntPtr.Zero && wndHandle != 0)
            {
                this.control = control;
                this.wndHandle = (int)control.Handle;
                LibVlcAPI.libvlc_media_player_set_hwnd(libvlc_media_player, wndHandle);
            }
        }

        public void PlayFile(string filePath)
        {
            IntPtr libvlc_media = LibVlcAPI.libvlc_media_new_path(libvlc_instance, filePath);
            if (libvlc_media != IntPtr.Zero)
            {
                LibVlcAPI.libvlc_media_parse(libvlc_media);
                duration_ = LibVlcAPI.libvlc_media_get_duration(libvlc_media) / 1000.0;

                LibVlcAPI.libvlc_media_player_set_media(libvlc_media_player, libvlc_media);
                LibVlcAPI.libvlc_media_release(libvlc_media);

                LibVlcAPI.libvlc_media_player_play(libvlc_media_player);

                byte[] lpszName = new byte[100];
                hWndC = LibVlcAPI.capCreateCaptureWindowA(lpszName, LibVlcAPI.WS_CHILD | LibVlcAPI.WS_VISIBLE, control.Left, control.Top, control.Width, control.Height, control.Handle, 0);
            }
        }

        public void PlayURL(string url)
        {
            IntPtr libvlc_media = LibVlcAPI.libvlc_media_new_location(libvlc_instance, url);
            if (libvlc_media != IntPtr.Zero)
            {
                LibVlcAPI.libvlc_media_parse(libvlc_media);
                duration_ = LibVlcAPI.libvlc_media_get_duration(libvlc_media) / 1000.0;

                LibVlcAPI.libvlc_media_player_set_media(libvlc_media_player, libvlc_media);
                LibVlcAPI.libvlc_media_release(libvlc_media);

                LibVlcAPI.libvlc_media_player_play(libvlc_media_player);

                byte[] lpszName = new byte[100];
                hWndC = LibVlcAPI.capCreateCaptureWindowA(lpszName, LibVlcAPI.WS_CHILD | LibVlcAPI.WS_VISIBLE, control.Left, control.Top, control.Width, control.Height, control.Handle, 0);
            }
        }

        public void Pause()
        {
            if (libvlc_media_player != IntPtr.Zero)
            {
                LibVlcAPI.libvlc_media_player_pause(libvlc_media_player);
            }
        }

        public void Stop()
        {
            if (libvlc_media_player != IntPtr.Zero)
            {
                LibVlcAPI.libvlc_media_player_stop(libvlc_media_player);
            }
        }

        public bool Snap(string path)
        {
            IntPtr hBmp = Marshal.StringToHGlobalAnsi(path);
            return LibVlcAPI.SendMessage(hWndC, LibVlcAPI.WM_CAP_SAVEDIB, 0, hBmp.ToInt32());
        }

        public double GetPlayTime()
        {
            return LibVlcAPI.libvlc_media_player_get_time(libvlc_media_player) / 1000.0;
        }

        public void SetPlayTime(double seekTime)
        {
            LibVlcAPI.libvlc_media_player_set_time(libvlc_media_player, (Int64)(seekTime * 1000));
        }

        public int GetVolume()
        {
            return LibVlcAPI.libvlc_audio_get_volume(libvlc_media_player);
        }

        public void SetVolume(int volume)
        {
            LibVlcAPI.libvlc_audio_set_volume(libvlc_media_player, volume);
        }

        public void SetFullScreen(bool istrue)
        {
            LibVlcAPI.libvlc_set_fullscreen(libvlc_media_player, istrue ? 1 : 0);
        }

        public double Duration()
        {
            return duration_;
        }

        public string Version()
        {
            return LibVlcAPI.libvlc_get_version();
        }

        public void SetPlaySpeed(float rate)
        {
            LibVlcAPI.libvlc_media_player_set_rate(libvlc_media_player, rate);
        }

        public void Dispose()
        {
            LibVlcAPI.libvlc_media_player_release(libvlc_media_player);
        }
    }

    internal static class LibVlcAPI
    {
        private const int WM_USER = 0x400;
        internal const int WS_CHILD = 0x40000000;
        internal const int WS_VISIBLE = 0x10000000;
        private const int WM_CAP_START = WM_USER;
        internal const int WM_CAP_SAVEDIB = WM_CAP_START + 25;

        [DllImport("User32.dll")]
        internal static extern bool SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        [DllImport("avicap32.dll")]
        internal static extern IntPtr capCreateCaptureWindowA(byte[] lpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, int nID);

        internal struct PointerToArrayOfPointerHelper
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            public IntPtr[] pointers;
        }

        public static IntPtr libvlc_new(string[] arguments)
        {
            PointerToArrayOfPointerHelper argv = new PointerToArrayOfPointerHelper();
            argv.pointers = new IntPtr[11];

            for (int i = 0; i < arguments.Length; i++)
            {
                argv.pointers[i] = Marshal.StringToHGlobalAnsi(arguments[i]);
            }

            IntPtr argvPtr = IntPtr.Zero;
            try
            {
                int size = Marshal.SizeOf(typeof(PointerToArrayOfPointerHelper));
                argvPtr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(argv, argvPtr, false);

                return libvlc_new(arguments.Length, argvPtr);
            }
            finally
            {
                for (int i = 0; i < arguments.Length + 1; i++)
                {
                    if (argv.pointers[i] != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(argv.pointers[i]);
                    }
                }
                if (argvPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(argvPtr);
                }
            }
        }

        public static IntPtr libvlc_media_new_path(IntPtr libvlc_instance, string path)
        {
            IntPtr pMrl = IntPtr.Zero;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(path);
                pMrl = Marshal.AllocHGlobal(bytes.Length + 1);
                Marshal.Copy(bytes, 0, pMrl, bytes.Length);
                Marshal.WriteByte(pMrl, bytes.Length, 0);
                return libvlc_media_new_path(libvlc_instance, pMrl);
            }
            finally
            {
                if (pMrl != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pMrl);
                }
            }
        }

        public static IntPtr libvlc_media_new_location(IntPtr libvlc_instance, string path)
        {
            IntPtr pMrl = IntPtr.Zero;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(path);
                pMrl = Marshal.AllocHGlobal(bytes.Length + 1);
                Marshal.Copy(bytes, 0, pMrl, bytes.Length);
                Marshal.WriteByte(pMrl, bytes.Length, 0);
                return libvlc_media_new_path(libvlc_instance, pMrl);
            }
            finally
            {
                if (pMrl != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pMrl);
                }
            }
        }

        // ----------------------------------------------------------------------------------------
        // 以下是libvlc.dll导出函数

        // 创建一个libvlc实例，它是引用计数的
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_new(int argc, IntPtr argv);

        // 释放libvlc实例
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_release(IntPtr libvlc_instance);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern String libvlc_get_version();

        // 从视频来源(例如Url)构建一个libvlc_meida
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_media_new_location(IntPtr libvlc_instance, IntPtr path);

        // 从本地文件路径构建一个libvlc_media
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_media_new_path(IntPtr libvlc_instance, IntPtr path);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_release(IntPtr libvlc_media_inst);

        // 创建libvlc_media_player(播放核心)
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr libvlc_media_player_new(IntPtr libvlc_instance);

        // 将视频(libvlc_media)绑定到播放器上
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_set_media(IntPtr libvlc_media_player, IntPtr libvlc_media);

        // 设置图像输出的窗口
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_set_hwnd(IntPtr libvlc_mediaplayer, Int32 drawable);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_play(IntPtr libvlc_mediaplayer);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_pause(IntPtr libvlc_mediaplayer);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_stop(IntPtr libvlc_mediaplayer);

        // 解析视频资源的媒体信息(如时长等)
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_parse(IntPtr libvlc_media);

        // 返回视频的时长(必须先调用libvlc_media_parse之后，该函数才会生效)
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Int64 libvlc_media_get_duration(IntPtr libvlc_media);

        // 当前播放的时间
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Int64 libvlc_media_player_get_time(IntPtr libvlc_mediaplayer);

        // 设置播放位置(拖动)
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_set_time(IntPtr libvlc_mediaplayer, Int64 time);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_release(IntPtr libvlc_mediaplayer);

        // 获取音量
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int libvlc_audio_get_volume(IntPtr libvlc_media_player);

        // 设置音量
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_audio_set_volume(IntPtr libvlc_media_player, int volume);

        // 设置全屏
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_set_fullscreen(IntPtr libvlc_media_player, int isFullScreen);

        // 设置播放速度
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_set_rate(IntPtr libvlc_mediaplayer, float rate);

        // 获取播放速度
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern float libvlc_media_player_get_rate(IntPtr libvlc_mediaplayer);
    }
}