using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CodeStage.AdvancedFPSCounter.Labels
{
	internal class DrawableLabel
	{
		// ----------------------------------------------------------------------------
		// internal fields
		// ----------------------------------------------------------------------------

		internal GameObject container;
		internal LabelAnchor anchor;
		internal Text uiText;
		internal RectTransform uiTextTransform;
		internal StringBuilder newText;
		internal bool dirty;

		// ----------------------------------------------------------------------------
		// private methods
		// ----------------------------------------------------------------------------

		private GameObject labelGameObject;

		private Font font;
		private int fontSize;
		private float lineSpacing;
		private Vector2 pixelOffset;

		private readonly LabelEffect shadow;
		private Shadow shadowComponent;

		private readonly LabelEffect outline;
		private Outline outlineComponent;

		// ----------------------------------------------------------------------------
		// constructor
		// ----------------------------------------------------------------------------

		internal DrawableLabel(GameObject container, LabelAnchor anchor, LabelEffect shadow, LabelEffect outline, Font font, int fontSize, float lineSpacing, Vector2 pixelOffset)
		{
			this.container = container;
			this.anchor = anchor;

			this.shadow = shadow;
			this.outline = outline;
			this.font = font;
			this.fontSize = fontSize;
			this.lineSpacing = lineSpacing;
			this.pixelOffset = pixelOffset;

			NormalizeOffset();

			newText = new StringBuilder(1000);
		}

		// ----------------------------------------------------------------------------
		// internal methods
		// ----------------------------------------------------------------------------

		internal void CheckAndUpdate()
		{
			if (newText.Length > 0)
			{
				if (uiText == null)
				{
					/* create label game object and configure it */
					labelGameObject = new GameObject(anchor.ToString());

					labelGameObject.layer = container.layer;
					labelGameObject.tag = container.tag;
					labelGameObject.transform.SetParent(container.transform, false);

					/* create UI Text component and apply settings */
					uiText = labelGameObject.AddComponent<Text>();
					uiTextTransform = uiText.rectTransform;

					uiText.horizontalOverflow = HorizontalWrapMode.Overflow;
					uiText.verticalOverflow = VerticalWrapMode.Overflow;

					ApplyShadow();
					ApplyOutline();
					ApplyFont();
                    uiText.fontSize = fontSize;
					uiText.lineSpacing = lineSpacing;

					UpdateTextPosition();
				}

				if (dirty)
				{
					uiText.text = newText.ToString();
					dirty = false;
				}
				newText.Length = 0;
			}
			else if (uiText != null)
			{
				Object.DestroyImmediate(uiText.gameObject);
			}
		}

		internal void Clear()
		{
			newText.Length = 0;
			if (uiText != null)
			{
				Object.Destroy(uiText.gameObject);
				uiTextTransform = null;
				uiText = null;
			}
		}

		internal void Dispose()
		{
			Clear();
			newText = null;
		}

		internal void ChangeFont(Font labelsFont)
		{
			font = labelsFont;
			ApplyFont();
		}

		internal void ChangeFontSize(int newSize)
		{
			fontSize = newSize;
			if (uiText != null) uiText.fontSize = fontSize;
		}

		internal void ChangeOffset(Vector2 newPixelOffset)
		{
			pixelOffset = newPixelOffset;
			NormalizeOffset();

			if (uiText != null)
			{
				uiTextTransform.anchoredPosition = pixelOffset;
			}
		}

		internal void ChangeLineSpacing(float newValueLineSpacing)
		{
			lineSpacing = newValueLineSpacing;

			if (uiText != null)
			{
				uiText.lineSpacing = newValueLineSpacing;
			}
		}

		internal void ChangeShadow(bool enabled)
		{
			shadow.enabled = enabled;
			ApplyShadow();
		}

		internal void ChangeShadowColor(Color color)
		{
			shadow.color = color;
			ApplyShadow();
		}

		internal void ChangeShadowDistance(Vector2 distance)
		{
			shadow.distance = distance;
			ApplyShadow();
		}

		internal void ChangeOutline(bool enabled)
		{
			outline.enabled = enabled;
			ApplyOutline();
		}

		internal void ChangeOutlineColor(Color color)
		{
			outline.color = color;
			ApplyOutline();
		}

		internal void ChangeOutlineDistance(Vector2 distance)
		{
			outline.distance = distance;
			ApplyOutline();
		}

		// ----------------------------------------------------------------------------
		// private methods
		// ----------------------------------------------------------------------------

		private void UpdateTextPosition()
		{
            uiTextTransform.localRotation = Quaternion.identity;
            uiTextTransform.sizeDelta = Vector2.zero;
			uiTextTransform.anchoredPosition3D = pixelOffset;

			switch (anchor)
			{
				case LabelAnchor.UpperLeft:
					uiText.alignment = TextAnchor.UpperLeft;
					uiTextTransform.anchorMin = Vector2.up;
					uiTextTransform.anchorMax = Vector2.up;
					break;
				case LabelAnchor.UpperRight:
					uiText.alignment = TextAnchor.UpperRight;
					uiTextTransform.anchorMin = Vector2.one;
					uiTextTransform.anchorMax = Vector2.one;
					break;
				case LabelAnchor.LowerLeft:
					uiText.alignment = TextAnchor.LowerLeft;
					uiTextTransform.anchorMin = Vector2.zero;
					uiTextTransform.anchorMax = Vector2.zero;
					break;
				case LabelAnchor.LowerRight:
					uiText.alignment = TextAnchor.LowerRight;
					uiTextTransform.anchorMin = Vector2.right;
					uiTextTransform.anchorMax = Vector2.right;
					break;
				case LabelAnchor.UpperCenter:
					uiText.alignment = TextAnchor.UpperCenter;
					uiTextTransform.anchorMin = new Vector2(0.5f, 1f);
					uiTextTransform.anchorMax = new Vector2(0.5f, 1f);
					break;
				case LabelAnchor.LowerCenter:
					uiText.alignment = TextAnchor.LowerCenter;
					uiTextTransform.anchorMin = new Vector2(0.5f, 0f);
					uiTextTransform.anchorMax = new Vector2(0.5f, 0f);
					break;
				default:
					Debug.LogWarning(AFPSCounter.LOG_PREFIX + "Unknown label anchor!", uiText);
					uiText.alignment = TextAnchor.UpperLeft;
					uiTextTransform.anchorMin = Vector2.up;
					uiTextTransform.anchorMax = Vector2.up;
					break;
			}
		}

		private void NormalizeOffset()
		{
			switch (anchor)
			{
				case LabelAnchor.UpperLeft:
					pixelOffset.y = -pixelOffset.y;
					break;
				case LabelAnchor.UpperRight:
					pixelOffset.x = -pixelOffset.x;
					pixelOffset.y = -pixelOffset.y;
					break;
				case LabelAnchor.LowerLeft:
					// it's fine
					break;
				case LabelAnchor.LowerRight:
					pixelOffset.x = -pixelOffset.x;
					break;
				case LabelAnchor.UpperCenter:
					pixelOffset.y = -pixelOffset.y;
					pixelOffset.x = 0;
					break;
				case LabelAnchor.LowerCenter:
					pixelOffset.x = 0;
					break;
				default:
					pixelOffset.y = -pixelOffset.y;
					break;
			}
		}

		private void ApplyShadow()
		{
			if (uiText == null) return;

			if (shadow.enabled && !shadowComponent)
			{
				shadowComponent = labelGameObject.AddComponent<Shadow>();
			}

			if (!shadow.enabled && shadowComponent)
			{
				Object.Destroy(shadowComponent);
			}

			if (shadowComponent != null)
			{
				shadowComponent.effectColor = shadow.color;
				shadowComponent.effectDistance = shadow.distance;
			}
		}

		private void ApplyOutline()
		{
			if (uiText == null) return;

			if (outline.enabled && !outlineComponent)
			{
				outlineComponent = labelGameObject.AddComponent<Outline>();
			}

			if (!outline.enabled && outlineComponent)
			{
				Object.Destroy(outlineComponent);
			}

			if (outlineComponent != null)
			{
				outlineComponent.effectColor = outline.color;
				outlineComponent.effectDistance = outline.distance;
			}
		}


		private void ApplyFont()
		{
			if (uiText == null) return;

			if (font == null) font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			uiText.font = font;
		}
	}
}