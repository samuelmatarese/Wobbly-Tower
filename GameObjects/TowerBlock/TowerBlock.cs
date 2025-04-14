using Godot;
using WobblyTower.Helpers;

public class TowerBlock : RigidBody
{
	private MeshInstance _blockMesh;
	private Material _blockMaterial;

	public override void _Ready()
	{
		var blockScene = (PackedScene)GD.Load("res://Assets/TowerShapes/tower_block_1.glb");
		var material = GD.Load("res://Assets/TowerShapes/Material.material");
		var block = blockScene.Instance();

		_blockMaterial = (Material)material;
		_blockMesh = NodeExtractionHelper.GetChild<MeshInstance>(block);
		_blockMesh.MaterialOverride = _blockMaterial;

		AddChild(block);
	}
}
