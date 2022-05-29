using static FuckSharp;
using static ConsoleApp.Color;

Menu menu = new Menu();
menu.Items.Add(new MenuItem("Play", () => menu.Hide()));
menu.Show();
Thread.Sleep(int.MaxValue);