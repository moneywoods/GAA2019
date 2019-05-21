/*
フェード付画面遷移クラス


＜使用法＞
このファイルをプロジェクトのアセットフォルダに追加

メニューのFile→Build Settings
Scene in Buildに使用するシーンをドラッグで追加

画面遷移したいタイミングで以下のコードを呼び出す
FadeManager.FadeOut("遷移したいシーン名");


参考サイト　https://onosendai.net/70/
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    //フェード用のCanvasとImage
    private static Canvas fadeCanvas;
	private static Image fadeImage;
    
	//フェードインアウトのフラグ
	public static bool isFadeIn = false;
	public static bool isFadeOut = false;

	//フェードしたい時間（単位は秒）
	private static float fadeTime = 0.5f;
    private static float timeElapsed = 0.0f;

	//遷移先のシーン名
	private static string nextScene;

    // 設定
    public static Color NextColor = Color.black;

    [Flags]
    public enum State
    {
        // fadeTimeの時間で行う処理。
        BIGGER    = 1 << 1, // 0000_0001 イメージサイズが大きくなる。
        SMALLER   = 1 << 2, // 0000_0010 イメージサイズが小さくなる。
        A_TO_ZERO = 1 << 3, // 0000_0100 透明度が大きくなる。
        A_TO_ONE  = 1 << 4, // 0000_1000 透明度が小さくなる。

        // チェック用
        CHECKER_IS_ACTIVE = 15 // 0000_1111
    }
    
    public static State CurrentState;

    //フェード用のCanvasとImage生成
    static void Init()
	{
		//フェード用のCanvas生成
		GameObject FadeCanvasObject = new GameObject("CanvasFade");

        fadeCanvas = FadeCanvasObject.AddComponent<Canvas>();
		FadeCanvasObject.AddComponent<GraphicRaycaster>();
		fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		FadeCanvasObject.AddComponent<FadeManager>();

		//最前面になるよう適当なソートオーダー設定
		fadeCanvas.sortingOrder = 100;

		//フェード用のImage生成
		fadeImage = new GameObject("ImageFade").AddComponent<Image>();
		fadeImage.transform.SetParent(fadeCanvas.transform, false);
		fadeImage.rectTransform.anchoredPosition = Vector3.zero;

		//Imageのサイズは適当に設定してください
		fadeImage.rectTransform.sizeDelta = new Vector2(1920, 1080);

        // 色の設定
        fadeImage.color = NextColor;
	}

	//フェードイン開始


	public static void FadeIn()
	{
		if (fadeImage == null) Init();
        fadeImage.color = NextColor; //一応
		isFadeIn = true;
        AddState(State.A_TO_ZERO);
	}

	//フェードアウト開始
	public static void FadeOut(string scene)
	{
		if (fadeImage == null) Init();
		nextScene = scene;

        //色の設定
		Color tmpColor = fadeImage.color;
        tmpColor.a = 0.0f;
        fadeImage.color = tmpColor;

		fadeCanvas.enabled = true;
        isFadeOut = true;
        AddState(State.A_TO_ONE);
    }

	void Update()
	{
		//フラグ有効なら毎フレームフェードイン/アウト処理
		if (CheckState(State.A_TO_ZERO))
		{
            //経過時間から透明度計算
            float alpha = fadeImage.color.a;
			alpha -= Time.deltaTime / fadeTime;

			//フェードイン終了判定
			if (alpha <= 0.0f)
			{
				fadeCanvas.enabled = false;
                RemoveState(State.A_TO_ZERO);
			}

            //フェード用Imageの透明度設定
            Color c = fadeImage.color;
            c.a = alpha;
			fadeImage.color = c;

		}
		else if (CheckState(State.A_TO_ONE))
		{
            //経過時間から透明度計算
            float alpha = fadeImage.color.a;
            alpha += Time.deltaTime / fadeTime;

			//フェードアウト終了判定
			if (alpha >= 1.0f)
			{
                RemoveState(State.A_TO_ONE);
			}

            //フェード用Imageの透明度設定
            Color c = fadeImage.color;
            c.a = alpha;
            fadeImage.color = c;

        }

        if(CheckState(State.BIGGER))
        {

        }
        else if(CheckState(State.SMALLER))
        {

        }

        if(isFadeIn)
        {
            timeElapsed += Time.deltaTime;

            if(fadeTime < timeElapsed)
            {
                isFadeIn = false;
                timeElapsed = 0.0f;
            }
        }
        else if(isFadeOut)
        {
            timeElapsed += Time.deltaTime;

            if (fadeTime < timeElapsed)
            {
                isFadeOut = false;
                timeElapsed = 0.0f;

                //次のシーンへ遷移
                SceneManager.LoadScene(nextScene);
            }
        }
	}

    // フェードイン/アウトのチェックフラグ
    public static bool CheckIsFade()
    {
        // シーン遷移が起動時はtrueを返す
        if (isFadeIn|| isFadeOut) return true;

        return false;
    }

    // フラグを追加する。
    public static State AddState(State state)
    {
        return CurrentState |= state;
    }

    // フラグを取り除く
    public static State RemoveState(State state)
    {
        return CurrentState &= ~state;
    }

    // フラグが立っているか見る。
    public static bool CheckState(State state)
    {
        if((CurrentState & state) == 0)
        {
            return false;
        }

        return true;
    }
}
