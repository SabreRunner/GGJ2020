
namespace PushForward.EventSystem
{
	using Base;
	using UnityEngine;
	using UnityEngine.Events;

	public class GameEventBoolListener : GameEventListenerBase
    {
		/// <summary>This listener's event is an event with a number.</summary>
		[SerializeField] private GameEventBool gameEventBool;
		protected override GameEvent GameEvent => this.gameEventBool;
		/// <summary>This listener's event gets an integer.</summary>
		[SerializeField] private BoolEvent eventResponse;
		[SerializeField] private UnityEvent trueResponse;
		[SerializeField] private UnityEvent falseResponse;

		protected override void OnEventRaised()
		{
			this.eventResponse?.Invoke(this.gameEventBool.condition);

			if (this.gameEventBool.condition)
			{ this.trueResponse?.Invoke(); }
			else { this.falseResponse?.Invoke(); }
		}
	}
}