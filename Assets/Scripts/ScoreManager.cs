using UnityEngine;
using System.Collections;

public static class ScoreManager {
	public const string VER_ONE_PLAYER  = "one_player_version";
	public const string VER_TWO_PLAYERs = "two_players_version";

	const string SCORE_MANAGER_PREFS_PREFIX = "ScoreManager_";

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

	public static bool SetHighScore(int score, string version = VER_ONE_PLAYER){
		int oldScore = PlayerPrefs.GetInt ( GetPrefKey(version), 0);
		if (score > oldScore) {
			PlayerPrefs.SetInt ( GetPrefKey(version), score );
			return true;
		}
		return false;
	}

	public static int GetHighScore(string version = VER_ONE_PLAYER){
		return PlayerPrefs.GetInt ( GetPrefKey(version), 0 );
	}

	static string GetPrefKey(string version){
		return SCORE_MANAGER_PREFS_PREFIX + version;
	}
}
	