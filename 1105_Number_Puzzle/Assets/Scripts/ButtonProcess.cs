using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonProcess : MonoBehaviour
{

	private System.Action<int,GameObject> ClickCallback;
	public int number_;

	public Text text_asset;	//テキストのアセット
	

	void Start()
    {
    }

    void Update()
    {
    }

	//ボタンが押されたときに実行する
	public void OnClick()
	{
		ClickCallback(number_,this.gameObject);
	}

	//ボタンのデータを保存する関数
	//引　数：1.int :ボタンのナンバー
	//		：2.int :ClickCallback関数
	public void SetButtonData(int number, System.Action<int,GameObject> action)
	{
		number_ = number;

		text_asset.text = number_ + "";

		ClickCallback = action;
	}
}
