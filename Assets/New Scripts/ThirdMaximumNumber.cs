using UnityEngine;

public class ThirdMaximumNumber : MonoBehaviour
{

	[SerializeField] int[] nums;

	[ContextMenu("Third Max Num")]
	void ThridMaxNum()
	{
		if (nums.Length >= 3)
		{
			int[] sortedArray = new int[nums.Length];
			sortedArray = BubbleSort(nums);

			int thirdHighest = sortedArray[sortedArray.Length - 1];
			int count = 0;
			for (int i = sortedArray.Length - 1; i >= 0; i--)
			{
				if (sortedArray[i] < thirdHighest)
				{
					thirdHighest = sortedArray[i];
					count += 1;
					if (count == 3)
					{
						break;
					}
				}
			}
			print(thirdHighest);
		}
		else
		{
			int highestNum = -999;
			for (int i = 0; i < nums.Length; i++)
			{
				if (nums[i] > highestNum)
				{
					highestNum = nums[i];
				}
			}

			print(highestNum);
		}
	}

	int[] BubbleSort(int[] intArray)
	{
		int count = 0;

		for (int j = 0; j <= intArray.Length - 2; j++)
		{
			//intArray.Length - 2
			for (int i = 0; i <= intArray.Length - 2; i++)
			{
				count = count + 1;
				if (intArray[i] > intArray[i + 1])
				{
					int temp = intArray[i + 1];
					intArray[i + 1] = intArray[i];
					intArray[i] = temp;
				}
			}
		}
		return intArray;
	}
}
