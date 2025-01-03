using Colore;
using Colore.Data;
using Colore.Effects.Keyboard;
using GTA;
using GTA.UI;
using System;

public class Police : Script
{
    private Color latestBlinkColor = Color.Blue;
    private DateTime latestBlinkTime = DateTime.Now;
    private int latestWantedLevel = 0;

    private Key[] oneStar = { Key.F6, Key.F7 };
    private Key[] twoStar = { Key.F5, Key.F6, Key.F7, Key.F8 };
    private Key[] threeStar = { Key.F4, Key.F5, Key.F6, Key.F7, Key.F8, Key.F9 };
    private Key[] fourStar = { Key.F3, Key.F4, Key.F5, Key.F6, Key.F7, Key.F8, Key.F9, Key.F10 };
    private Key[] fiveStar = { Key.F1, Key.F2, Key.F3, Key.F4, Key.F5, Key.F6, Key.F7, Key.F8, Key.F9, Key.F10, Key.F11, Key.F12 };
    private Key[] resetKeys = { Key.F1, Key.F2, Key.F3, Key.F4, Key.F5, Key.F6, Key.F7, Key.F8, Key.F9, Key.F10, Key.F11, Key.F12 };

    public Police()
    {
        Tick += OnTick;
    }

    private void OnTick(object sender, EventArgs e)
    {
        int currentWantedLevel = Game.Player.WantedLevel;

        if (latestWantedLevel != currentWantedLevel)
        {
            if(currentWantedLevel == 0)
            {
                PoliceLostPlayerEffect();
            }
            else
            {
                Start.ChangeColorToCharacter();
            }
        }

        latestWantedLevel = currentWantedLevel;

        if (currentWantedLevel > 0 && Game.Player.IsAlive)
        {
            if ((DateTime.Now - latestBlinkTime).TotalSeconds >= 0.5)
            {
                Key[] keysToBlink = null;

                switch (currentWantedLevel)
                {
                    case 1:
                        keysToBlink = oneStar;
                        break;
                    case 2:
                        keysToBlink = twoStar;
                        break;
                    case 3:
                        keysToBlink = threeStar;
                        break;
                    case 4:
                        keysToBlink = fourStar;
                        break;
                    case 5:
                        keysToBlink = fiveStar;
                        break;
                }

                if (keysToBlink != null)
                {
                    ApplyWantedLevelEffect(keysToBlink);
                    latestBlinkColor = latestBlinkColor == Color.Blue ? Color.Red : Color.Blue;
                }

                latestBlinkTime = DateTime.Now;
            }
        }
    }

    private void ApplyWantedLevelEffect(Key[] keys)
    {
        var customEffect = Start.keyboardCharacterColor;
        int halfLength = keys.Length / 2;
        Color firstGroupColor = latestBlinkColor;
        Color secondGroupColor = latestBlinkColor == Color.Blue ? Color.Red : Color.Blue;

        for (int i = 0; i < keys.Length; i++)
        {
            Color keyColor = i < halfLength ? firstGroupColor : secondGroupColor;
            customEffect[keys[i]] = keyColor;
        }

        Start._chroma.Keyboard.SetCustomAsync(customEffect).Wait();
    }

    private void PoliceLostPlayerEffect()
    {
        Color[] colors = { Color.Red, Color.Blue };

        for (int i = 0; i < resetKeys.Length; i++)
        {
            Start._chroma.Keyboard.SetKeyAsync(resetKeys[i], colors[i % 2]);

            Wait(25);
        }
        Wait(1000);
        for (int i = 0; i < resetKeys.Length; i++)
        {
            Start._chroma.Keyboard.SetKeyAsync(resetKeys[i], Start.GetCurrentCharacterColor());

            Wait(25);
        }
    }
}