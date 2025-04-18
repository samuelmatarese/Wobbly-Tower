using System;
using System.Linq;
using Godot;
using WobblyTower.Constants;
using WobblyTower.Helpers;

public class TowerBlock : RigidBody
{
	private MeshInstance _blockMesh;
	private Material _blockMaterial;
	private ResourcePreloader _resourcePreloader;
	private Resource _holdSymbol;
	private bool _canDrag = false;
	private bool _isDragging = false;
	private Camera _camera;
	private Plane _dragPlane;

	public override void _Ready()
	{
		_camera = GetViewport().GetCamera();
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

	public override void _PhysicsProcess(float delta)
	{
		if ((_canDrag || _isDragging) && Input.IsActionPressed(InputKeys.Interact))
		{
			if (!_isDragging)
			{
				Vector3 cameraForward = _camera.GlobalTransform.basis.z.Normalized();
				_isDragging = true;
				_dragPlane = new Plane(cameraForward, GlobalTransform.origin.y);
				Mode = ModeEnum.Kinematic;
			}

			Vector2 mousePos = GetViewport().GetMousePosition();
			Vector3 rayOrigin = _camera.ProjectRayOrigin(mousePos);
			Vector3 rayDirection = _camera.ProjectRayNormal(mousePos);
			Vector3? intersection = _dragPlane.IntersectRay(rayOrigin, rayDirection);

			if (intersection != null)
			{
				Transform = new Transform(GlobalTransform.basis, intersection.Value);
			}
		}
		else if (_isDragging)
		{
			_isDragging = false;
			Mode = ModeEnum.Rigid;
			GravityScale = 1f;
			LinearVelocity = Vector3.Zero;
		}
	}

	private void OnRigidBodyMouseEntered()
	{
		_canDrag = true;
		Input.SetCustomMouseCursor(_holdSymbol);
	}


	private void OnRigidBodyMouseExited()
	{
		_canDrag = false;
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
