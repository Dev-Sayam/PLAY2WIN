using UnityEngine;

public class DeactivateAfterSomeTime : MonoBehaviour
{
	[SerializeField] float timeDelay = 1.0f;

	// Start is called before the first frame update
	void OnEnable()
	{
		Invoke("DeactivateGameObject", timeDelay);
	}

	void DeactivateGameObject()
	{
		gameObject.SetActive(false);
	}
}
