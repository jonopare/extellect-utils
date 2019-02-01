#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;

namespace Extellect.Utilities.Net
{
    public static class SocketExtensions
    {
        public static Task ConnectAsync(this Socket socket, EndPoint endPoint)
        {
            return Task.Factory.FromAsync(
                (asyncCallback, asyncState) => socket.BeginConnect(endPoint, asyncCallback, asyncState),
                (asyncResult) => socket.EndConnect(asyncResult),
                null);
        }

        public static Task<Socket> AcceptAsync(this Socket socket, int receiveSize)
        {
            return Task.Factory.FromAsync(
                (asyncCallback, asyncState) => socket.BeginAccept(receiveSize, asyncCallback, asyncState),
                (asyncResult) => socket.EndAccept(asyncResult),
                null);
        }

        public static Task DisconnectAsync(this Socket socket, bool reuseSocket)
        {
            return Task.Factory.FromAsync(
                (asyncCallback, asyncState) => socket.BeginDisconnect(reuseSocket, asyncCallback, asyncState),
                (asyncResult) => socket.EndDisconnect(asyncResult),
                null);
        }

        public static Task<int> ReceiveAsync(this Socket socket, byte[] buffer, int offset, int count, SocketFlags socketFlags)
        {
            return Task.Factory.FromAsync(
                (asyncCallback, asyncState) => socket.BeginReceive(buffer, offset, count, socketFlags, asyncCallback, asyncState),
                (asyncResult) => socket.EndReceive(asyncResult),
                null);
        }

        public static Task<int> SendAsync(this Socket socket, byte[] buffer, int offset, int count, SocketFlags socketFlags)
        {
            return Task.Factory.FromAsync(
                (asyncCallback, asyncState) => socket.BeginSend(buffer, offset, count, socketFlags, asyncCallback, asyncState),
                (asyncResult) => socket.EndSend(asyncResult),
                null);
        }
    }
}
