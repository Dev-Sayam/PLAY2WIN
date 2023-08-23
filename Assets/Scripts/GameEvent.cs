using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Event", fileName = "New Game Event")]
public class GameEvent : ScriptableObject
{
	HashSet<GameEventListener> listeners = new HashSet<GameEventListener>();

	public void Invoke()
	{
		foreach (GameEventListener listener in listeners) 
		{
			listener.RaiseEvent();
		}
	}

	public void Register(GameEventListener gameEventListener)
	{
		listeners.Add(gameEventListener);
	}

	public void DeRegister(GameEventListener gameEventListener)
	{
		listeners.Remove(gameEventListener);
	}
}
