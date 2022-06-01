using static ConsoleHandler;
using static ConsoleApp;
using static FuckSharp;
using System.Media;

public class Menu : IDisposable
{
    public Menu() {}
    public ItemCollection Items { get; set; } = new ItemCollection();
    public (int X, int Y) startPoint = (0, 0);
    private Color textColor = Color.Gray;
    public Color TextColor
    {
        get => textColor;
        set
        {
            textColor = value;
            Reload();
        }
    }
    private HorAlign hAlign = HorAlign.Center;
    public HorAlign HAlign
    {
        get => hAlign;
        set
        {
            hAlign = value;
            Reload();
        }
    }
    private VerAlign vAlign = VerAlign.Center;
    public VerAlign VAlign
    {
        get => vAlign;
        set
        {
            vAlign = value;
            Reload();
        }
    }
    public void Reload()
    {
        int cx = Console.WindowWidth / 2;
        int cy = Console.WindowHeight / 2;
        int w = Console.WindowWidth;
        int h = Console.WindowHeight;
        for(int i = 0; i < Items.Count; i++)
        {
            switch (hAlign)
            {
                case HorAlign.Left:
                    Items[i].X = startPoint.X + Items[i].BaseX;
                    break;
                case HorAlign.Center:
                    Items[i].X = cx - Items[i].Line.Length / 2;
                    break;
                case HorAlign.Right:
                    Items[i].X = w - startPoint.X - Items[i].BaseX;
                    break;
            }
            switch (vAlign)
            {
                case VerAlign.Top:
                    Items[i].Y = Items[i].BaseY + startPoint.Y;
                    break;
                case VerAlign.Center:
                    if(i < Items.Count / 2)
                        Items[i].Y = cy - (Items.Count / 2 - Items[i].BaseY);
                    else Items[i].Y = cy + Items[i].BaseY - Items.Count / 2;      
                    break;
                case VerAlign.Bottom:
                    Items[i].Y = h - Items[i].BaseY - startPoint.Y;
                    break;
            }
        }
    }
    public void Render() => Items.ToList().ForEach(z => z.Render(TextColor));
    public void Close()
    {
        MouseUpdate -= Mouse_Update;
        PressKey -= KeyPress_Update;
        Console.Clear();
        Dispose();
    }
    public void Hide()
    {
        MouseUpdate -= Mouse_Update;
        PressKey -= KeyPress_Update;
        Console.Clear();
    }
    public void Show()
    {
        Reload();
        Render();
        MouseUpdate += Mouse_Update;
        PressKey += KeyPress_Update;
        Items.ToList().Where(z => z is InputItem).ToList().ForEach(z =>
        {
            string value;
            while (true)
            {
                CCP = (z.X + z.Line.Length, z.Y);
                CFC = ConsoleColor.White;
                value = Console.ReadLine();
                string? validated = ((InputItem)z).Validate(value);
                if (validated == null)
                {
                    ((InputItem)z).Value = value;
                    CCP = (z.X + z.Line.Length, z.Y + 1);
                    CFC = ConsoleColor.Green;
                    W = value + "                 ";
                    CCP = (0, 0);
                    break;                
                }
                else
                {
                    CCP = (z.X + z.Line.Length, z.Y + 1);
                    CFC = ConsoleColor.Red;
                    W = "     " + validated + Enumerable.Repeat(' ', value.Length + validated.Length + 5).bTS();                     
                }
            }
        });
    }
    private int lastEntered = -1, inputIndex = -1, lastIndexButtonEnter = -1;
    public void KeyPress_Update(bool down, char @char, ushort key, int state)
    {
        return;
        if (inputIndex != -1)
        {
            CCP = (0, 20);
            W = down + "|" + @char + "|" + key + "|" + state + "             ";
            if (!new int[]{ 60, 70, 112, 96 }.Contains(state))
                return;
            InputItem item = Items[inputIndex].AsInput;
            if (!down && key == 8)
            {
                if(item.Value.Length > 0)
                {
                    Items[inputIndex].AsInput.Value = item.Value.Remove(item.Value.Length - 1);
                    CCP = (item.X + item.Value.Length + Items[inputIndex].Line.Length, inputIndex + 1);
                    W = default(char);
                    CCP = (item.X + item.Value.Length + Items[inputIndex].Line.Length, inputIndex + 1);
                }
            }
            else if(((down && state == 112) || !down) && (char.IsLetter(@char) || char.IsDigit(@char) || @char == 32))
            {
                CCP = (item.X + item.Value.Length + Items[inputIndex].Line.Length, inputIndex + 1);
                W = @char;
                Items[inputIndex].AsInput.Value += @char;
            }          
        }
    }
    public void Mouse_Update()
    {
        BaseMenuItem? item = Items.ToList().Find(z => Intersection(z.Y, z.X, z.X + z.Length));
        if(item != null)
        {
            if(Mouse.KeyState)
        }
    }
    private bool Intersection(int y, int xs, int xe) => Mouse.Y == y && Mouse.X >= xs && Mouse.X <= xe;
    private bool Intersection(int xs, int xe) => Mouse.X >= xs && Mouse.X <= xe;

    public void Dispose() { }

    public enum HorAlign
    {
        Left,
        Center,
        Right,
    }
    public enum VerAlign
    {
        Top,
        Center,
        Bottom,
    }
}
public class MenuItem
{
    public static Color PressColor = Color.White;
    public static Color EnterColor = Color.DarkGray;
    public static List<Type> UnUpdaterTypes = new List<Type>() {
        typeof(LabelItem), typeof(InputItem)
    };
    public enum MenuOptions 
    {
        None,
        Hide,
        Close,
    }
}