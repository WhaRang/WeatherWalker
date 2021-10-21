using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AudioSyncColor : AudioSyncer
{
	[SerializeField] private Color[] beatColors;
	[SerializeField] private Color restColor;

	private int randomIndex;
	private Image image;

	private void Start()
	{
		image = GetComponent<Image>();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (isBeat) return;

		image.color = Color.Lerp(image.color, restColor, RestSmoothTime * Time.deltaTime);
	}

	public override void OnBeat()
	{
		base.OnBeat();

		Color _c = RandomColor();

		StopCoroutine("MoveToColor");
		StartCoroutine("MoveToColor", _c);
	}

	private IEnumerator MoveToColor(Color _target)
	{
		Color _curr = image.color;
		Color _initial = _curr;
		float _timer = 0;

		while (_curr != _target)
		{
			_curr = Color.Lerp(_initial, _target, _timer / TimeToBeat);
			_timer += Time.deltaTime;

			image.color = _curr;

			yield return null;
		}

		isBeat = false;
	}

	private Color RandomColor()
	{
		if (beatColors == null || beatColors.Length == 0) return Color.white;
		randomIndex = Random.Range(0, beatColors.Length);
		return beatColors[randomIndex];
	}
}
