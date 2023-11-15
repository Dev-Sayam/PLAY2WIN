using TMPro;
using UnityEngine;
namespace KheloJeeto
{
	public class MessageView : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI msgText;

		// Start is called before the first frame update
		void Start()
		{

		}

		public void ShowMsg(string msg)
		{
			msgText.text = msg;
			Invoke(nameof(EraseMsg), 3f);
		}

		void EraseMsg()
		{
			msgText.text = "";
		}

	}
}