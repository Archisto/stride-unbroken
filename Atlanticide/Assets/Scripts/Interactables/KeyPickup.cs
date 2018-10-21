﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlanticide
{
    public class KeyPickup : Pickup
    {
        public int keyCode;

        public override void Collect(PlayerCharacter character)
        {
            base.Collect(character);
            World.Instance.keyCodes.AddIfNew(keyCode);
        }
    }
}