using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
	public bool random_arrangement_ = true;		//ボタンをランダムに配置するフラグ

	private int next_target;					//次のターゲットの数字

	public ButtonManager button_manager_;       //ButtonManager

	//縦と横のサイズ指定
	//[Range(2, 8)] public int horizontal_size_ = 5;
	//[Range(2, 8)] public int vertical_size_ = 5;

	void Start()
	{
	}

	void Update()
	{
		//debug
		if (Input.GetKeyDown(KeyCode.Space))
		{
			RestartGame();
		}
	}

	//ボタンがクリックされた時の処理
	//引　数：int		:ボタンの番号
	public void ClickCallback(int button_number)
	{
		if (button_number == next_target)
		{
			next_target++;
			button_manager_.DestroyButton(button_number);
		}
	}

	//ゲームをリスタートする時の処理
	public void RestartGame()
	{
		Debug.Log("リスタート");
		button_manager_.SetArrangement();
		next_target = 0;
	}
}
