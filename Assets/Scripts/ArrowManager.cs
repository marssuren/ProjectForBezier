using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class ArrowManager : MonoBehaviour
{

	public static ArrowManager instance;

	/// <summary>
	/// 箭头聚焦的圆环
	/// </summary>
	Transform focusRingTfm;

	RectTransform canvasRect;

	/// <summary>
	/// 将屏幕上的一点投影到RectTransform上的世界空间坐标
	/// </summary>
	Vector3 worldPosInRect;

	//Transform arrowHeadTfm ;

	/// <summary>
	/// The arrow mask tfm.
	/// </summary>
	Transform arrowMaskTfm;

	Transform nodesContainerTfm;

	/// <summary>
	/// 箭头从哪个物体起始的
	/// </summary>
	Transform startTfm;

	/// <summary>
	/// 箭头身体的从第几个孩子——Node开始
	/// </summary>
	int initialIndex;

	/// <summary>
	/// 临时的Node节点
	/// </summary>
	Transform tempNodeTfm;

	/// <summary>
	/// 箭头的可见长度
	/// </summary>
	float visibleLen = 1500f;

	/// <summary>
	/// 箭头的流动速度
	/// </summary>
	[Range(10f, 300f)]
	public float flowSpeed = 150f;

	/// <summary>
	/// 遮罩箭头Node的RectTransform
	/// </summary>
	RectTransform maskRect;

	/// <summary>
	/// Rectangle上拖拽的起始位点的世界坐标
	/// </summary>
	Vector2 dragStartPos;

	[Range(30f, 120f)]
	public float offset = 50f;

	Vector3 offsetV = new Vector3(0f, 50f, 0f);

	/// <summary>
	/// Arrow 的 Head 部分的高度
	/// </summary>
	[Range(40f, 120f)]
	const float minHeight = 80f;

	/// <summary>
	/// 箭头当前是否处于激活状态
	/// </summary>
	bool mActive = false;

	float dist;

	protected void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start()
	{
		//Initialize();
		//float tVector2 = Vector2.Angle(new Vector2(0, 1), new Vector2(2, 2));
		Quaternion tQuaternion = CaculateRotation(new Vector2(0, 1), new Vector2(2, 2));
		Vector3 tVector3 = tQuaternion.eulerAngles;
		Debug.LogError(tVector3);

	}

	void FixedUpdate()
	{

		MakeArrowFlow();

	}

	void Initialize()
	{
		//arrowHeadTfm = transform.Find ("ArrowHead");
		mActive = false;

		arrowMaskTfm = transform.GetChild(0);

		maskRect = arrowMaskTfm.GetComponent<RectTransform>();

		nodesContainerTfm = arrowMaskTfm.Find("NodesContainer");

		focusRingTfm = GameObject.Find("FocusRing").transform;

		canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();

	}

	void MakeArrowFlow()
	{
		if(!mActive)
			return;
		//改变箭头前端的透明度
		for(int i = 0; i < nodesContainerTfm.childCount; i++)
		{
			tempNodeTfm = nodesContainerTfm.GetChild(i);

			tempNodeTfm.localPosition = new Vector3(0f, tempNodeTfm.localPosition.y + Time.fixedDeltaTime * flowSpeed, 0f);

			//改变箭头起点的透明度
			initialIndex = (int)(visibleLen / 100f);

			if(i <= 2)
			{
				tempNodeTfm.GetComponent<Image>().color = Color.Lerp(tempNodeTfm.GetComponent<Image>().color, new Color(1, 1, 1, (60 * i + 60) / 255f), Time.fixedDeltaTime * 5f);
			}
			else if(i <= (initialIndex + 3) && i >= (initialIndex - 3))
			{
				int diff = i - (initialIndex - 3);
				tempNodeTfm.GetComponent<Image>().color = Color.Lerp(tempNodeTfm.GetComponent<Image>().color, new Color(1, 1, 1, (255f - 40f * diff) / 255f), Time.fixedDeltaTime * 5f);
			}
			else if(i > (initialIndex + 3))
			{
				tempNodeTfm.GetComponent<Image>().color = new Color(1, 1, 1, 0);
			}
			else
			{
				tempNodeTfm.GetComponent<Image>().color = Color.white;
			}

			if(tempNodeTfm.localPosition.y > -100f)
			{
				tempNodeTfm.GetComponent<Image>().color = Color.white;
				tempNodeTfm.localPosition = new Vector3(0f, -100 + nodesContainerTfm.GetChild(nodesContainerTfm.childCount - 1).localPosition.y, 0f);
				tempNodeTfm.SetAsLastSibling();
			}
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		mActive = true;

		transform.localScale = Vector3.one;

		Vector3 startObjPos = eventData.pointerDrag.gameObject.transform.position;

		WorldPointInRectangle(canvasRect, startObjPos, Camera.main, out worldPosInRect);

		transform.position = worldPosInRect;

		dragStartPos = worldPosInRect;
	}


	/// <summary>
	/// 将世界空间下一点投影到目标Rectangle上，得到投影点在世界空间中的坐标
	/// </summary>
	/// <param name="rect">Rect.</param>
	/// <param name="worldPos">World position.</param>
	/// <param name="camera">Camera.</param>
	/// <param name="worldPosInRect">World position in rect.</param>
	void WorldPointInRectangle(RectTransform rect, Vector3 worldPos, Camera camera, out Vector3 worldPosInRect)
	{
		Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
		RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, screenPos, camera, out worldPosInRect);
	}


	public void OnDrag(PointerEventData eventData)
	{
		//transform.position = eventData.pointerDrag.gameObject.transform.position;

		RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, eventData.position, Camera.main, out worldPosInRect);

		transform.position = worldPosInRect;

		transform.rotation = CaculateRotation(worldPosInRect, dragStartPos);
		//dist = Vector2.Distance (worldPosInRect, dragStartPos) + offset;
		CaculateVisibleLen(worldPosInRect);
		dist = visibleLen + offset;
		dist = dist >= minHeight ? dist : minHeight;
		maskRect.sizeDelta = new Vector2(100f, dist);
		RayCastCheck();
	}

	void RayCastCheck()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//Debug.DrawLine (ray.origin, ray.origin + 10000 * ray.direction, Color.red);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("BattleUnit")))
		{
			Transform hitTfm = hit.collider.transform;
			if(hitTfm != startTfm)
			{
				ShowFocusRing(hitTfm);
				RotateFocusRing(worldPosInRect);
			}
		}
		else
		{
			HideFocusRing();
		}
	}

	void ShowFocusRing(Transform focusTargetTfm)
	{
		//focusRingTfm.localScale = Vector3.one;
		focusRingTfm.gameObject.SetActive(true);
		focusRingTfm.position = focusTargetTfm.position;
	}

	/// <summary>
	/// Rotates the focus ring.
	/// </summary>
	/// <param name="currentPos">当前鼠标投影到CanvasRect上的世界坐标.</param>
	void RotateFocusRing(Vector3 currentPos)
	{
		Vector3 focusRingPosInRect;
		WorldPointInRectangle(canvasRect, focusRingTfm.position, Camera.main, out focusRingPosInRect);
		focusRingTfm.rotation = CaculateRotation(currentPos, focusRingPosInRect);
	}

	void HideFocusRing()
	{
		//focusRingTfm.localScale = Vector3.zero;
		focusRingTfm.gameObject.SetActive(false);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		transform.localScale = Vector3.zero;
		this.startTfm = null;
		mActive = false;
		HideFocusRing();
	}

	/// <summary>
	/// 计算箭头身体的可见长度
	/// </summary>
	/// <param name="currentPos">Current position.</param>
	void CaculateVisibleLen(Vector2 currentPos)
	{
		Vector2 dirVector = currentPos - dragStartPos;

		//因为Arrow本身是处在Canvas上的，Arrow的长度会受父物体影响
		//这先将这一长度尺寸还原到世界空间下的尺寸，然后被父物体缩放影响，得到正确尺寸
		visibleLen = dirVector.magnitude / canvasRect.localScale.x;
	}

	/// <summary>
	/// 输入当前拖拽位置，获得箭头的正确转向——rotation
	/// </summary>
	/// <returns>The rotation.</returns>
	/// <param name="currentPos">Current position.</param>
	Quaternion CaculateRotation(Vector2 currentPos, Vector2 middlePos)
	{
		Vector2 fromVector = Vector2.up;
		Vector2 toVector = currentPos - middlePos;
		//虽然形参的名称好像是会有方向区别
		//即从哪个向量到哪个向量
		//然而实际中操作发现，它只会返回两个向量之间的最小非负数夹角
		float angle = Vector2.Angle(fromVector, toVector);
		//当x分量大于0时，Vector2.Angle 函数得到的角度为绕z轴顺时针度数
		if(toVector.x > 0)
		{
			angle = 360f - angle;
		}
		//组合得到欧拉角
		Vector3 diff = new Vector3(0f, 0f, angle);
		//将欧拉角转化为四元数
		Quaternion rotation = Quaternion.Euler(diff);
		return rotation;
	}
}