using UnityEngine;

internal class LabelEffect
{
	public bool enabled;
	public Color color;
	public Vector2 distance;

	internal LabelEffect(bool enabled, Color color, Vector2 distance)
	{
		this.enabled = enabled;
		this.color = color;
		this.distance = distance;
	}
}