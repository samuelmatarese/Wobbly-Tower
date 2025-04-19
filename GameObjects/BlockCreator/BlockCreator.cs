using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class BlockCreator : StaticBody
{
	private const string CreationCollectionName = "CreationPositions";

	private Position3D[] _creationPositions;
	private PackedScene _towerBlockScene;

	public override void _Ready()
	{
		_towerBlockScene = GD.Load<PackedScene>("res://GameObjects/TowerBlock/TowerBlock.tscn");
		_creationPositions = GetChildren().Cast<Node>()
			.First(n => n.Name == CreationCollectionName)
			.GetChildren()
			.OfType<Position3D>()
			.ToArray();
		
		CreateBlocks();
	}

	private void CreateBlocks()
	{
		foreach (var position in _creationPositions)
		{
			var block = _towerBlockScene.Instance<TowerBlock>();
			block.Translation = position.Position;
			block.Position = position.Position;
			AddChild(block);
		}
	}
}
