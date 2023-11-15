using UnityEngine;
namespace KheloJeeto
{
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
			this.gameObject.transform.localScale = Vector3.zero;
		}
	}
}