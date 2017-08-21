

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace Sheng.SailingEase.Net
{
    public class TcpSocketEventArgs : EventArgs, IDisposable
    {
        private TcpSocketReceiveDelGate OnReceive;
        private TcpSocketDelgate OnDisConnected;
        private SendOrPostCallback OnUIInvoke;
        private LimitSleepDelgate OnLimitSleep;
        private OnErrorHandle OnError;
        private SynchronizationContext SC = null;
        private Socket Sock;
        public byte[] ReceiveBuffer, SendBuffer;
        public UInt16 SocketTag;
        public FileStream ReceiveFileStream, SendFileStream;
        public long FileSize, FileSeek;
        public string FileName;
        private bool Releaserbool = true;
        private EndPoint GetRemoteEndPoint, GetLocalEndPoint;
        internal int LimitSleep = 0;

        //命令和数据长度的缓冲区
        private byte[] tempby = new byte[6];
        //数据长度缓冲区
        private byte[] headby = new byte[4];
        //命令缓冲区
        private byte[] tagby = new byte[2];
        //当前接收到的数据长度
        private int TempReceiveDateSize;
        //从Buffer里得到的数据长度
        private int TempDateSize;
        //剩余的数据长度
        private int TempSyDateSize;

        private short _sign;
        /// <summary>
        /// 此连接的标志
        /// 作用是允许客户端发起不同类型的连接,如专门用于传输文件的连接,或专门用来获取信息的连接等
        /// 注意:Server端没有这个标识,(值为-1)
        /// Server端建立连接时不知道这个连接的标识是什么
        /// Server端只管解析数据包中的命令来处理数据包
        /// </summary>
        public short Sign
        {
            get
            {
                return this._sign;
            }
            private set
            {
                this._sign = value;
            }
        }

        /// <summary>
        /// 服务端专用
        /// 服务端不知道连接的标志,将连接标志置为-1
        /// </summary>
        /// <param name="TcpSc"></param>
        /// <param name="TcpEventDisConnected"></param>
        /// <param name="TcpEventReceive"></param>
        /// <param name="TcpEventUpDateUi"></param>
        /// <param name="TcpLimitSleep"></param>
        /// <param name="OnError"></param>
        /// <param name="TcpReceiveBufferSize"></param>
        /// <param name="TcpSendBufferSize"></param>
        /// <param name="TcpSocket"></param>
        internal TcpSocketEventArgs(SynchronizationContext TcpSc, TcpSocketDelgate TcpEventDisConnected, 
            TcpSocketReceiveDelGate TcpEventReceive,SendOrPostCallback TcpEventUpDateUi, LimitSleepDelgate TcpLimitSleep, 
            OnErrorHandle OnError, int TcpReceiveBufferSize,int TcpSendBufferSize, Socket TcpSocket) :
            this(TcpSc, TcpEventDisConnected, TcpEventReceive, TcpEventUpDateUi, TcpLimitSleep, OnError, TcpReceiveBufferSize,
             TcpSendBufferSize, TcpSocket, -1)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="TcpSc">SC同步上下文</param>
        /// <param name="TcpEventDisConnected">当有断开连接时事件的激活方法</param>
        /// <param name="TcpEventReceive">接收数据时事件的激活方法</param>
        /// <param name="TcpEventUpDateUi">更新UI界面事件的激活方法</param>
        /// <param name="TcpRemoveSocket">移除服务端或客户端的方法</param>
        /// <param name="TcpReceiveBufferSize">接收缓冲区的大小</param>
        /// <param name="TcpSendBufferSize">发送缓冲区的大小</param>
        /// <param name="TcpSocket">Socket</param>
        internal TcpSocketEventArgs(SynchronizationContext TcpSc, TcpSocketDelgate TcpEventDisConnected,
            TcpSocketReceiveDelGate TcpEventReceive, SendOrPostCallback TcpEventUpDateUi, LimitSleepDelgate TcpLimitSleep,
            OnErrorHandle OnError, int TcpReceiveBufferSize, int TcpSendBufferSize, Socket TcpSocket, short sign)
        {
            this.Sign = sign;

            SC = TcpSc;
            OnDisConnected = TcpEventDisConnected;
            OnReceive = TcpEventReceive;
            OnUIInvoke = TcpEventUpDateUi;
            OnLimitSleep = TcpLimitSleep;
            this.OnError = OnError;

            Sock = TcpSocket;
            Sock.SendBufferSize = TcpSendBufferSize;
            Sock.ReceiveBufferSize = TcpReceiveBufferSize;
            ReceiveBuffer = new byte[Sock.ReceiveBufferSize];
            SendBuffer = new byte[Sock.SendBufferSize];
            GetRemoteEndPoint = Sock.RemoteEndPoint;
            GetLocalEndPoint = Sock.LocalEndPoint;

            int keepAlive = -1744830460; // SIO_KEEPALIVE_VALS
            byte[] inValue = new byte[] { 1, 0, 0, 0, 0x10, 0x27, 0, 0, 0xe8, 0x03, 0, 0 }; // True, 10秒, 1 秒
            Sock.IOControl(keepAlive, inValue, null);
            //Sock.IOControl(IOControlCode.KeepAliveValues, inValue, null);
            //设置 KeepAlive 为 10 秒，检查间隔为 1 秒。如果拨掉客户端网线
            //服务器的 Socket.Receive() 会在 10 秒后断开连接
        }
      
        /// <summary>
        /// 更新UI操作
        /// </summary>
        /// <param name="tag">更新指令</param>
        public void UIInvoke(ushort invokeCommand, object tag)
        {
            SC.Post(OnUIInvoke, new object[] { invokeCommand, tag, this });
        }
       
        /// <summary>
        /// 接收命令和数据长度
        /// </summary>
        public void Receive()
        {
            try
            {
                if (Releaserbool)
                {
                    //选接收6个字节,把命令和长度接收下来
                    //然后再回调接收后面的数据
                    Sock.BeginReceive(tempby, 0, tempby.Length, SocketFlags.None, ReceiveCallBack, null);
                }
            }
            catch (ObjectDisposedException)
            {
                Debug.Assert(false);
            }
        }

        /// <summary>
        /// 命令和数据长度转换
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int ReceiveLen = Sock.EndReceive(ar);
                if (ReceiveLen != 0)
                {
                    //这里要考虑一个问题,有可能接收到的这个数据,是其它程序或进程向这个端口发来的数据
                    //比如客户端remoting的端口设置成了这个TCP端口
                    //自然不是按我TCP协定来,在逻辑上是无效数据,但这里无法判断
                    //造成的一个问题是,用前四个字节得到的长度是一个错误的,不存在的,无效的长度
                    //暂时通过  TempDateSize 是否大于  ReceiveBuffer.Length 来判断

                    Array.Copy(tempby, 0, headby, 0, 4);
                    TempDateSize = BitConverter.ToInt32(headby, 0);
                    TempSyDateSize = TempDateSize;
                    Array.Copy(tempby, 4, tagby, 0, 2);
                    SocketTag = BitConverter.ToUInt16(tagby, 0);

                    //Sock.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, ReceiveDateCallBack, null);
                    if (TempDateSize > ReceiveBuffer.Length)
                    {
                        this.Dispose();
                    }
                    else
                    {
                        Sock.BeginReceive(ReceiveBuffer, 0, TempDateSize, SocketFlags.None, ReceiveDateCallBack, null);
                    }
                }
                else
                {
                    Dispose();
                }
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10053 || e.ErrorCode == 10054)
                {
                    Dispose();
                }
            }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// 开始接收数据
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveDateCallBack(IAsyncResult ar)
        {
            try
            {
                int ReceiveLen = Sock.EndReceive(ar);
                if (ReceiveLen != 0)
                {
                   

                    //Thread.Sleep(LimitSleep);
                    Thread.Sleep(5);

                    TempSyDateSize -= ReceiveLen;
                    TempReceiveDateSize += ReceiveLen;

                    //如果当前接收到的长度  <  这个数据大小的长度 说明这次发送的数据没有接收完,那么继续接收
                    //客户端接收文件时确实会走到这里来,原因不明,不知道原作者为什么这里要这样写
                    //按说发文件时每包都会是固定的buffer长度
                    if (TempReceiveDateSize < TempDateSize)
                    {
                        Sock.BeginReceive(ReceiveBuffer, TempReceiveDateSize, TempSyDateSize, SocketFlags.None, ReceiveDateCallBack, null);
                    }
                    //如果当前接收的长度   =  这个数据大小的长度 说明这次发送的数据已接收完,那么进行第二次的接收
                    else if (TempReceiveDateSize == TempDateSize)
                    {
                        OnReceive(this, TempDateSize, ReceiveBuffer, SocketTag);
                        TempReceiveDateSize = 0;
                        Receive();
                    }
                }
                else
                {
                    Dispose();
                }
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10053 || e.ErrorCode == 10054)
                {
                    Dispose();
                }
            }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// 向接收端发送数据
        /// </summary>
        /// <param name="dataTag">命令</param>
        /// <param name="buffer">数据</param>
        public void Send(UInt16 dataTag, byte[] buffer)
        {
            if (buffer == null)
                buffer = new byte[] { 0 };

            //实际数据长度
            byte[] headby = BitConverter.GetBytes(buffer.Length);
            //命令
            byte[] tagby = BitConverter.GetBytes(dataTag);
            //实际数据
            byte[] MYSendBuffer = new byte[6 + buffer.Length];
            Array.Copy(headby, 0, MYSendBuffer, 0, 4);
            Array.Copy(tagby, 0, MYSendBuffer, 4, 2);
            Array.Copy(buffer, 0, MYSendBuffer, 6, buffer.Length);
            Sock.BeginSend(MYSendBuffer, 0, MYSendBuffer.Length, SocketFlags.None, SendCallBack, null);

        }

        private void SendCallBack(IAsyncResult ar)
        {
            try
            {
                int SendLen = Sock.EndSend(ar);
                if (SendLen == 0)
                {
                    Dispose();
                }
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10053 || e.ErrorCode == 10054)
                {
                    Dispose();
                }
            }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// 获取远程终结点
        /// </summary>
        public EndPoint RemoteEndPoint
        {
            get { return GetRemoteEndPoint; }
        }

        /// <summary>
        /// 获取本地终结点
        /// </summary>
        public EndPoint LocalEndPoint
        {
            get { return GetLocalEndPoint; }
        }

        ~TcpSocketEventArgs()
        {
            Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (Releaserbool)
            {
                Releaserbool = false;
                if (disposing)
                {
                    if (ReceiveBuffer != null) { ReceiveBuffer = null; }
                    if (SendBuffer != null) { SendBuffer = null; }
                    if (tempby != null) { tempby = null; }
                    if (headby != null) { headby = null; }
                    if (tagby != null) { tagby = null; }
                }
                OnLimitSleep();
                OnDisConnected(this);
                if (SendFileStream != null) { SendFileStream.Close(); }
                if (ReceiveFileStream != null) { ReceiveFileStream.Close(); }
                try
                {
                    if (Sock != null)
                    {
                        Sock.Shutdown(SocketShutdown.Both);
                        Sock.Close();
                    }
                }
                catch (SocketException e)
                {
                    System.Diagnostics.Debug.WriteLine("SocketError：" + e.ErrorCode);
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
