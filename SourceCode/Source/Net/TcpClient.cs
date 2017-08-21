/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace Sheng.SailingEase.Net
{
    public class TcpClient : Component
    {
        private string _ServerIP = "127.0.0.1";
        private int _ServerPort = 5000;
        private int _ReceiveBufferSize = 65480;
        private int _SendBufferSize = 65480;
        public List<TcpSocketEventArgs> ServerList = new List<TcpSocketEventArgs>();
        private SynchronizationContext SC = null;
        private bool Releaserbool = true;
        private int _LimitSleep = 10;
        public TcpClient()
        {
            SC = SynchronizationContext.Current;
        }
        [Category("客户端属性"), Description("服务器地址")]
        public string ServerIP
        {
            get { return _ServerIP; }
            set { _ServerIP = value; }
        }
        [Category("客户端属性"), Description("服务器端口")]
        public int ServerPort
        {
            get { return _ServerPort; }
            set { _ServerPort = value; }
        }
        [Category("客户端属性"), Description("接收缓冲区大小")]
        public int ReceiveBufferSize
        {
            get { return _ReceiveBufferSize; }
            set { _ReceiveBufferSize = value; }
        }
        [Category("客户端属性"), Description("发送缓冲区大小")]
        public int SendBufferSize
        {
            get { return _SendBufferSize; }
            set { _SendBufferSize = value; }
        }
        [Category("客户端速度限制"), Description("下载速度限制(单位[M])")]
        public int LimitSleep
        {
            get { return _LimitSleep; }
            set { _LimitSleep = value; }
        }
        [Category("客户端行为"), Description("当连接上服务器后")]
        public event TcpSocketConnectHandel Connectd;
        [Category("客户端行为"), Description("当断开服务器连接时")]
        public event TcpSocketEventHandle DisConnectd;
        [Category("客户端行为"), Description("当接收数据时")]
        public event TcpSocketReceiveHandle Receive;
        [Category("客户端行为"), Description("更新UI界面")]
        public event UIInvokeEventHandle UIInvoke;
        public event OnErrorHandle Error;
        private void OnConnected(TcpSocketEventArgs value)
        {
            if (Connectd != null)
            {
                Connectd(this, value);
            }
        }
        private void OnDisConnected(TcpSocketEventArgs value)
        {
            RemoveServer(value);
            if (DisConnectd != null)
            {
                DisConnectd(this, value);
            }
        }
        private void OnReceive(TcpSocketEventArgs value, int len, byte[] buff, ushort tag)
        {
            if (Receive != null)
            {
                Receive(this, value, len, buff, tag);
            }
        }
        private void OnUIInvoke(object value)
        {
            if (UIInvoke != null)
            {
                object[] parobject = (object[])value;
                TcpSocketEventArgs client = (TcpSocketEventArgs)parobject[2];
                UIInvoke(this, (ushort)parobject[0], parobject[1], client);
            }
        }
        private void OnLimitSleep()
        {
            int intLimitSleep = LimitSleep * 1024 * 1024 / 8 / ReceiveBufferSize / ServerList.Count;
            int btLimitSleep = 0;
            if (intLimitSleep != 0)
            {
                btLimitSleep = 1000 / intLimitSleep;
            }
            foreach (TcpSocketEventArgs e in ServerList)
            {
                e.LimitSleep = btLimitSleep;
            }
        }
        private void OnError(Exception ex, TcpSocketEventArgs e)
        {
            if (Error != null)
            {
                Error(ex, e);
            }
        }
        public void StartConnect(short sign, string filename)
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort);
                Socket ClientSocket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                ClientSocket.BeginConnect(ip, ConnectCallBack, new object[] { ClientSocket, sign, filename });
            }
            catch (Exception ex)
            {
                if (Error != null)
                {
                    Error(ex,null);
                    return;
                }
                else
                    throw ex;
            }
        }
        public void StartConnect(short sign)
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort);
                Socket ClientSocket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                ClientSocket.BeginConnect(ip, ConnectCallBack, new object[] { ClientSocket, sign });
            }
            catch (Exception ex)
            {
                if (Error != null)
                {
                    Error(ex,null);
                    return;
                }
                else
                    throw ex;
            }
        }
        private void RemoveServer(TcpSocketEventArgs ser)
        {
            ServerList.Remove(ser);
        }
        private TcpSocketEventArgs Server;
        private void ConnectCallBack(IAsyncResult ar)
        {
            object[] parobj = (object[])ar.AsyncState;
            Socket ClientSocket = (Socket)parobj[0];
            short sign = (short)parobj[1];
            if (parobj.Length > 2)
            {
                string filename = (string)parobj[2];
                try
                {
                    ClientSocket.EndConnect(ar);
                }
                catch (Exception ex)
                {
                    if (Error != null)
                    {
                        Error(ex,null);
                        return;
                    }
                    else
                        throw ex;
                }
                Server = new TcpSocketEventArgs(SC, OnDisConnected, OnReceive, OnUIInvoke, OnLimitSleep,OnError,
                    ReceiveBufferSize, SendBufferSize, ClientSocket, sign);
                ServerList.Add(Server);
                Server.FileName = filename;
                OnLimitSleep();
                OnConnected(Server);
                Server.Receive();
            }
            else
            {
                try
                {
                    ClientSocket.EndConnect(ar);
                }
                catch(Exception ex)
                {
                    if (Error != null)
                    {
                        Error(ex,null);
                        return;
                    }
                    else
                        throw ex;
                }
                Server = new TcpSocketEventArgs(SC, OnDisConnected, OnReceive, OnUIInvoke, OnLimitSleep, OnError,
                    ReceiveBufferSize, SendBufferSize, ClientSocket, sign);
                ServerList.Add(Server);
                OnLimitSleep();
                OnConnected(Server);
                Server.Receive();
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (Releaserbool)
            {
                Releaserbool = false;
                if (disposing) { }
                TcpSocketEventArgs[] Servers = new TcpSocketEventArgs[ServerList.Count];
                ServerList.CopyTo(Servers);
                foreach (TcpSocketEventArgs e in Servers)
                {
                    e.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }
}
