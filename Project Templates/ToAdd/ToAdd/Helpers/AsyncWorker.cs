using DGII_PFD.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DGII_PFD.Helpers
{
    public class AsyncWorker
    {
        private AsyncWorker() { }
        public static AsyncWorker Instance { get { return SingletonHolder.Instance; } }

        public bool Completed = false;
        public Task RunningTask { get; private set; }
        
        
        public void LoadADUsers()
        {
            RunningTask = Task.Factory.StartNew(() =>
                {
                    Completed = false;
                    Completed = ADUsers.Instance.Completed;
                });
        }

        public void RunAsynchronously(Action method, Action callback = null)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    Completed = false;
                    method();
                    Completed = true;
                }
                catch{}

                if (callback != null) callback();
            });
        }

        private class SingletonHolder
        {
            static SingletonHolder() { }
            internal static readonly AsyncWorker Instance = new AsyncWorker();
        }

    }
}