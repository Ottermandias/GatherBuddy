using Dalamud.Plugin.Services;
using ECommons.Automation.LegacyTaskManager;
using ECommons.GameHelpers;
using GatherBuddy.CustomInfo;
using System;
using System.Diagnostics;
using System.Numerics;

namespace GatherBuddy.AutoGather.Movement
{
    public enum AdvancedUnstuckCheckResult
    {
        Pass,
        Wait,
        Fail
    }
    public sealed class AdvancedUnstuck : IDisposable
    {
        private const double UnstuckDuration = 1.0;
        private const double CheckExpiration = 1.0;
        private const float MinMovementDistance = 2.0f;

        private readonly OverrideMovement _movementController = new();
        private DateTime _lastMovement;
        private DateTime _unstuckStart;
        private DateTime _lastCheck;
        private Vector3 _lastPosition;
        private bool _lastWasFailure;

        public bool IsRunning => _movementController.Enabled;

        public AdvancedUnstuckCheckResult Check(Vector3 destination, bool isPathGenerating, bool isPathing)
        {
            if (IsRunning)
                return AdvancedUnstuckCheckResult.Fail;

            var now = DateTime.Now;

            //On cooldown, not navigating or near the destination: disable tracking and reset
            if (now.Subtract(_unstuckStart).TotalSeconds < GatherBuddy.Config.AutoGatherConfig.NavResetCooldown
                || destination == default
                || destination.DistanceToPlayer() < 3)
            {
                _lastCheck = DateTime.MinValue;
                return AdvancedUnstuckCheckResult.Pass;
            }

            var lastCheck = _lastCheck;
            _lastCheck = now;

            //Tracking wasn't active for 1 second or was reset: restart tracking from the current position
            if (now.Subtract(lastCheck).TotalSeconds > CheckExpiration)
            {
                _lastPosition = Player.Position;
                _lastMovement = now;
                _lastWasFailure = false;
                return AdvancedUnstuckCheckResult.Pass;
            }

            //vnavmesh is generating path: update current position
            if (isPathGenerating)
            {
                _lastPosition = Player.Position;
                _lastMovement = now;
            }
            //vnavmesh is moving...
            else if (isPathing)
            {
                //...and quite fast: update current position
                if (_lastPosition.DistanceToPlayer() >= MinMovementDistance)
                {
                    _lastPosition = Player.Object.Position;
                    _lastMovement = now;
                }
                //...but not fast enough: unstuck
                else if (now.Subtract(_lastMovement).TotalSeconds > GatherBuddy.Config.AutoGatherConfig.NavResetThreshold)
                {
                    GatherBuddy.Log.Warning($"Advanced Unstuck: the character is stuck. Moved {_lastPosition.DistanceToPlayer()} yalms in {now.Subtract(_lastMovement).TotalSeconds} seconds.");
                    Start();
                }
            }
            //Not generating path and not moving for 2 consecutive framework updates: unstuck
            else if (_lastWasFailure)
            {
                GatherBuddy.Log.Warning($"Advanced Unstuck: vnavmesh failure detected.");
                Start();
            }

            //Not generating path and not moving: remember that fact and exit main loop
            _lastWasFailure = !isPathGenerating && !isPathing;
            return IsRunning ? AdvancedUnstuckCheckResult.Fail : _lastWasFailure ? AdvancedUnstuckCheckResult.Wait : AdvancedUnstuckCheckResult.Pass;
        }

        public void Force()
        {
            if (!IsRunning)
            {
                GatherBuddy.Log.Warning("Advanced Unstuck: force start.");
                Start();
            }
        }

        private void Start()
        {
            var rng = new Random();
            float rnd() => (rng.Next(2) == 0 ? -1 : 1) * rng.NextSingle();
            var newPosition = Player.Position + Vector3.Normalize(new Vector3(rnd(), rnd(), rnd())) * 25f;
            _movementController.DesiredPosition = newPosition;
            _movementController.Enabled = true;
            _unstuckStart = DateTime.Now;
            Dalamud.Framework.Update += RunningUpdate;
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