using Dalamud.Plugin.Ipc;
using Dalamud.Plugin.Ipc.Exceptions;
using Dalamud.Plugin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GatherBuddy.Plugin
{
    public class VNavmeshIpc : IDisposable
    {
        private readonly ICallGateSubscriber<bool> _isready;
        private readonly ICallGateSubscriber<bool> _reload;
        private readonly ICallGateSubscriber<Vector3, bool, bool> _pathfindAndMoveTo;
        private readonly ICallGateSubscriber<bool> _isPathing;
        private readonly string _guid;
        public VNavmeshIpc() 
        {
            _isready = Dalamud.PluginInterface.GetIpcSubscriber<bool>("vnavmesh.Nav.IsReady");
            _reload = Dalamud.PluginInterface.GetIpcSubscriber<bool>("vnavmesh.Nav.Reload");
            _pathfindAndMoveTo = Dalamud.PluginInterface.GetIpcSubscriber<Vector3, bool, bool>("vnavmesh.SimpleMove.PathfindAndMoveTo");
            _isPathing = Dalamud.PluginInterface.GetIpcSubscriber<bool>("vnavmesh.Path.IsRunning");

            try
            {
                var isReady = _isready.InvokeFunc();
                if (isReady)
                {
                    _guid = Guid.NewGuid().ToString();
                }
                else
                {
                    _guid = string.Empty;
                }
            }
            catch (IpcNotReadyError)
            {
                _guid = string.Empty;
            }
        }

        public void Dispose()
        {

        }

        public bool IsPathing()
        {
            try
            {
                return _isPathing.InvokeFunc();
            }
            catch (IpcNotReadyError)
            {
                return true;
            }
        }

        public bool IsReady()
        {
            try
            {
                return _isready.InvokeFunc();
            }
            catch (IpcNotReadyError)
            {
                return false;
            }
        }

        public bool Reload()
        {
            try
            {
                return _reload.InvokeFunc();
            }
            catch (IpcNotReadyError)
            {
                return false;
            }
        }

        public bool PathfindAndMoveTo(Vector3 destination, bool fly = true)
        {
            try
            {
                return _pathfindAndMoveTo.InvokeFunc(destination, fly);
            }
            catch (IpcNotReadyError)
            {
                return false;
            }
        }
    }
}
