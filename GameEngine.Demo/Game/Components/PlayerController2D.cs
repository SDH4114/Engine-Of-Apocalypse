using GameEngine.Core;
using System.Numerics;

namespace GameEngine.Demo.Game.Components;

public sealed class PlayerController2D : Component
{
    public float MoveSpeed { get; set; } = 6f;
    public float JumpImpulse { get; set; } = 7f;
    public float GroundCheckDistance { get; set; } = 0.12f;

    private RigidBody? _body;
    private BoxCollider? _box;

    public override void OnAttach()
    {
        _body = GameObject?.GetComponent<RigidBody>();
        _box = GameObject?.GetComponent<BoxCollider>();
    }

    public override void Update(float deltaTime)
    {
        if (GameObject == null || _body == null)
        {
            return;
        }

        var input = Engine.Instance.Input;
        if (input == null)
        {
            return;
        }

        var move = 0f;
        if (input.IsKeyDown(KeyCode.A) || input.IsKeyDown(KeyCode.Left)) move -= 1f;
        if (input.IsKeyDown(KeyCode.D) || input.IsKeyDown(KeyCode.Right)) move += 1f;

        var v = _body.Velocity;
        v.X = move * MoveSpeed;
        v.Z = 0f;
        _body.Velocity = v;

        // Keep player in 2D plane
        var pos = GameObject.Transform.Position;
        pos.Z = 0f;
        GameObject.Transform.Position = pos;

        if (input.IsKeyPressed(KeyCode.Space) && IsGrounded())
        {
            _body.AddImpulse(Vector3.UnitY * JumpImpulse);
        }
    }

    private bool IsGrounded()
    {
        if (GameObject == null)
        {
            return false;
        }

        var physics = Engine.Instance.Physics;
        if (physics == null)
        {
            return false;
        }

        var halfHeight = 0.5f;
        if (_box != null)
        {
            halfHeight = (_box.Size.Y * GameObject.Transform.Scale.Y) * 0.5f;
        }

        var origin = GameObject.Transform.Position + new Vector3(0f, -halfHeight + 0.01f, 0f);
        var dir = -Vector3.UnitY;

        return physics.Raycast(origin, dir, GroundCheckDistance, out _);
    }
}
