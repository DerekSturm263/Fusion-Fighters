﻿using Photon.Deterministic;

namespace Quantum {
  partial class RuntimeConfig {
        public AssetRefMap StageSourceMap;

        public AssetRefEntityPrototype Hurtbox;
        public AssetRefEntityPrototype Hitbox;
        
        public LayerMask HitboxLayer;
        public LayerMask GroundLayer;
        public int ItemLayer;

        public ArrayColors TeamColors;
        public ArrayColors DarkTeamColors;

        public AssetRefPlayerState DefaultState;

        partial void SerializeUserData(BitStream stream)
        {
            stream.Serialize(ref StageSourceMap);
            stream.Serialize(ref Hurtbox);
            stream.Serialize(ref Hitbox);
            stream.Serialize(ref HitboxLayer);
            stream.Serialize(ref GroundLayer);
            stream.Serialize(ref ItemLayer);
            stream.Serialize(ref TeamColors);
            stream.Serialize(ref DarkTeamColors);
            stream.Serialize(ref DefaultState);
        }
    }
}