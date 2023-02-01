using System;
using UnityEngine;
using ExaGames.Common;
using TMPro;
using UnityEngine.UI;
using CodeMonkey.Utils;
using Mkey;
using UI;
using Unity.VisualScripting;


public class LifeCount : MonoBehaviour {
	/// <summary>
	/// Reference to the LivesManager.
	/// </summary>
	public LivesManager LivesManager;
	/// <summary>
	/// Label to show number of available lives.
	/// </summary>
	public Text LivesText;
	/// <summary>
	/// Label to show time to next life.
	/// </summary>
	public Text TimeToNextLifeText;

	public static LifeCount Instance;

	/// <summary>
	/// Image display for the result of consuming a life.
	/// </summary>
	/// <remarks>
	/// This should be, for example, a reference to your game controller.
	/// </remarks>
	public GameObject lifeWindow;
	public Button_UI btnAdsGetHeart;
	public Button_UI btnAdsClose;


	public void Awake()
	{
		Instance = this;
		
		btnAdsGetHeart.ClickFunc = () =>
		{
			SoundMaster.Instance.SoundPlayClick(0, null);
			UIAdsController.Instance.ShowStatic();
		};
		
		btnAdsClose.ClickFunc = () =>
		{
			SoundMaster.Instance.SoundPlayClick(0, null);
			UIAdsController.Instance.HideStatic();

		};
	}

	private void Update() {
		#if UNITY_EDITOR
		/* When testing in the editor, you can use the keyboard to control the lives manager:
		 * C - Consume life
		 * G - Give one
		 * F - Fill lives
		 * A - Add one slot (increase max)
		 * Z - Add one slot and fill
		 * I - Give infinite lives
		 */ 

		if(Input.GetKeyUp(KeyCode.C)) {
			OnButtonConsumePressed();
		}

		if(Input.GetKeyUp(KeyCode.G)) {
			OnButtonGiveOnePressed();
		}

		if(Input.GetKeyUp(KeyCode.F)) {
			OnButtonFillPressed();
		}

		if(Input.GetKeyUp(KeyCode.A)) {
			OnButtonIncreaseMaxPressed();
		}

		if(Input.GetKeyUp(KeyCode.Z)) {
			LivesManager.AddLifeSlots(1, true);
		}

		if(Input.GetKeyUp(KeyCode.I)) {
			OnButtonInfinitePressed();
		}
		#endif
	}

	#region Button Event Handlers

	/// <summary>
	/// Play (consume life) button event handler.
	/// </summary>
	public bool checkLoadLevel = false;
	
	public void OnButtonConsumePressed() {
		if(LivesManager.ConsumeLife()) {
			// Go to your game!
			checkLoadLevel = true;
			Debug.Log("A life was consumed and the player can continue!");
			Debug.Log("on comsume in level ");
		} else {
			// Tell player to buy lives, then:
			// LivesManager.GiveOneLife();
			// or
			// LivesManager.FillLives();
			Debug.Log("Not enough lives to play!");
			// ResultDisplay.PopUp("Over");
			checkLoadLevel = false;
			UIAdsController.Instance.ShowStatic();;
		}
	}

	public void ClosePopUp()
	{
		UIAdsController.Instance.HideStatic();;
	}

	public void OnButtonGiveOnePressed() {
		LivesManager.GiveOneLife();
	}

	public void OnButtonFillPressed() {
		LivesManager.FillLives();
	}

	public void OnButtonInfinitePressed() {
		LivesManager.GiveInifinite(1);
	}

	public void OnButtonIncreaseMaxPressed() {
		LivesManager.AddLifeSlots(1);
		Debug.LogFormat("Max lives is now {0}", LivesManager.MaxLives);
	}

	public void OnButtonResetPressed() {
		LivesManager.ResetPlayerPrefs();
		Debug.LogFormat("Max lives is now {0}", LivesManager.MaxLives);
		OnLivesChanged();
		OnTimeToNextLifeChanged();
	}
	#endregion

	/// <summary>
	/// Lives changed event handler, changes the label value.
	/// </summary>
	public void OnLivesChanged() {
		LivesText.text = LivesManager.LivesText;
	}

	/// <summary>
	/// Time to next life changed event handler, changes the label value.
	/// </summary>
	public void OnTimeToNextLifeChanged() {
		TimeToNextLifeText.text = LivesManager.RemainingTimeString;
	}
}