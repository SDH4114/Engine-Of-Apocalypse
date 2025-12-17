// ============================================================================
// INPUT MANAGER
// ============================================================================

using System;
using System.Collections.Generic;
using System.Numerics;

namespace GameEngine.Core
{
    // ========================================================================
    // INPUT MANAGER
    // ========================================================================
    public class InputManager
    {
        private Dictionary<KeyCode, bool> _keys = new Dictionary<KeyCode, bool>();
        private Dictionary<KeyCode, bool> _prevKeys = new Dictionary<KeyCode, bool>();
        private Dictionary<MouseButton, bool> _mouseButtons = new Dictionary<MouseButton, bool>();
        private Dictionary<MouseButton, bool> _prevMouseButtons = new Dictionary<MouseButton, bool>();
        
        private Vector2 _mousePosition;
        private Vector2 _prevMousePosition;
        private float _scrollDelta;
        
        public InputManager()
        {
            Console.WriteLine("✓ Input Manager initialized");
        }

        public void Update()
        {
            // Сохранение предыдущего состояния
            _prevKeys = new Dictionary<KeyCode, bool>(_keys);
            _prevMouseButtons = new Dictionary<MouseButton, bool>(_mouseButtons);
            _prevMousePosition = _mousePosition;
            _scrollDelta = 0;

            // Обновление состояния через события окна (GLFW/Silk.NET)
            // Это будет вызываться из обработчиков событий окна
        }

        // ====================================================================
        // КЛАВИАТУРА
        // ====================================================================
        public bool IsKeyDown(KeyCode key)
        {
            return _keys.TryGetValue(key, out var state) && state;
        }

        public bool IsKeyPressed(KeyCode key)
        {
            var current = _keys.TryGetValue(key, out var state) && state;
            var previous = _prevKeys.TryGetValue(key, out var prevState) && prevState;
            return current && !previous;
        }

        public bool IsKeyReleased(KeyCode key)
        {
            var current = _keys.TryGetValue(key, out var state) && state;
            var previous = _prevKeys.TryGetValue(key, out var prevState) && prevState;
            return !current && previous;
        }

        internal void SetKeyState(KeyCode key, bool pressed)
        {
            _keys[key] = pressed;
        }

        // ====================================================================
        // МЫШЬ
        // ====================================================================
        public Vector2 GetMousePosition() => _mousePosition;

        public Vector2 GetMouseDelta() => _mousePosition - _prevMousePosition;

        public bool IsMouseButtonDown(MouseButton button)
        {
            return _mouseButtons.TryGetValue(button, out var state) && state;
        }

        public bool IsMouseButtonPressed(MouseButton button)
        {
            var current = _mouseButtons.TryGetValue(button, out var state) && state;
            var previous = _prevMouseButtons.TryGetValue(button, out var prevState) && prevState;
            return current && !previous;
        }

        public bool IsMouseButtonReleased(MouseButton button)
        {
            var current = _mouseButtons.TryGetValue(button, out var state) && state;
            var previous = _prevMouseButtons.TryGetValue(button, out var prevState) && prevState;
            return !current && previous;
        }

        internal void SetMousePosition(Vector2 position)
        {
            _mousePosition = position;
        }

        internal void SetMouseButtonState(MouseButton button, bool pressed)
        {
            _mouseButtons[button] = pressed;
        }

        // ====================================================================
        // СКРОЛЛ
        // ====================================================================
        public float GetScrollDelta() => _scrollDelta;

        internal void SetScrollDelta(float delta)
        {
            _scrollDelta = delta;
        }

        // ====================================================================
        // ГЕЙМПАД
        // ====================================================================
        public bool IsGamepadConnected(int index)
        {
            // Проверка подключения геймпада через GLFW/SDL
            return false;
        }

        public float GetGamepadAxis(int index, GamepadAxis axis)
        {
            // Получение значения оси геймпада
            return 0f;
        }

        public bool IsGamepadButtonDown(int index, GamepadButton button)
        {
            // Проверка кнопки геймпада
            return false;
        }

        // ====================================================================
        // ХЕЛПЕРЫ ДЛЯ УПРАВЛЕНИЯ
        // ====================================================================
        public Vector2 GetMovementInput()
        {
            var movement = Vector2.Zero;

            if (IsKeyDown(KeyCode.W)) movement.Y += 1;
            if (IsKeyDown(KeyCode.S)) movement.Y -= 1;
            if (IsKeyDown(KeyCode.A)) movement.X -= 1;
            if (IsKeyDown(KeyCode.D)) movement.X += 1;

            if (movement.LengthSquared() > 0)
                movement = Vector2.Normalize(movement);

            return movement;
        }

        public Vector2 GetLookInput()
        {
            return GetMouseDelta();
        }
    }
}
