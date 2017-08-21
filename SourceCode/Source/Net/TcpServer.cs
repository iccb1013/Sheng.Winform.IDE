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
    public delegate void LimitSleepDelgate();
    public delegate void TcpSocketEventHandle(object sender, TcpSocketEventArgs e);
    public delegate void TcpSocketConnectHandel(object sender, TcpSocketEventArgs e);
    public delegate void TcpSocketDelgate(TcpSocketEventArgs e);
    public delegate void UIInvokeEventHandle(object sender, ushort invokeCommand,object tag, TcpSocketEventArgs e);
    public delegate void TcpSocketReceiveDelGate(TcpSocketEventArgs e, int ReceiveLen, byte[] ReceiveBuff, ushort Tag);
    public delegate void TcpSocketReceiveHandle(object sender, TcpSocketEventArgs e, int ReceiveLen, byte[] ReceiveBuff, ushort Tag);
    public delegate void OnErrorHandle(Exception ex, TcpSocketEventArgs e);
    public class TcpServer : Component
    {
        private SynchronizationContext SC = null;
        private Socket ServerSocket;
        private string _ServerIP = "127.0.0.1";
        private int _ServerPort = 5000;
        private int _SendBufferSize = 65480;
        private int _ReceiveBufferSize = 65480;
        private bool Releaserbool = true, Listenbool = true;
        public List<TcpSocketEventArgs> ClientList = new List<TcpSocketEventArgs>();
        public TcpServer()
        {
            SC = SynchronizationContext.Current;
        }
        [Category("服务端属性"), Description("服务器地址")]
        public string ServerIP
        {
            get { return _ServerIP; }
            set { _ServerIP = value; }
        }
        [Category("服务端属性"), Description("服务器端口")]
        public int ServerPort
        {
            get { return _ServerPort; }
            set { _ServerPort = value; }
        }
        [Category("服务端属性"), Description("发送缓冲区大小")]
        public int SendBufferSize
        {
            get { return _SendBufferSize; }
            set { _SendBufferSize = value; }
        }
        [Category("服务端属性"), Description("接收缓冲区大小")]
        public int ReceiveBufferSize
        {
            get { return _ReceiveBufferSize; }
            set { _ReceiveBufferSize = value; }
        }
        public event TcpSocketEventHandle Connected;
        public event TcpSocketReceiveHandle Receive;
        public event TcpSocketEventHandle DisConnected;
        public event UIInvokeEventHandle UIInvoke;
        public event OnErrorHandle Error;
        private void OnConnected(TcpSocketEventArgs value)
        {
            if (Connected != null)
            {
                Connected(this, value);
            }
        }
        private void OnReceive(TcpSocketEventArgs value, int len, byte[] buff, ushort tag)
        {
            if (Receive != null)
            {
                Receive(this, value, len, buff, tag);
            }
        }
        private void OnDisConnected(TcpSocketEventArgs value)
        {
            RemoveClient(value);
            if (DisConnected != null)
            {
                DisConnected(this, value);
            }
        }
        private void OnUIInvoke(object value)
        {
            if (UIInvoke != null)
            {
                object[] parobj = (object[])value;
                UIInvoke(this,(ushort)parobj[0], parobj[1], (TcpSocketEventArgs)parobj[2]);
            }
        }
        private void OnError(Exception ex, TcpSocketEventArgs e)
        {
            if (Error != null)
            {
                Error(ex, e);
            }
        }
        private void OnLimitSleep()
        {
        }
        private void RemoveClient(TcpSocketEventArgs client)
        {
            ClientList.Remove(client);
        }
        public bool StartServer()
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort);
                ServerSocket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                ServerSocket.Bind(ip);
                ServerSocket.Listen(100);
                ServerSocket.BeginAccept(AcceptCallBack, null);
                return true;
            }
            catch (Exception ex)
            {
                if (Error != null)
                {
                    Error(ex,null);
                    return false;
                }
                else
                    throw ex;
            }
        }
        private void AcceptCallBack(IAsyncResult ar)
        {
            if (Listenbool)
            {
                Socket ClientSocket;
                try
                {
                    ClientSocket = ServerSocket.EndAccept(ar);
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
                TcpSocketEventArgs cl = new TcpSocketEventArgs(SC, OnDisConnected, OnReceive,
                    OnUIInvoke, OnLimitSleep, OnError,ReceiveBufferSize, SendBufferSize, ClientSocket);
                ClientList.Add(cl);
                OnConnected(cl);
                try
                {
                    ServerSocket.BeginAccept(AcceptCallBack, null);
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
                cl.Receive();
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (Releaserbool)
            {
                Releaserbool = false;
                Listenbool = false;
                if (disposing) { }
                TcpSocketEventArgs[] Clients = new TcpSocketEventArgs[ClientList.Count];
                ClientList.CopyTo(Clients);
                foreach (TcpSocketEventArgs e in Clients)
                {
                    e.Dispose();
                }
                if (ServerSocket != null)
                {
                    ServerSocket.Close();
                    ServerSocket = null;
                }
                base.Dispose(disposing);
            }
        }
    }
}
