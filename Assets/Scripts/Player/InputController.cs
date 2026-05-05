using System;

public sealed class InputController : ManagerBase<InputController>
{
	public static event Action StartPerformed;
	public static event Action JumpPerformed;
	public static event Action RestartPerformed;
	public static event Action FlipPerformed;

	public void OnStart() => StartPerformed?.Invoke();
	public void OnRestart() => RestartPerformed?.Invoke();
	public void OnJump() => JumpPerformed?.Invoke();
	public void OnFlip() => FlipPerformed?.Invoke();
}