using Colore;
using Colore.Data;
using Colore.Effects.Keyboard;
using GTA;
using GTA.Native;
using GTA.UI;
using System;

public class HealthBar : Script
{

    private Key[] healthKeys = { Key.D1, Key.D2, Key.D3, Key.D4, Key.D5 };
    private Key[] armorKeys = { Key.D6, Key.D7, Key.D8, Key.D9, Key.D0 };
    private bool abilityOnce = false;

    public HealthBar()
    {
        Tick += OnTick;
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (!Game.Player.IsDead)
        {

            for (int i = 0; i < healthKeys.Length; i++)
            {
                int threshold = 100 + (i * (100 / healthKeys.Length));
                Color keyColor = Game.Player.Character.Health > threshold ? Color.Green : Color.Red;
                Start._chroma.Keyboard.SetKeyAsync(healthKeys[i], keyColor);
            }
            for (int i = 0; i < armorKeys.Length; i++)
            {
                int threshold = i * (100 / armorKeys.Length);
                Color keyColor = Game.Player.Character.Armor > threshold ? Color.Blue : Color.Black;
                Start._chroma.Keyboard.SetKeyAsync(armorKeys[i], keyColor);
            }
            if (Game.Player.IsSpecialAbilityActive)
            {
                if (!abilityOnce)
                {
                    Start.keyboardCharacterColor.Set(Start.GetCurrentCharacterColor());
                }
                abilityOnce = true;
            }
            else
            {
                if (abilityOnce)
                {
                    Start.ResetCharacterColor();
                }
                abilityOnce = false;
            }
        }
        else
        {
            Start.isDeadOnce = true;
            CustomKeyboardEffect deadEffect = CustomKeyboardEffect.Create();
            deadEffect.Set(Color.Red);
            Start._chroma.Keyboard.SetCustomAsync(deadEffect).Wait();
        }
    }
}