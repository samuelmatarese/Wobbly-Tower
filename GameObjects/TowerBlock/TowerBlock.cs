using System;
using System.IO;
using System.Linq;
using Godot;
using WobblyTower.Helpers;

public class TowerBlock : RigidBody
{
	private MeshInstance _blockMesh;
	private Material _blockMaterial;
	private ResourcePreloader _resourcePreloader = new ResourcePreloader();

	public override void _Ready()
	{
		var block = GetRandomBlock();

		_blockMaterial = GetRandomMaterial();
		_blockMesh = NodeExtractionHelper.GetChild<MeshInstance>(block);
		_blockMesh.MaterialOverride = _blockMaterial;

		AddChild(block);
	}

	private Node GetRandomBlock()
	{
		var blockShapes =_resourcePreloader.GetResourceList()
			.Where(r => r.StartsWith("tower_block"))
			.ToArray();

		GD.Print(blockShapes.Length);
		var random = new Random();
		var selectedBlock = random.Next(0, blockShapes.Length);
		var blockScene = (PackedScene)GD.Load(blockShapes[selectedBlock]);

		return blockScene.Instance();
	}

	private Material GetRandomMaterial()
	{
		var materialFiles = _resourcePreloader.GetResourceList()
			.Where(r => r.StartsWith("block_material"))
			.ToArray();

		var random = new Random();
		var selectedBlock = random.Next(0, materialFiles.Length);

		return (Material)GD.Load(materialFiles[selectedBlock]);
	}
}
