// ============================================================================
// ИГРОВОЙ ОБЪЕКТ
// ============================================================================

using System.Collections.Generic;

namespace GameEngine.Core
{
    // ========================================================================
    // ИГРОВОЙ ОБЪЕКТ
    // ========================================================================
    public class GameObject
    {
        public string Name { get; set; }
        public Transform Transform { get; set; }
        public bool IsActive { get; set; } = true;
        
        private List<Component> _components = new List<Component>();
        private GameObject? _parent;
        private List<GameObject> _children = new List<GameObject>();

        public GameObject(string name = "GameObject")
        {
            Name = name;
            Transform = new Transform();
        }

        public T AddComponent<T>() where T : Component, new()
        {
            var component = new T { GameObject = this };
            _components.Add(component);
            component.OnAttach();
            return component;
        }

        public T? GetComponent<T>() where T : Component
        {
            return _components.Find(c => c is T) as T;
        }

        public void Update(float deltaTime)
        {
            if (!IsActive) return;
            
            foreach (var component in _components)
                if (component.Enabled) component.Update(deltaTime);
            
            foreach (var child in _children)
                child.Update(deltaTime);
        }

        public void AddChild(GameObject child)
        {
            child._parent = this;
            _children.Add(child);
        }
    }
}

