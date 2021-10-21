using System.Collections;
using UnityEngine;

public class AudioSyncerScale : AudioSyncer
{
	[SerializeField] private Vector3 beatScale;
	[SerializeField] private Vector3 restScale;

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (isBeat) return;

		transform.localScale = Vector3.Lerp(transform.localScale, restScale, RestSmoothTime * Time.deltaTime);
	}

	public override void OnBeat()
	{
		base.OnBeat();

		StopCoroutine("MoveToScale");
		StartCoroutine("MoveToScale", beatScale);
	}

	private IEnumerator MoveToScale(Vector3 _target)
	{
		Vector3 _curr = transform.localScale;
		Vector3 _initial = _curr;
		float _timer = 0;

		while (_curr != _target)
		{
			_curr = Vector3.Lerp(_initial, _target, _timer / TimeToBeat);
			_timer += Time.deltaTime;

			transform.localScale = _curr;

			yield return null;
		}

		isBeat = false;
	}
}