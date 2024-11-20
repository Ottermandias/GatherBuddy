using Dalamud.Plugin.Services;
using ECommons.Automation.LegacyTaskManager;
using ECommons.GameHelpers;
using GatherBuddy.CustomInfo;
using System;
using System.Diagnostics;
using System.Numerics;

namespace GatherBuddy.AutoGather.Movement
{
    public sealed class AdvancedUnstuck : IDisposable
    {
        private const double UnstuckDuration = 1.5;
        private const double CheckExpiration = 1.0;
        private const float MinMovementDistance = 2.0f;

        private readonly OverrideMovement _movementController = new();
        private DateTime _lastMovement;
        private DateTime _unstuckStart;
        private DateTime _lastCheck;
        private Vector3 _lastPosition;

        public bool IsRunning => _movementController.Enabled;

        public bool Check(bool isPathGenerating, bool isPathing)
        {
            if (!GatherBuddy.Config.AutoGatherConfig.UseExperimentalUnstuck)
            {
                return false;
            }

            var now = DateTime.Now;
            if (!IsRunning)
            {
                var lastCheck = _lastCheck;
                _lastCheck = now;
                if (now.Subtract(lastCheck).TotalSeconds > CheckExpiration)
                {
                    _lastPosition = Player.Position;
                    _lastMovement = now;
                }
                else
                {
                    if (isPathGenerating)
                    {
                        _lastPosition = Player.Position;
                        _lastMovement = now;
                    } 
                    else if (isPathing)
                    {
                        if (_lastPosition.DistanceToPlayer() >= MinMovementDistance)
                        {
                            // Character has moved, update last known position and time
                            _lastPosition = Player.Object.Position;
                            _lastMovement = now;
                        }
                        else if (now.Subtract(_lastMovement).TotalSeconds > GatherBuddy.Config.AutoGatherConfig.NavResetThreshold)
                        {
                            // If the character hasn't moved much
                            GatherBuddy.Log.Debug($"Advanced Unstuck: stuck detected. Moved {_lastPosition.DistanceToPlayer()} yalms in {now.Subtract(_lastMovement).TotalSeconds} seconds.");
                            Start(false);
                        }                        
                    }
                    else
                    {
                        //vnavmesh failed to find a path
                        GatherBuddy.Log.Debug($"Advanced Unstuck: vnavmesh failure detected.");
                        Start(false);
                    }
                }
            }
            return IsRunning;
        }

        public void Force()
        {
            GatherBuddy.Log.Debug("Advanced Unstuck: force start.");
            if (!IsRunning) Start(true);
        }

        private void Start(bool force)
        {
            if (force || DateTime.Now.Subtract(_unstuckStart).TotalSeconds >= GatherBuddy.Config.AutoGatherConfig.NavResetCooldown)
            {
                GatherBuddy.Log.Warning("Advanced Unstuck: the character is stuck; attempting to move and resetting navigation.");
                var rng = new Random();
                float rnd() => (rng.Next(2) == 0 ? -1 : 1) * rng.NextSingle();
                var newPosition = Player.Position + Vector3.Normalize(new Vector3(rnd(), rnd(), rnd())) * 25f;
                _movementController.DesiredPosition = newPosition;
                _movementController.Enabled = true;
                _unstuckStart = DateTime.Now;
                Dalamud.Framework.Update += RunningUpdate;
            }
        }

        private void RunningUpdate(IFramework framework)
        {
            if (DateTime.Now.Subtract(_unstuckStart).TotalSeconds > UnstuckDuration)
            {
                Stop();
            }
        }
        
        private void Stop()
        {
            _movementController.Enabled = false;
            _lastCheck = DateTime.MinValue;
            Dalamud.Framework.Update -= RunningUpdate;
        }

        public void Dispose()
        {
            Stop();
            _movementController.Dispose();
        }
    }
}