using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonProcess : MonoBehaviour
{
	private System.Action<int> ClickCallback;
	public int number_;		//ボタンが持つ担当番号
	public Text text_asset; //テキストのアセット
	
	void Start()
    {
    }

    void Update()
    {
    }

	//ボタンが押されたときに実行する
	public void OnClick()
	{
		ClickCallback(number_);
	}

	//ボタンのデータを保存する関数
	//引　数：1.int :ボタンのナンバー
	//		：2.int :ClickCallback関数
	public void SetButtonData(int number, System.Action<int> action)
	{
		number_ = number;
		text_asset.text = (number_ + 1) + "";
		ClickCallback = action;
	}
}
