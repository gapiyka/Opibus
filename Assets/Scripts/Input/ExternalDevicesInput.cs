
namespace Opibus.Input
{
    public class ExternalDevicesInput : IEntityInputSource
    {
        public float HorizontalDirection =>
            UnityEngine.Input.GetAxisRaw("Horizontal");

        public float VerticalDirection =>
            UnityEngine.Input.GetAxisRaw("Vertical");
    }
}