using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudReferences : MonoBehaviour
{
	[Header("Health")]
	public List<Image> HealthBars;
	public Image HealthWarningSymbol;
	public Image HealthBarBackground;
	public Image HealthBarBoarder;
	public TextMeshProUGUI HealthTimer;

	[Header("Radar")]
	public Image PlayerIcon;
	public List<GameObject> RadarNotches;
	public Image RadarBoarder;
	public Image RadarInnerBoarder;
	public Image RadarDividers;
	public Image RadarBackground;
	public TextMeshProUGUI WaveText;
	public Image WaveBoarder;
	public Image WaveBackground;

	[Header("Abilities")]
	public Image AbilityIcon;
	public Image AbilityBackground;
	public Image AbilityIconBoarder;
	public Image AbilityBarFill;
	public Image AbilityBarBoarder;
	public Image AbilityBarBackground;
	public Image AbilityMXBackground;
	public Image AbilityMXBoarder;
	public TextMeshProUGUI AbilityMXText;

	[Header("Information")]
	public TextMeshProUGUI ScoreText;
	public TextMeshProUGUI MultiplierText;
	public TextMeshProUGUI KillsText;
	public TextMeshProUGUI StreakText;

	[Header("Status")]
	public GridLayoutGroup BuffsPanel;
	public GridLayoutGroup DebuffsPanel;
	public GameObject[] Buffs;
	public GameObject[] Debuffs;
}
