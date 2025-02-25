﻿using Photon.Deterministic;

namespace Quantum
{
    [System.Serializable]
    public sealed unsafe partial class ApplyPhysicsEvent : FrameEvent
    {
        [System.Flags]
        public enum Usability : byte
        {
            Grounded = 1 << 0,
            Airborne = 1 << 1
        }

        public Usability UsabilityType;

        public FPVector2 StartOffset;
        public FPVector2 EndOffset;

        public PhysicsSettings UnchargedSettings;
        public PhysicsSettings FullyChargedSettings;

        public bool ResetPosition;

        public override void Begin(Frame f, QuantumAnimationEvent parent, ref CharacterControllerSystem.Filter filter, Input input, int frame)
        {
            Log.Debug("Applying physics!");

            filter.PhysicsBody->GravityScale = 0;
            filter.CharacterController->ApplyPhysicsPosition = filter.Transform->Position + StartOffset;
        }

        public override void Update(Frame f, QuantumAnimationEvent parent, ref CharacterControllerSystem.Filter filter, Input input, int frame, int elapsedFrames)
        {
            Log.Debug("Updating physics!");

            PhysicsSettings settings = filter.CharacterController->LerpFromAnimationHold(PhysicsSettings.Lerp, UnchargedSettings, FullyChargedSettings);
            FPVector2 newPos = filter.CharacterController->ApplyPhysicsPosition + GetPositionAtTime(settings, (FP)elapsedFrames / Length, filter.CharacterController->MovementDirection);

            if (!settings.UseXAxis)
                newPos.X = filter.Transform->Position.X;

            if (!settings.UseYAxis)
                newPos.Y = filter.Transform->Position.Y;

            if (newPos.X > filter.Transform->Position.X && filter.CharacterController->GetNearbyCollider(Colliders.RightWall) ||
                newPos.X < filter.Transform->Position.X && filter.CharacterController->GetNearbyCollider(Colliders.LeftWall) ||
                newPos.Y > filter.Transform->Position.Y && filter.CharacterController->GetNearbyCollider(Colliders.Ceiling) ||
                newPos.Y < filter.Transform->Position.Y && filter.CharacterController->GetNearbyCollider(Colliders.Ground))
                return;

            filter.Transform->Position = newPos;
        }

        public static FPVector2 GetPositionAtTime(PhysicsSettings settings, FP normalizedTime, int direction)
        {
            return new FPVector2(settings.XCurve.Evaluate(normalizedTime) * settings.XForce * direction, settings.YCurve.Evaluate(normalizedTime) * settings.YForce);
        }

        public override void End(Frame f, QuantumAnimationEvent parent, ref CharacterControllerSystem.Filter filter, Input input, int frame)
        {
            Log.Debug("Cleaning up physics!");

            filter.PhysicsBody->GravityScale = 1;

            if (ResetPosition)
                filter.Transform->Position = filter.CharacterController->ApplyPhysicsPosition - StartOffset;

            filter.Transform->Position += new FPVector2(EndOffset.X * filter.CharacterController->MovementDirection, EndOffset.Y);
        }
    }
}
