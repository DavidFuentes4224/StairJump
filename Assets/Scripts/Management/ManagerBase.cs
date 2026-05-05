using UnityEngine;

public abstract class ManagerBase<T> : MonoBehaviour where T: MonoBehaviour
{
	protected ManagerBase() { }

	public static T Instance => m_instance;

	protected virtual void Awake()
	{
		if (m_instance != null)
		{
			Debug.LogWarning($"Instance for {m_instance.GetType()} already exists.");
			Destroy(this.gameObject);
			return;
		}

		m_instance = this as T;
		DontDestroyOnLoad(this.gameObject);
	}

	static T m_instance;
}