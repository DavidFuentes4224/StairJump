using UnityEngine;

public abstract class ManagerBase<T> : MonoBehaviour where T: MonoBehaviour
{
	protected ManagerBase() { }

	public static T Instance => m_instance;

	protected virtual void Awake()
	{
		if (m_instance != null)
			Destroy(this.gameObject);
		
		m_instance = this as T;
		DontDestroyOnLoad(this.gameObject);
	}

	static T m_instance;
}