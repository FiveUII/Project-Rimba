using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using ProjectRimba.Player;

namespace ProjectRimba.UI
{
    [RequireComponent(typeof(Image))]
    public class CursorReticle : MonoBehaviour
    {
        [SerializeField] private OrangutanController playerController;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color grabbableColor = new Color(0.3f, 1f, 0.45f, 1f);
        [SerializeField] private Color grabbingColor = new Color(1f, 0.85f, 0.2f, 1f);

        private Image _image;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();
            _image.raycastTarget = false;
        }

        private void Update()
        {
            if (Mouse.current != null)
            {
                _rectTransform.position = Mouse.current.position.ReadValue();
            }

            if (playerController == null) return;

            if (playerController.IsGrappling)
            {
                _image.color = grabbingColor;
            }
            else
            {
                _image.color = playerController.IsNearGrabbable ? grabbableColor : normalColor;
            }
        }
    }
}
