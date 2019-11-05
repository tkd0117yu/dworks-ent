using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{


	private int next_target;    //次のターゲットの数字

	void Start()
	{
		
	}

	void Update()
	{

	}

	//ボタンがクリックされた時の処理
	//引　数：int  :クリックされたボタンの数値
	//戻り値：bool :目的の番号の場合true
	public bool ClickNextTarge(int number)
	{
		bool result = false;
		if(number == next_target)
		{
			result = true;
			next_target++;
		}
		return result;
	}

	public void ResetNextTarge()
	{
		next_target = 1;
	}
}
