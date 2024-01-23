using Godot;
using System;

public partial class PlayerLook : MeshInstance3D
{
	[Export]
	private CharacterBody3D playerBody;
    [Export]
    private float MouseSensitivity = 0.5f;
    [Export]
    private float MinClampValue = -80f;
    [Export]
    private float MaxClampValue = 120f;

    private Vector2 mouseRotation = Vector2.Zero;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            mouseRotation = mouseMotion.Relative;
        }

        if(Input.IsActionJustPressed("ui_cancel"))
        {
            Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured ?
                Input.MouseModeEnum.Visible :
                Input.MouseModeEnum.Captured;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // Rotate the player's body horizontally based on mouse movement
        var bodyRot = -mouseRotation.X * MouseSensitivity * (float) delta;
        playerBody.RotateY(bodyRot);

        // Rotate the Eyes (this object) vertically based on mouse movement
        var eyesRot = -mouseRotation.Y * MouseSensitivity * (float)delta;
        RotateX(eyesRot);
        var curEyesRot = Rotation;
        curEyesRot.X = Math.Clamp(curEyesRot.X, Mathf.DegToRad(MinClampValue), Mathf.DegToRad(MaxClampValue));
        Rotation = curEyesRot;

        // Reset the mouse vector
        mouseRotation = Vector2.Zero;
    }
}
