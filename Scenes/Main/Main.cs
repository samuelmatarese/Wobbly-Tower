using Godot;
using WobblyTower.Helpers;

public class Main : Spatial
{
	private PackedScene _towerBlockScene;

	public override void _Ready()
	{
		_towerBlockScene = GD.Load<PackedScene>("res://GameObjects/TowerBlock/TowerBlock.tscn");
		for (int i = 0; i < 10; i++)
		{
			GenerateTowerBlock(new Vector3(i, 6, i));
		}
	}

	private void GenerateTowerBlock(Vector3 position)
	{
		var block = _towerBlockScene.Instance<TowerBlock>();
		block.Translation = position;
		AddChild(block);
	}
}
