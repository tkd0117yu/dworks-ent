using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//ボタンを配置するクラス
public class ButtonManager : MonoBehaviour
{
	[SerializeField]
	private const int BUTTON_ELEMENT_COUNT_HORIZONTAL = 5;    //横のボタンの配置の大きさ
	[SerializeField]
	private const int BUTTON_ELEMENT_COUNT_VERTICAL = 5;    //縦のボタンの配置の大きさ

	private const int BUTTON_OBJECT_SIZE = 64;          //パのサイズの1辺の大きさ

	[SerializeField]
	private int[,] button_data_array_ = new int[BUTTON_ELEMENT_COUNT_HORIZONTAL, BUTTON_ELEMENT_COUNT_VERTICAL];  //ボタンのデータの配列

	public bool random_arrangement_ = true;  //ボタンをランダムに配置するフラグ

	public ButtonList original_buttonList_;         //オリジナルのボタンリスト
	public ButtonProcess original_buttonProcess_;   //オリジナルのボタンのButtonProcess.css
	private ButtonList now_game_buttonList_;        //現在のゲームののボタンリスト
	public GameManager game_manager_;               //GameManager.css

	void Start()
	{
	}

	void Update()
	{
		//debug
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SetArrangement();
		}
	}

	//ボタンを再配置する関数
	void SetArrangement()
	{

		int elements_content = 0;       //中身一時保管用
		string content_display = "";    //中身表示用

		game_manager_.ResetNextTarge();

		//新しいボタンの親を生成
		if (now_game_buttonList_ != null)
		{
			now_game_buttonList_.RestartGame();
		}

		now_game_buttonList_ = Instantiate(
			original_buttonList_,
			original_buttonList_.gameObject.transform.position,
			original_buttonList_.gameObject.transform.rotation,
			original_buttonList_.gameObject.transform.root
			);


		//初期化
		for (int verticall_size = 0; verticall_size < BUTTON_ELEMENT_COUNT_VERTICAL; verticall_size++)
		{
			for (int horizontal_size = 0; horizontal_size < BUTTON_ELEMENT_COUNT_HORIZONTAL; horizontal_size++)
			{
				button_data_array_[verticall_size, horizontal_size] = 0;
			}
		}

		//再配置
		for (int verticall_size = 0; verticall_size < BUTTON_ELEMENT_COUNT_VERTICAL; verticall_size++)
		{
			for (int horizontal_size = 0; horizontal_size < BUTTON_ELEMENT_COUNT_HORIZONTAL; horizontal_size++)
			{
				//ランダムな配置の使用
				if (random_arrangement_)
				{
					int freeze_dodge_ = 0;  //フリーズ回避用

					//乱数生成と重複チェック
					do
					{
						elements_content = UnityEngine.Random.Range(1, BUTTON_ELEMENT_COUNT_VERTICAL * BUTTON_ELEMENT_COUNT_HORIZONTAL + 1);

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
				GenerationButton(verticall_size, horizontal_size, elements_content, now_game_buttonList_.gameObject);

				//番号の保存
				button_data_array_[verticall_size, horizontal_size] = elements_content;
				content_display += "[" + elements_content.ToString("D2") + "]";
			}
			content_display += "\r\n";

		}

		//リストをアクティブ化
		now_game_buttonList_.gameObject.SetActive(true);

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
				if (button_data_array_[verticall_size, horizontal_size] == search_number)
				{
					has_duplicate = true;
				}
			}
		}
		return has_duplicate;
	}

	//ボタンの生成処理
	//引　数：1.int :縦の要素番号
	//		：2.int :横の要素番号
	//		：3.int :ボタンの番号
	private void GenerationButton(int coordinate_vertical, int coordinate_horizontal, int button_Number, GameObject parent_button)
	{

		//生成先の座標
		Vector2 generating_coordinate = new Vector2(
			(float)(coordinate_horizontal * BUTTON_OBJECT_SIZE) - (float)BUTTON_ELEMENT_COUNT_HORIZONTAL / 2.0f,
			(float)(BUTTON_ELEMENT_COUNT_VERTICAL * BUTTON_OBJECT_SIZE - coordinate_vertical * BUTTON_OBJECT_SIZE) - (float)BUTTON_ELEMENT_COUNT_VERTICAL / 2.0f
			);

		//ボタンの複製、配置
		ButtonProcess generated_object = Instantiate(
			original_buttonProcess_, 
			original_buttonProcess_.gameObject.transform.position,
			original_buttonProcess_.gameObject.transform.rotation,
			parent_button.transform
		);
		generated_object.transform.localPosition = generating_coordinate;

		//表示
		generated_object.gameObject.SetActive(true);

		//ボタンのデータの保存
		generated_object.SetButtonData(button_Number, ClickCallback);

	}

	//ボタンがクリックされた時の処理
	//引　数：1.int			:ボタンの番号
	//		：2.gameobject	:呼び出し先のボタンのオブジェクト
	public void ClickCallback(int button_number, GameObject gameObject)
	{
		if (game_manager_.ClickNextTarge(button_number))
		{
			Destroy(gameObject);
		}
	}
}


