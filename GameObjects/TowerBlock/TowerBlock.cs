using System;
using System.Linq;
using Godot;
using WobblyTower.Helpers;

public class TowerBlock : RigidBody
{
	private MeshInstance _blockMesh;
	private Material _blockMaterial;
	private ResourcePreloader _resourcePreloader;
	private Resource _holdSymbol;

	public override void _Ready()
	{
		_resourcePreloader = NodeExtractionHelper.GetChild<MainResourcePreloader>(GetParent());
		_holdSymbol = GD.Load<Resource>("res://Assets/Symbols/hold.svg");
		var block = GetRandomBlock();

		_blockMaterial = GetRandomMaterial();
		_blockMesh = NodeExtractionHelper.GetChild<MeshInstance>(block);
		_blockMesh.MaterialOverride = _blockMaterial;
		var shape = _blockMesh.Mesh.CreateConvexShape();
		var collision = new CollisionShape { Shape = shape };

		// Remove default collider
		RemoveChild(GetChild(0));
		AddChild(_blockMesh.Duplicate());
		AddChild(collision);
	}

	private void OnRigidBodyMouseEntered()
	{
		GD.Print("Mouse Entered");
		Input.SetCustomMouseCursor(_holdSymbol);
	}


	private void OnRigidBodyMouseExited()
	{
		Input.SetCustomMouseCursor(null);
	}


	private Node GetRandomBlock()
	{
		var blockShapes = _resourcePreloader.GetResourceList()
			.Where(r => r.Contains("tower_block"))
			.ToArray();

		var random = new Random();
		var selectedBlock = random.Next(0, blockShapes.Length);
		var blockScene = (PackedScene)GD.Load(blockShapes[selectedBlock]);

		return blockScene.Instance();
	}

	private Material GetRandomMaterial()
	{
		var materialFiles = _resourcePreloader.GetResourceList()
			.Where(r => r.Contains("block_material"))
			.ToArray();

		var random = new Random();
		var selectedBlock = random.Next(0, materialFiles.Length);

		return (Material)GD.Load(materialFiles[selectedBlock]);
	}
}
