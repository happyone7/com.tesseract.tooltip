using Tesseract.Core;
using UnityEngine;

namespace Tesseract.Tooltip
{
    /// <summary>
    /// Manages tooltip display with mouse-follow and slot-based positioning.
    /// </summary>
    public class TooltipManager : Singleton<TooltipManager>
    {
        [SerializeField] private TooltipView _tooltip;
        [SerializeField] private Vector2 _offset = new Vector2(15f, -15f);

        private bool _isFollowingMouse;

        private void Update()
        {
            if (_isFollowingMouse && _tooltip != null)
            {
                Vector2 mousePosition = (Vector2)Input.mousePosition + _offset;
                mousePosition = ClampToScreen(mousePosition);
                _tooltip.transform.position = mousePosition;
            }
        }

        /// <summary>
        /// Show tooltip at mouse position (follows mouse).
        /// </summary>
        public void ShowAtMouse(string text)
        {
            if (_tooltip == null) return;
            _tooltip.SetText(text);
            _tooltip.Show();
            _isFollowingMouse = true;
        }

        /// <summary>
        /// Show tooltip at a fixed world position.
        /// </summary>
        public void ShowAtPosition(string text, Vector2 screenPosition)
        {
            if (_tooltip == null) return;
            _tooltip.SetText(text);
            _tooltip.Show();
            _tooltip.transform.position = ClampToScreen(screenPosition);
            _isFollowingMouse = false;
        }

        /// <summary>
        /// Show tooltip anchored to a RectTransform (e.g. a UI slot).
        /// Automatically positions left or right based on screen center.
        /// </summary>
        public void ShowAtSlot(string text, RectTransform slotTransform)
        {
            if (_tooltip == null || slotTransform == null) return;
            _tooltip.SetText(text);
            _tooltip.Show();
            _isFollowingMouse = false;

            Vector2 slotScreenPos = RectTransformUtility.WorldToScreenPoint(null, slotTransform.position);
            float screenCenter = Screen.width * 0.5f;

            RectTransform tooltipRect = _tooltip.RectTransform;
            float tooltipHalfWidth = tooltipRect.rect.width * 0.5f;
            float slotHalfWidth = slotTransform.rect.width * 0.5f;
            float totalOffset = tooltipHalfWidth + slotHalfWidth;

            // If slot is on right side of screen, show tooltip on left (and vice versa)
            float xOffset = slotScreenPos.x >= screenCenter ? -totalOffset : totalOffset;

            Vector2 finalPos = new Vector2(
                slotScreenPos.x + xOffset,
                Screen.height * 0.5f
            );

            _tooltip.transform.position = ClampToScreen(finalPos);
        }

        /// <summary>
        /// Hide the tooltip.
        /// </summary>
        public void Hide()
        {
            if (_tooltip != null)
                _tooltip.Hide();
            _isFollowingMouse = false;
        }

        private Vector2 ClampToScreen(Vector2 position)
        {
            if (_tooltip == null) return position;

            RectTransform rect = _tooltip.RectTransform;
            float halfWidth = rect.rect.width * 0.5f;
            float halfHeight = rect.rect.height * 0.5f;

            position.x = Mathf.Clamp(position.x, halfWidth, Screen.width - halfWidth);
            position.y = Mathf.Clamp(position.y, halfHeight, Screen.height - halfHeight);

            return position;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
                Hide();
        }
    }
}
