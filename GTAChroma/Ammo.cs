using Colore;
using Colore.Data;
using Colore.Effects.Keyboard;
using GTA;
using System;

public class Ammo : Script
{
    private Key[] ammoKeys = { Key.Num1, Key.Num2, Key.Num3, Key.Num4, Key.Num5, Key.Num6, Key.Num7, Key.Num8, Key.Num9 };
    private bool unarmedOnce = true;

    public Ammo()
    {
        Tick += OnTick;
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (!Game.Player.IsDead)
        {
            if (Game.Player.Character.Weapons.Current.Hash != WeaponHash.Unarmed)
            {
                unarmedOnce = false;
                int max = Game.Player.Character.Weapons.Current.MaxAmmoInClip;
                int actual = Game.Player.Character.Weapons.Current.AmmoInClip;

                double proportion = (double)actual / max;
                int keysToIlluminate = (int)(proportion * ammoKeys.Length);

                for (int i = 0; i < ammoKeys.Length; i++)
                {
                    if (i < keysToIlluminate)
                    {
                        Start._chroma.Keyboard.SetKeyAsync(ammoKeys[i], Color.White);
                    }
                    else
                    {
                        Start._chroma.Keyboard.SetKeyAsync(ammoKeys[i], Start.GetCurrentCharacterColor());
                    }
                }
            }
            else
            {
                foreach (var key in ammoKeys)
                {
                    Start._chroma.Keyboard.SetKeyAsync(key, Start.GetCurrentCharacterColor());
                }
            }
        }
    }
}