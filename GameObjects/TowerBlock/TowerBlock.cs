using System;
using System.Linq;
using Godot;
using WobblyTower.Constants;
using WobblyTower.Helpers;

public class TowerBlock : RigidBody
{
	private MeshInstance _blockMesh;
	private Main _mainScene;
	private Material _blockMaterial;
	private ResourcePreloader _resourcePreloader;
	private Resource _holdSymbol;
	private bool _canDrag = false;
	private bool _isDragging = false;
	private Camera _camera;
	private Plane _dragPlane;

	public override void _Ready()
	{
		Rotation = Vector3.Zero;
		_mainScene = NodeExtractionHelper.GetParent<Main>(this);
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
		ProcessDragging();

		if (_isDragging)
		{
			ProcessRotation();
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

	private void ProcessRotation()
	{
		if (Input.IsActionJustPressed(InputKeys.BlockRotateUp))
		{
			Rotation = new Vector3(Rotation.x, Rotation.y, 0);
			RotateX(90);
		}
		else if (Input.IsActionJustPressed(InputKeys.BlockRotateDown))
		{
			Rotation = new Vector3(Rotation.x, Rotation.y, 0);
			RotateX(-90);
		}
		else if (Input.IsActionJustPressed(InputKeys.BlockRotateLeft))
		{
			RotateY(90);
		}
		else if (Input.IsActionJustPressed(InputKeys.BlockRotateRight))
		{
			RotateY(-90);
		}
	}

	private void ProcessDragging()
	{
		if ((_canDrag || _isDragging) 
		&& Input.IsActionPressed(InputKeys.Interact)
		&& (_mainScene.SelectedTowerBlock == null || _mainScene.SelectedTowerBlock == this))
		{
			if (!_isDragging)
			{
				_mainScene.SelectedTowerBlock = this;
				var cameraForward = _camera.GlobalTransform.basis.z.Normalized();
				_isDragging = true;
				_dragPlane = new Plane(cameraForward, GlobalTransform.origin.y);
				Mode = ModeEnum.Kinematic;
			}

			var mousePos = GetViewport().GetMousePosition();
			var rayOrigin = _camera.ProjectRayOrigin(mousePos);
			var rayDirection = _camera.ProjectRayNormal(mousePos);
			var intersection = _dragPlane.IntersectRay(rayOrigin, rayDirection);

			if (intersection != null)
			{
				GlobalTranslation = intersection.Value;
			}
		}
		else if (_isDragging)
		{
			_mainScene.SelectedTowerBlock = null;
			_isDragging = false;
			Mode = ModeEnum.Rigid;
			ApplyCentralImpulse(Vector3.Down);
		}
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
