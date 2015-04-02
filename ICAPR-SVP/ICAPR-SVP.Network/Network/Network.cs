using System;
using Fleck;
using ICAPR_SVP.Misc;
using Newtonsoft.Json;
using ICAPR_SVP.Misc.Calibration;

namespace ICAPR_SVP.Network
{
    public class Network
    {
        private volatile bool _isConnected;    //Is server running?
        private int _port;                     //Server port
        private String _host;                  //Host address
        private String _path;                  //URL path
        private String _fullServerUrl;         //Full server URL 
        private WebSocketServer _serverSocket; //Server web socket
        private NetworkDispatcher _dispatcher;
        private IWebSocketConnection _clientSocket;

        public Network(NetworkDispatcher dispatcher,String host,String path,int port)
        {
            this._dispatcher = dispatcher;
            this._port = port;
            this._host = host;
            this._path = path;
            this._isConnected = false;
            this._fullServerUrl = getFullServerUrl(this._host,this._path,this._port);
        }

        private String getFullServerUrl(String host,String path,int port)
        {
            if(host.EndsWith("/"))
                host = host.Remove(host.Length - 1);

            if(path.EndsWith("/"))
                path = path.Remove(path.Length - 1);

            if(path.StartsWith("/"))
                path = path.Substring(1,path.Length - 1);

            return "ws://" + host + ":" + this._port + "/" + path;
        }

        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
        }

        public void startNetwork(Port dispatcherOutputPort,Calibrator calibrator)
        {
            stopNetwork();
            _dispatcher.init(dispatcherOutputPort,calibrator);
            setUpNetwork();
        }

        public void stopNetwork()
        {
            if(this._isConnected)
                if(_serverSocket != null)
                    _serverSocket.Dispose();

            this._isConnected = false;
        }

        public void sendMessage(String msg)
        {
            _clientSocket.Send(msg);
        }

        private void setUpNetwork()
        {
            _serverSocket = new WebSocketServer(this._fullServerUrl);

            _serverSocket.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    _clientSocket = socket;
                    Console.WriteLine("Network: client connected " + socket.ConnectionInfo.ClientIpAddress);
                    this._isConnected = true;
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Network: client disconnected " + socket.ConnectionInfo.ClientIpAddress);
                    this._isConnected = false;
                };
                socket.OnMessage = message =>
                {
                    String result = _dispatcher.dispatchMessage(message);
                    if(result != null)
                        sendMessage(result);
                };
            });
        }
    }

}
