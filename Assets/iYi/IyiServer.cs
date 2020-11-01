using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Iyi
{
    public class IyiServer : MonoBehaviour
    {
        UdpClient udp;
        Thread thread;

        public Received faceData { get; private set; }
        public byte[] faceDataRaw { get; private set; }
        public event Action<Received> OnReceived;

        [SerializeField]
        readonly int port = 11111;

        void Start()
        {
            udp = new UdpClient(port);
            udp.Client.ReceiveTimeout = int.MaxValue;
            thread = new Thread(new ThreadStart(ReceiveThread));
            thread.Start();
        }

        void OnDestroy()
        {
            thread.Abort();
            udp.Close();
        }

        private void ReceiveThread()
        {
            while (true)
            {
                IPEndPoint ep = null;
                var data = faceDataRaw = udp.Receive(ref ep);

                var received = Protocol.ToReceived(data);

                OnReceived?.Invoke(received);
                if (faceData == null || faceData.timestamp < received.timestamp)
                {
                    faceData = received;
                }
            }
        }
    }
}
