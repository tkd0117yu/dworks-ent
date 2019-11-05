using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//ボタンを配置するクラス
public class ButtonManager : MonoBehaviour
{
	//定数
	private const int BUTTON_ELEMENT_COUNT_HORIZONTAL = 5;		//横のボタンの配置の大きさ
	private const int BUTTON_ELEMENT_COUNT_VERTICAL = 5;		//縦のボタンの配置の大きさ
	private const int BUTTON_OBJECT_SIZE = 64;                  //ボタンのサイズの1辺の大きさ

	//ボタンのデータの配列
	private ButtonProcess[,] button_process_array_ = new ButtonProcess[BUTTON_ELEMENT_COUNT_HORIZONTAL, BUTTON_ELEMENT_COUNT_VERTICAL];  
	
	public ButtonProcess original_buttonProcess_;   //ボタン原本のButtonProcess.css
	public GameManager game_manager_;               //GameManager

	void Start()
	{
	}

	void Update()
	{
	}

	//ボタンを再配置する関数
	public void SetArrangement()
	{
		int elements_content = 0;       //中身一時保管用
		string content_display = "";    //中身表示用
		
		//初期化
		for (int verticall_size = 0; verticall_size < BUTTON_ELEMENT_COUNT_VERTICAL; verticall_size++)
		{
			for (int horizontal_size = 0; horizontal_size < BUTTON_ELEMENT_COUNT_HORIZONTAL; horizontal_size++)
			{
				if(button_process_array_[verticall_size, horizontal_size]!=null)
				{
					Destroy(button_process_array_[verticall_size, horizontal_size].gameObject);
					button_process_array_[verticall_size, horizontal_size] = null;
				}
			}
		}

		//再配置
		for (int verticall_size = 0; verticall_size < BUTTON_ELEMENT_COUNT_VERTICAL; verticall_size++)
		{
			for (int horizontal_size = 0; horizontal_size < BUTTON_ELEMENT_COUNT_HORIZONTAL; horizontal_size++)
			{
				//ランダムな配置の使用
				if (game_manager_.random_arrangement_)
				{
					int freeze_dodge_ = 0;  //フリーズ回避用

					//乱数生成と重複チェック
					do
					{
						//0~縦*横のサイズ内で乱数生成
						elements_content = UnityEngine.Random.Range(0, BUTTON_ELEMENT_COUNT_VERTICAL * BUTTON_ELEMENT_COUNT_HORIZONTAL);

						freeze_dodge_++;
						if (freeze_dodge_ > 1000)
						{
							Debug.Log("フリーズ回避");
							break;
						}

					} while (SearchDuplicate(elements_content));
				}
				else
				{
					elements_content = horizontal_size + verticall_size * BUTTON_ELEMENT_COUNT_HORIZONTAL;
				}

				//ボタンの生成
				button_process_array_[verticall_size, horizontal_size] = GenerationButton(verticall_size, horizontal_size, elements_content);

				//番号の保存
				button_process_array_[verticall_size, horizontal_size].number_ = elements_content;
				content_display += "[" + elements_content.ToString("D2") + "]";
			}
			content_display += "\r\n";

		}

		//配列の中身を表示
		Debug.Log(content_display);
	}

	//データの重複チェック
	//引　数：int  :チェックする数値
	//戻り値：bool : 重複がある場合true
	bool SearchDuplicate(int search_number)
	{
		bool has_duplicate = false;
		for (int verticall_size = 0; verticall_size < BUTTON_ELEMENT_COUNT_VERTICAL; verticall_size++)
		{
			for (int horizontal_size = 0; horizontal_size < BUTTON_ELEMENT_COUNT_HORIZONTAL; horizontal_size++)
			{
				if (button_process_array_[verticall_size, horizontal_size] != null)
				{
					if (button_process_array_[verticall_size, horizontal_size].number_ == search_number)
					{
						has_duplicate = true;
					}
				}
			}
		}
		return has_duplicate;
	}

	//ボタンの生成処理
	//引　数：1.int :縦の要素番号
	//		：2.int :横の要素番号
	//		：3.int :ボタンの番号
	private ButtonProcess GenerationButton(int coordinate_vertical, int coordinate_horizontal, int button_Number)
	{
		//生成先の座標
		Vector2 generating_coordinate = new Vector2(
			coordinate_horizontal * BUTTON_OBJECT_SIZE - BUTTON_ELEMENT_COUNT_HORIZONTAL / 2 * BUTTON_OBJECT_SIZE,
			(BUTTON_ELEMENT_COUNT_VERTICAL -1) * BUTTON_OBJECT_SIZE - coordinate_vertical * BUTTON_OBJECT_SIZE - BUTTON_ELEMENT_COUNT_VERTICAL / 2 * BUTTON_OBJECT_SIZE
		);

		//ボタンの複製、配置
		ButtonProcess generated_object = Instantiate(
			original_buttonProcess_, 
			original_buttonProcess_.gameObject.transform.position,
			original_buttonProcess_.gameObject.transform.rotation,
			original_buttonProcess_.gameObject.transform.root.gameObject.transform
		);
		generated_object.transform.localPosition = generating_coordinate;

		//表示
		generated_object.gameObject.SetActive(true);

		//ボタンのデータの保存
		generated_object.SetButtonData(button_Number, game_manager_.ClickCallback);

		return generated_object;
	}

	//ボタンをデリートする処理
	//引　数：int		:ボタンの番号
	public void DestroyButton(int button_number)
	{
		for (int verticall_size = 0; verticall_size < BUTTON_ELEMENT_COUNT_VERTICAL; verticall_size++)
		{
			for (int horizontal_size = 0; horizontal_size < BUTTON_ELEMENT_COUNT_HORIZONTAL; horizontal_size++)
			{
				if (button_process_array_[verticall_size, horizontal_size] != null)
				{
					if (button_process_array_[verticall_size, horizontal_size].number_ == button_number)
					{
						Destroy(button_process_array_[verticall_size, horizontal_size].gameObject);
						button_process_array_[verticall_size, horizontal_size] = null;
					}
				}
			}
		}
	}
}


