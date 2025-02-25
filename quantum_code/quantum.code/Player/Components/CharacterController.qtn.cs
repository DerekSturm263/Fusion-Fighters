﻿using Photon.Deterministic;

namespace Quantum
{
    public unsafe partial struct CharacterController
    {
        public readonly bool IsHeldThisFrame(Input input, Input.Buttons button) => input.InputButtons.HasFlag(button);
        public readonly bool WasPressedThisFrame(Input input, Input.Buttons button) => input.InputButtons.HasFlag(button) && !LastFrame.InputButtons.HasFlag(button);
        public readonly bool WasReleasedThisFrame(Input input, Input.Buttons button) => !input.InputButtons.HasFlag(button) && LastFrame.InputButtons.HasFlag(button);

        public readonly Colliders GetNearbyColliders(Frame f, MovementSettings movementSettings, Transform2D* parent)
        {
            Colliders result = default;

            if (movementSettings.GroundedCast.GetCastResults(f, parent).Count > 0)
                result |= Colliders.Ground;
            if (movementSettings.WallCastLeft.GetCastResults(f, parent).Count > 0)
                result |= Colliders.LeftWall;
            if (movementSettings.WallCastRight.GetCastResults(f, parent).Count > 0)
                result |= Colliders.RightWall;
            if (movementSettings.CeilingCast.GetCastResults(f, parent).Count > 0)
                result |= Colliders.Ceiling;

            return result;
        }

        public readonly bool GetNearbyCollider(Colliders collider) => NearbyColliders.HasFlag(collider);

        public readonly MovementMoveSettings GetMoveSettings(PlayerState state)
        {
            if (GetNearbyCollider(Colliders.Ground))
                return state.Movement.GroundedMovement;
            else
                return state.Movement.AerialMovement;
        }

        public readonly MovementCurveSettings GetJumpSettings(PlayerState state)
        {
            return JumpType switch
            {
                JumpType.ShortHop => state.Jump.ShortJump,
                JumpType.FullHop => state.Jump.FullJump,
                JumpType.Aerial => state.Jump.AerialJump,
                _ => default,
            };
        }

        public readonly MovementCurveSettingsXY GetDodgeSettings(DodgeState state)
        {
            return DodgeType switch
            {
                DodgeType.Spot => state.SpotDodge,
                DodgeType.RollForward => state.ForwardRoll,
                DodgeType.RollBackward => state.BackwardRoll,
                DodgeType.Aerial => state.AerialDodge,
                _ => default,
            };
        }

        public void Move(Frame f, FP amount, ref CharacterControllerSystem.Filter filter, MovementSettings movementSettings, PlayerState state, ApparelStats stats, FP multiplier)
        {
            if (MaintainVelocity)
                return;

            if (!CanMove)
            {
                filter.PhysicsBody->Velocity.X = 0;
                return;
            }

            if (!CanInput || filter.Shakeable->Time > 0)
            {
                filter.PhysicsBody->Velocity.X = filter.CharacterController->CurrentKnockback.Direction.X;
                return;
            }

            amount *= multiplier;

            MovementMoveSettings moveSettings = GetMoveSettings(state);

            if (GetNearbyCollider(Colliders.LeftWall) && amount < 0)
                amount = 0;
            if (GetNearbyCollider(Colliders.RightWall) && amount > 0)
                amount = 0;

            bool isTurning = false;

            // Exit if the player is holding too much up or down.
            if (FPMath.Abs(amount) < movementSettings.DeadStickZone)
            {
                // Set the player to stop moving.
                Velocity = LerpSpeed(moveSettings, f.DeltaTime, 0, Velocity, moveSettings.Deceleration);
                MovingLerp = FPMath.Lerp(MovingLerp, 0, f.DeltaTime * moveSettings.MovingLerpDeceleration);

                CustomAnimator.SetBoolean(f, filter.CustomAnimator, "IsMoving", false);
            }
            else
            {
                // Calculate the player's speed.
                FP topSpeed = CalculateTopSpeed(moveSettings, amount);

                // Apply the target velocity based on their speed.
                if (FPMath.Abs(Velocity) > moveSettings.TurnAroundThreshold && FPMath.SignInt(amount) != FPMath.SignInt(Velocity))
                {
                    Velocity = LerpSpeed(moveSettings, f.DeltaTime, amount, Velocity, moveSettings.TurnAroundSpeed);
                    isTurning = true;
                }
                else if (FPMath.Abs(Velocity) < FPMath.Abs(topSpeed) || FPMath.Abs(Velocity) > FPMath.Abs(topSpeed))
                {
                    Velocity = LerpSpeed(moveSettings, f.DeltaTime, amount, Velocity, moveSettings.Acceleration);
                }

                // Set the player's look direction.
                if (GetNearbyCollider(Colliders.Ground))
                {
                    if (MovementDirection == 1 && filter.PhysicsBody->Velocity.X < 0)
                    {
                        SetDirection(f, -1, filter.Entity, filter.PlayerStats->Index);
                    }
                    else if (MovementDirection == -1 && filter.PhysicsBody->Velocity.X > 0)
                    {
                        SetDirection(f, 1, filter.Entity, filter.PlayerStats->Index);
                    }
                }

                MovingLerp = 1;
                CustomAnimator.SetBoolean(f, filter.CustomAnimator, "IsMoving", true);
            }

            CustomAnimator.SetFixedPoint(f, filter.CustomAnimator, "Speed", FPMath.Abs(Velocity / 10));
            CustomAnimator.SetBoolean(f, filter.CustomAnimator, "IsTurning", isTurning);

            filter.PhysicsBody->Velocity.X = (filter.CharacterController->Velocity * stats.Agility) + filter.CharacterController->CurrentKnockback.Direction.X;
        }

        private readonly FP LerpSpeed(MovementMoveSettings settings, FP deltaTime, FP stickX, FP currentAmount, FP speedMultiplier) => FPMath.Lerp(currentAmount, CalculateTopSpeed(settings, stickX), deltaTime * speedMultiplier);
        private readonly FP CalculateTopSpeed(MovementMoveSettings settings, FP stickX) => stickX * settings.TopSpeed;

        public void SetDirection(Frame f, int direction, EntityRef entity, FighterIndex index)
        {
            MovementDirection = direction;
            f.Events.OnPlayerChangeDirection(entity, index, MovementDirection);
        }

        public readonly T LerpFromAnimationHold<T>(System.Func<T, T, FP, T> lerpFunc, T a, T b) where T : struct => lerpFunc.Invoke(a, b, HoldLevel);
        public readonly T LerpFromAnimationHold_UNSAFE<T>(System.Func<T, T, float, T> lerpFunc, T a, T b) where T : struct => lerpFunc.Invoke(a, b, HoldLevel.AsFloat);
    }
}
