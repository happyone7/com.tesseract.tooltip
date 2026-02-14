# Changelog

## [1.1.0] - 2026-02-14
### Fixed
- TooltipView.Awake: Added null fallback for _canvasGroup and _tooltipRect
- TooltipView.Hide: Added null check for _canvasGroup
- ClampToScreen: Now accounts for pivot position instead of assuming center pivot
- ShowAtSlot: Uses sizeDelta (via GetSize()) for accurate width after SetText
- Added GetSize() method to TooltipView for reliable size queries

## [1.0.0] - 2026-02-14
### Added
- TooltipView: Visual tooltip component with TMP text, auto-sizing, CanvasGroup fade
- TooltipManager: Singleton manager with mouse-follow, fixed position, and slot-based positioning
- TooltipTrigger: Pointer hover component for easy tooltip setup on UI elements
- Screen boundary clamping to prevent tooltip overflow
