using Colore;
using Colore.Data;
using Colore.Effects.Keyboard;
using GTA;
using GTA.UI;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Start : Script
{
    private static Model latestPlayerModel = null;
    public static IChroma _chroma = null;
    public static Color Michael = new Color(63, 88, 191);
    public static Color Franklin = new Color(67, 189, 48);
    public static Color Trevor = new Color(208, 88, 14);
    public static Color MichaelSpecial = new Color(0, 0, 255);
    public static Color FranklinSpecial = new Color(0, 255, 0);
    public static Color TrevorSpecial = new Color(254, 57, 0);
    public static CustomKeyboardEffect keyboardCharacterColor = CustomKeyboardEffect.Create();
    public static bool isDeadOnce = false;

    public Start()
    {
        _chroma = ColoreProvider.CreateNativeAsync().Result;
        _chroma.Keyboard.SetCustomAsync(keyboardCharacterColor);
        Tick += OnTick;
        Aborted += OnAborted;
        KeyDown += OnKeyDown;
        ChangeColorToCharacter();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if(e.KeyCode == Keys.NumPad1)
        {
            Game.Player.Character.Health = Game.Player.Character.Health - 1;
        }
        if (e.KeyCode == Keys.NumPad2)
        {
            Game.Player.Character.Health = Game.Player.Character.Health + 1;
        }
    }

    private void OnAborted(object sender, EventArgs e)
    {
        _chroma.Dispose();
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (isDeadOnce && !Game.Player.IsDead)
        {
            isDeadOnce = false;
            ChangeColorToCharacter();
        }
        if (latestPlayerModel != Game.Player.Character.Model)
        {
            ChangeColorToCharacter();
        }

    }

    public static void ChangeColorToCharacter()
    {
        latestPlayerModel = Game.Player.Character.Model;
        keyboardCharacterColor.Set(GetCurrentCharacterColor());
        _chroma.Keyboard.SetCustomAsync(keyboardCharacterColor).Wait();
    }

    public static Color GetCurrentCharacterColor()
    {
        if (latestPlayerModel == new Model(PedHash.Michael))
        {
            return Game.Player.IsSpecialAbilityActive ? MichaelSpecial : Michael;
        }
        if (latestPlayerModel == new Model(PedHash.Franklin))
        {
            return Game.Player.IsSpecialAbilityActive ? FranklinSpecial : Franklin;
        }
        if (latestPlayerModel == new Model(PedHash.Trevor))
        {
            return Game.Player.IsSpecialAbilityActive ? TrevorSpecial : Trevor;
        }
        return Color.Black;
    }

    internal static void ResetCharacterColor()
    {
        if (latestPlayerModel == new Model(PedHash.Michael))
        {
            keyboardCharacterColor.Set(Michael);
        }
        if (latestPlayerModel == new Model(PedHash.Franklin))
        {
            keyboardCharacterColor.Set(Franklin);
        }
        if (latestPlayerModel == new Model(PedHash.Trevor))
        {
            keyboardCharacterColor.Set(Trevor);
        }
    }
}