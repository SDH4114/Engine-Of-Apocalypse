// ============================================================================
// КОМПОНЕНТНАЯ СИСТЕМА
// ============================================================================

namespace GameEngine.Core
{
    // ========================================================================
    // КОМПОНЕНТНАЯ СИСТЕМА
    // ========================================================================
    public abstract class Component
    {
        public GameObject GameObject { get; set; }
        public bool Enabled { get; set; } = true;

        public virtual void OnAttach() { }
        public virtual void Update(float deltaTime) { }
        public virtual void OnDestroy() { }
    }
}