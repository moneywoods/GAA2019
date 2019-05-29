/* 
 * 使い方
 * AddState関数でやりたいことを設定する。
 * 徐々に大きさを変化させる場合 AddStateの引数を BIGGER か SMALLER にして呼び出し、Size_StartとSize_Endをそれぞれ指定する（Unmaskのサイズも同様に指定可能）
 * NextColorで色の変更ができます
 * ・画像の変更 Resources/SpritesフォルダにSpriteを追加して、imagePathArray配列にパス、ImageIndexにインデックスを書き込む
 *   SetImage関数でイメージの設定ができます
 * Unmaskについてもほぼ同様
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Coffee.UIExtensions;

public class FadeManager : MonoBehaviour
{

    // 設定
    public static Color NextColor = Color.black;
    public static Vector2 ImageSize_Start;
    public static Vector2 ImageSize_End;

    public static Vector2 UnmaskSize_Start;
    public static Vector2 UnmaskSize_End;
    private static GameObject instance;

    //フェード用のCanvasとImage
    private static Canvas fadeCanvas;
	private static Image mask;
    private static Image unmask;
    private static Image image;
    
	//フェードインアウトのフラグ
	public static bool isFadeIn = false;
	public static bool isFadeOut = false;

	//フェードしたい時間（単位は秒）
	private static float fadeTime = 0.5f;
    private static float timeElapsed = 0.0f;

	//遷移先のシーン名
	private static string nextScene;


    [Flags, Serializable]
    public enum State
    {
        // fadeTimeの時間で行う処理。
        NONE      = 0,
        BIGGER    = 1 << 1, // 0000_0001 イメージサイズが大きくなる
        SMALLER   = 1 << 2, // 0000_0010 イメージサイズが小さくな
        A_TO_ZERO = 1 << 3, // 0000_0100 透明度が大きくなる
        A_TO_ONE  = 1 << 4, // 0000_1000 透明度が小さくなる
        UNMASK    = 1 << 5, // 0001_0000 Unmaskを利用する

        // チェック用
        CHECKER_IS_ACTIVE = 15 // 0000_1111
    }
    
    public static State CurrentState;
    [Serializable]
    public enum ImageIndex
    {
        STAR_1_ALPHA,
        MENDAKO_1_ALPHA,
        NONE,
        OTHER
    }

    // 使用する画像（Material)のパス。
    // 画像のαを利用するためシェーダはUnlit->Transparentに設定する。
    // 上のImageIndexとの順序の一致を確認すること。
    private static string[] imagePathArray = 
    {
        "FadeImages/Sprites/star_1_alpha",
        "FadeImages/Sprites/mendako_1_alpha"
    };

    private static List<Sprite> imageList;
    [SerializeField] private static ImageIndex CurrentImage = ImageIndex.NONE;
    [SerializeField] private static ImageIndex CurrentUnmask = ImageIndex.NONE;

    // 処理用の変数
    private static Vector2 imageDiff;
    private static Vector2 unmaskDiff;

    //フェード用のCanvasとImage生成
    static void Init()
	{
        // イメージをリストに保持
        if(imageList == null)
        {
            imageList = new List<Sprite>();
        }

        for (int i = 0; i < imagePathArray.GetLength(0); i++)
        {
            imageList.Add(Resources.Load<Sprite>(imagePathArray[i]));
        }

        // GameObjectの生成
        //フェード用のCanvas生成
        GameObject FadeCanvasObject = new GameObject("CanvasFade");

        fadeCanvas = FadeCanvasObject.AddComponent<Canvas>();
		FadeCanvasObject.AddComponent<GraphicRaycaster>();
		fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		FadeCanvasObject.AddComponent<FadeManager>();

        // mask
        //  +unmask imageにアルファのある画像を設定する
        //  +image ここから下のimageがunmaskで切り抜きされる
		//最前面になるよう適当なソートオーダー設定
		fadeCanvas.sortingOrder = 100;

        // mask Maskコンポーネントが必須
        mask = new GameObject("mask").AddComponent<Image>();
        mask.transform.SetParent(fadeCanvas.transform, false);
        mask.rectTransform.anchoredPosition = Vector3.zero;
        mask.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        mask.gameObject.AddComponent<Mask>();
        mask.GetComponent<Mask>().showMaskGraphic = false;
        mask.gameObject.SetActive(true);

        // unmask Maskコンポーネントのついたオブジェクトの子にする
        unmask = new GameObject("unmask").AddComponent<Image>();
        unmask.transform.SetParent(mask.transform, false);
        unmask.rectTransform.anchoredPosition = Vector3.zero;
        unmask.gameObject.AddComponent<Unmask>();
        unmask.gameObject.SetActive(false);
        unmask.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        if(CurrentUnmask != ImageIndex.NONE)
        {
            unmask.sprite = imageList[(int)CurrentUnmask];
        }

        //image unmaskで切り抜かれるimage
        image = new GameObject("image").AddComponent<Image>();
		image.transform.SetParent(mask.transform, false);
		image.rectTransform.anchoredPosition = Vector3.zero;

        if (CurrentImage != ImageIndex.NONE)
        {
            image.sprite = imageList[(int)CurrentImage];
        }

        //Imageのサイズは適当に設定してください
        image.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        // 色の設定
        image.color = NextColor;
    }
    
    private static void ResetSize()
    {
        mask.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        image.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        unmask.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    private static void InitFade()
    {
        // サイズの初期化
        image.rectTransform.sizeDelta = ImageSize_Start;
        unmask.rectTransform.sizeDelta = UnmaskSize_Start;

        // 差分の計算
        imageDiff = (ImageSize_End - ImageSize_Start) / fadeTime;
        unmaskDiff = (UnmaskSize_End - UnmaskSize_Start) / fadeTime;
    }

    //シーン導入開始
    public static void SceneIn()
	{
		if (mask == null) Init();
        image.color = NextColor; //一応
        fadeCanvas.enabled = true;
		isFadeIn = true;
        
        InitFade();
    }

	//シーン遷移開始
	public static void SceneOut(string scene)
	{
		if (mask == null) Init();
		nextScene = scene;

        //色の設定
		Color tmpColor = image.color;
        tmpColor.a = 0.0f;
        image.color = tmpColor;

		fadeCanvas.enabled = true;
        isFadeOut = true;

        InitFade();
    }

	void Update()
	{
		//フラグ有効なら毎フレームフェードイン/アウト処理
		if (CheckState(State.A_TO_ZERO))
		{
            //経過時間から透明度計算
            float alpha = image.color.a;
			alpha -= Time.deltaTime / fadeTime;

			//フェードイン終了判定
			if (alpha <= 0.0f)
			{
				fadeCanvas.enabled = false;
                RemoveState(State.A_TO_ZERO);
			}

            //フェード用Imageの透明度設定
            Color c = image.color;
            c.a = alpha;
            image.color = c;

		}
		else if (CheckState(State.A_TO_ONE))
		{
            //経過時間から透明度計算
            float alpha = image.color.a;
            alpha += Time.deltaTime / fadeTime;

			//フェードアウト終了判定
			if (alpha >= 1.0f)
			{
                RemoveState(State.A_TO_ONE);
			}

            //フェード用Imageの透明度設定
            Color c = image.color;
            c.a = alpha;
            image.color = c;

        }

        if(CheckState(State.BIGGER) || CheckState(State.SMALLER))
        {
            var size = mask.rectTransform.sizeDelta + imageDiff * Time.deltaTime; // maskとimage用のサイズ
            mask.rectTransform.sizeDelta = size;
            image.rectTransform.sizeDelta = size;

            unmask.rectTransform.sizeDelta = unmask.rectTransform.sizeDelta + unmaskDiff * Time.deltaTime;
        }

        if(isFadeIn)
        {
            timeElapsed += Time.deltaTime;

            // 終了判定
            if(fadeTime < timeElapsed)
            {
                isFadeIn = false;
                timeElapsed = 0.0f;
                ClearState();
                ResetSize();
            }
        }
        else if(isFadeOut)
        {
            timeElapsed += Time.deltaTime;

            // 終了判定
            if(fadeTime < timeElapsed)
            {
                isFadeOut = false;
                timeElapsed = 0.0f;
                ClearState();
                ResetSize();
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
    // 戻り値: 処理後のステートフラグ
    public static State AddState(State state)
    {
        CurrentState |= state;

        if(CheckState(State.UNMASK))
        {
            unmask.gameObject.SetActive(true);
        }

        return CurrentState;
    }

    // フラグを取り除く
    // 戻り値: 処理後のステートフラグ
    public static State RemoveState(State state)
    {
        CurrentState &= ~state;

        if (!CheckState(State.UNMASK))
        {
            unmask.gameObject.SetActive(false);
        }

        return CurrentState;
    }

    // フラグを全リセット
    public static void ClearState()
    {
        CurrentState = State.NONE;
    }

    // フラグが立っているか見る。
    // 戻り値: 立っていたらture
    public static bool CheckState(State state)
    {
        if((CurrentState & state) == 0)
        {
            return false;
        }

        return true;
    }

    // インデックスから表示する画像を選択する（同時に表示できるのは1枚）。
    public static void SetImage(ImageIndex index)
    {
        if (mask == null) Init();

        if (ImageIndex.NONE <= index)
        {
            image.sprite = null;
            return;
        }

        image.sprite = imageList[(int)index];
        CurrentImage = index;
    }

    // 直接マテリアルを設定する。
    public static void SetImage(Sprite s)
    {
        if (mask == null) Init();

        image.sprite = s;
        CurrentImage = ImageIndex.OTHER;
    }

    public static void SetUnmaskImage(ImageIndex index)
    {
        if (mask == null) Init();

        if (ImageIndex.NONE <= index)
        {
            unmask.sprite = null;
            unmask.gameObject.SetActive(false);
            return;
        }

        unmask.sprite = imageList[(int)index];
        CurrentUnmask = index;
    }

    public static void SetUnmaskImage(Sprite s)
    {
        if (mask == null) Init();

        unmask.sprite = s;
        CurrentUnmask = ImageIndex.OTHER;
    }
}
