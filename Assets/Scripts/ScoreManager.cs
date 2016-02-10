using UnityEngine;
using System.Collections;

public class ScoreManager {

	public static int GetTotalScore(int colCount, int rowCount){
		int multiplier = (colCount > 0 && rowCount > 0) ? 3 : 2;
		int scoreCol = colCount == 0 ? 0 : sequence(colCount);
		int scoreRow = rowCount == 0 ? 0 : sequence(rowCount);

		return (scoreCol + scoreRow) * multiplier;
	}

//	static int[] weights = new int[]{0,1,3,7,13,21,31,43,57,};
	static int sequence(int num){
		if (num == 1)
			return 1;
		else
			return sequence (num - 1) + 2 * (num - 1);
	}
}
	