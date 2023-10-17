using UnityEngine;
using UnityEngine.UI;

namespace Opibus.Input
{
    public class UIInput : MonoBehaviour, IEntityInputSource
    {
        [SerializeField] private Joystick joystick;

        public float HorizontalDirection => joystick.Horizontal;
        public float VerticalDirection => joystick.Vertical;
    }
}