using Godot;
using System;
using WobblyTower.Constants;

public class Player : Camera
{
	private const int MaxUpRotation = 50;
	private const int MaxDownRotation = -50;
	private const float MouseSensitivity = 0.3f;
	private const int MaxLeftRotation = -50;
	private const int MaxRightRotation = 50;
	private const int MaxZoomLevel = 5;
	private const int MinZoomLevel = -20;

	private Spatial _rotationPoint = null;
	private int _currentZoomLevel = 0;

	public override void _Ready()
	{
		_rotationPoint = GetParent<Spatial>();
	}

	public override void _Process(float delta)
	{
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton)
		{
			Translate(GetZoom(mouseButton.ButtonIndex).Normalized());
		}

		if (@event is InputEventMouseMotion mouseMotion)
		{
			if (Input.IsActionPressed(InputKeys.Rotate))
				RotationDegrees += ProcessRotation(mouseMotion);

			if (Input.IsActionPressed(InputKeys.Move))
				_rotationPoint.RotationDegrees += ProcessPivotRotation(mouseMotion);
		}
	}

	private Vector3 ProcessRotation(InputEventMouseMotion mouseMotion)
	{
		var rotation = new Vector3();

		if (mouseMotion.Relative.y + RotationDegrees.x < MaxUpRotation &&
			mouseMotion.Relative.y + RotationDegrees.x > MaxDownRotation)
		{
			rotation.x += mouseMotion.Relative.y * MouseSensitivity;
		}

		if (mouseMotion.Relative.x + RotationDegrees.y < MaxRightRotation &&
			mouseMotion.Relative.x + RotationDegrees.y > MaxLeftRotation)
		{
			rotation.y += mouseMotion.Relative.x * MouseSensitivity;
		}

		return rotation;
	}

	private Vector3 ProcessPivotRotation(InputEventMouseMotion mouseMotion)
	{
		var rotation = new Vector3();

		if (mouseMotion.Relative.y - _rotationPoint.RotationDegrees.x < MaxUpRotation &&
			mouseMotion.Relative.y - _rotationPoint.RotationDegrees.x > MaxDownRotation)
		{
			rotation.x += -mouseMotion.Relative.y * MouseSensitivity;
		}

		rotation.y += -mouseMotion.Relative.x * MouseSensitivity;
		return rotation;
	}

	private Vector3 GetZoom(int buttonIndex)
	{
		var zoom = new Vector3();

		if (buttonIndex == (int)ButtonList.WheelUp 
		&& _currentZoomLevel < MaxZoomLevel)
		{
			_currentZoomLevel++;
			zoom = Vector3.Forward;
		}

		else if (buttonIndex == (int)ButtonList.WheelDown 
		&& _currentZoomLevel > MinZoomLevel)
		{
			_currentZoomLevel--;
			zoom = Vector3.Back;
		}

		return zoom;
	}
}
