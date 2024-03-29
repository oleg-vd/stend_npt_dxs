﻿using System;
using System.Threading;
using System.Net.Sockets;

using Cet.IO.Protocols;

/*
 * Copyright 2012, 2013 by Mario Vernari, Cet Electronics
 * Part of "Cet Open Toolbox" (http://cetdevelop.codeplex.com/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace Cet.IO.Net
{
    /// <summary>
    /// Base/abstract implementation of an IP-listener
    /// </summary>
    internal abstract class IpServer
        : ICommServer
    {
        protected IpServer(
            Socket port,
            IProtocol protocol, string ip)
        {
            this.Port = port;
            this.Protocol = protocol;
            this.ip = ip;
            hwaddr = "NOSET";
            addr_internal = 0;
            IPClientOnline = false;
            waitOnLine = false;
        }


        public readonly Socket Port;
        public readonly IProtocol Protocol;

        private Thread _thread;
        protected bool _closing;
        private string ip;
        private int addr_internal;
        private string hwaddr;
        private bool IPClientOnline;
        private bool waitOnLine;

        public bool isWaitOnLine()
        {
            return waitOnLine;
        }

        public void doWaitOnLine()
        {
            waitOnLine = true;
        }

        public void setWaitOnLineFalse()
        {
            waitOnLine = false;
        }


        public bool isIPClientOnline()
        {
            return IPClientOnline;
        }
        
        public void setIPClientOnline(bool online)
        {
            IPClientOnline = online;
        }

        public string getHWAddr()
        {
            return hwaddr;
        }

        public void setHWAddr(string hwaddr)
        {
            this.hwaddr = hwaddr;
            //this.addr = addr;
            this.IPClientOnline = true;
        }


        public int getAddr()
        {
            return addr_internal;
        }
        public void setAddr(ushort addr)
        {
            addr_internal = addr;
            //this.addr = addr;
            this.IPClientOnline = true;
        }
        public string getRemoteIP()
        {
            return ip;
        }

        /// <summary>
        /// 
        /// Indicate whether the server is running
        /// </summary>
        public bool IsRunning { get; protected set; }


        /// <summary>
        /// Start the listener session
        /// </summary>
        public void Start()
        {
            //marks the server running
            this.IsRunning = true;

            this._thread = new Thread(this.Worker);
            this._thread.Start();
        }


        /// <summary>
        /// Handler for the session thread
        /// </summary>
        protected abstract void Worker();


        /// <summary>
        /// Close/abort the listener session
        /// </summary>
        public void Abort()
        {
            this._closing = true;

            if (this._thread != null &&
                this._thread.IsAlive)
            {
                this._thread.Join();
            }
        }


        #region EVT ServeCommand

        public event ServeCommandHandler ServeCommand;


        protected virtual void OnServeCommand(ServerCommData data)
        {
            var handler = this.ServeCommand;

            if (handler != null)
            {
                handler(
                    this,
                    new ServeCommandEventArgs(data));
            }
        }

        #endregion

    }
}
