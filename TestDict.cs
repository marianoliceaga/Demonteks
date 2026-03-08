using Godot;

public partial class TestDict : SceneTree
{
    public override void _Initialize()
    {
        var dict1 = new Godot.Collections.Dictionary();
        var dict2 = new Godot.Collections.Dictionary<string, Variant>(dict1);
        GD.Print(dict2);
        Quit();
    }
}
