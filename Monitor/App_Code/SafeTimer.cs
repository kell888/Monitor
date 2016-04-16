using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Diagnostics;

namespace Monitor.App_Code
{
    public class IntEventArgs : EventArgs
    {
        int interval;

        public int Interval
        {
            get { return interval; }
            private set { interval = value; }
        }

        int timeout;

        public int Timeout
        {
            get { return timeout; }
            private set { timeout = value; }
        }

        public IntEventArgs(int interval, int timeout)
            : base()
        {
            this.Interval = interval;
            this.Timeout = timeout;
        }
    }

    public class SafeTimer
    {
        static System.Timers.Timer timer;
        static bool flag = true;
        static object mylock = new object();
        static object state;
        static int timeout;

        public static object State
        {
            get { return SafeTimer.state; }
        }

        public static event EventHandler<IntEventArgs> Checking;

        public static void Dispose()
        {
            if (timer != null)
            {
                if (timer.Enabled)
                {
                    timer.Stop();
                    flag = false;
                }
                timer.Close();
                timer.Dispose();
            }
        }

        public static void Init(int interval, int timeout, object state)
        {
            flag = true;
            SafeTimer.state = state;
            SafeTimer.timeout = timeout;
            timer = new System.Timers.Timer(interval);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            try
            {
                Thread.CurrentThread.IsBackground = false;

                lock (mylock)
                {
                    if (!flag)
                    {
                        return;
                    }
                }

                if (Checking != null)
                    Checking(null, new IntEventArgs((int)timer.Interval, SafeTimer.timeout));
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(true);
                StackFrame sf = st.GetFrame(0);
                string fileName = sf.GetFileName();
                Type type = sf.GetMethod().ReflectedType;
                string assName = type.Assembly.FullName;
                string typeName = type.FullName;
                string methodName = sf.GetMethod().Name;
                int lineNo = sf.GetFileLineNumber();
                int colNo = sf.GetFileColumnNumber();
                Logs.LogError(ErrorLevel.Normal, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", ex.Message);
                Logs.Error("定时任务执行出错：" + ex.Message);
            }
            finally
            {
                timer.Start();
            }
        }
    }
}
