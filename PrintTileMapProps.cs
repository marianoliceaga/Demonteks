using Godot;
using System;
public partial class PrintTileMapProps : SceneTree
{
    public override void _Initialize()
    {
        var tml = new TileMapLayer();
        var props = tml.GetPropertyList();
        foreach (var p in props)
        {
            var name = p["name"].AsString();
            if (name.Contains("collision") || name.Contains("physics"))
            {
                GD.Print(name);
            }
        }
        Quit();
    }
}
