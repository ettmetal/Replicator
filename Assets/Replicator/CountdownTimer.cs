using System;
using System.Collections;
using UnityEngine;

namespace Replicator {
	internal class CountdownTimer : MonoBehaviour {
		public float Target { get; set; }
		public bool IsOneShot { get; set; }
		public bool StartOnEnable { get; set; }
		public bool IsRunning => countdownCoroutine != null;
		public event Action Timeout;
		private float elapsedTime = 0f;
		private Coroutine countdownCoroutine;

		private void OnEnable() {
			if(StartOnEnable) StartTimer();
		}

		private IEnumerator tickTimer() {
			if(elapsedTime >= Target) Timeout?.Invoke();
			elapsedTime += Time.unscaledDeltaTime;
			yield return null;
		}

		private void onTimeout() {
			if(!IsOneShot) reset();
			else StopCoroutine(countdownCoroutine);
		}

		public void StartTimer() {
			if(countdownCoroutine == null) {
				countdownCoroutine = StartCoroutine(tickTimer());
				Timeout += onTimeout;
			}
		}

		public void RestartTimer() {
			StopTimer();
			reset();
			StartTimer();
		}

		public void StopTimer() {
			StopCoroutine(countdownCoroutine);
			countdownCoroutine = null;
			Timeout -= onTimeout;
		}

		private void reset() => elapsedTime = 0f;
	}
}
