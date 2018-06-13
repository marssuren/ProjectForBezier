using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class JumpingNumberTextComponent : MonoBehaviour
{

	[SerializeField]
	[Tooltip("按最高位起始顺序设置每位数字Text（显示组）")]
	private List<Text> _numbers;
	[SerializeField]
	[Tooltip("按最高位起始顺序设置每位数字Text（替换组）")]
	private List<Text> _unactiveNumbers;
	/// <summary>
	/// 动画时长
	/// </summary>
	[SerializeField]
	private float _duration = 1.5f;
	/// <summary>
	/// 数字每次滚动时长
	/// </summary>
	[SerializeField]
	private float _rollingDuration = 0.05f;
	/// <summary>
	/// 数字每次变动数值
	/// </summary>
	[SerializeField]
	private int speed = 1;
	/// <summary>
	/// 滚动延迟（每进一位增加一倍延迟，让滚动看起来更随机自然）
	/// </summary>
	[SerializeField]
	private float _delay = 0.008f;
	/// <summary>
	/// Text文字宽高
	/// </summary>
	private Vector2 _numberSize;
	/// <summary>
	/// 当前数字
	/// </summary>
	private int curNumber;
	/// <summary>
	/// 起始数字
	/// </summary>
	[SerializeField]
	private int fromNumber;
	/// <summary>
	/// 最终数字
	/// </summary>
	[SerializeField]
	private int toNumber;
	/// <summary>
	/// 各位数字的缓动实例
	/// </summary>
	private List<Tweener> tweener = new List<Tweener>();
	/// <summary>
	/// 是否处于数字滚动中
	/// </summary>
	private bool isJumping;
	/// <summary>
	/// 滚动完毕回调
	/// </summary>
	public Action OnComplete;

	private void Awake()
	{
		if(_numbers.Count == 0 || _unactiveNumbers.Count == 0)
		{
			Debug.LogError("[JumpingNumberTextComponent] 还未设置Text组件!");
			return;
		}
		_numberSize = _numbers[0].rectTransform.sizeDelta;
	}

	public float duration
	{
		get
		{
			return _duration;
		}
		set
		{
			_duration = value;
		}
	}

	private float different;
	public float Different
	{
		get
		{
			return different;
		}
	}
	void Start()
	{
		//Change(100, 112);
	}
	public void Change(int _from, int _to)
	{
		bool tIsRepeatStop = isJumping && fromNumber == _from && toNumber == _to;	
		if(tIsRepeatStop)
		{
			return;
		}

		bool isContinuousChange = (toNumber == _from) && ((_to - _from > 0 && different > 0) || (_to - _from < 0 && different < 0));
		if(isJumping && isContinuousChange)
		{
		}
		else
		{
			fromNumber = _from;
			curNumber = fromNumber;
		}
		toNumber = _to;

		different = toNumber - fromNumber;
		speed = (int)Math.Ceiling(different / (_duration * (1 / _rollingDuration)));
		speed = speed == 0 ? (different > 0 ? 1 : -1) : speed;

		SetNumber(curNumber, false);
		isJumping = true;
		StopCoroutine(DoJumpNumber());
		StartCoroutine(DoJumpNumber());
	}

	public int number
	{
		get
		{
			return toNumber;
		}
		set
		{
			if(toNumber == value)
				return;
			Change(curNumber, toNumber);
		}
	}

	IEnumerator DoJumpNumber()
	{
		while(true)
		{
			if(speed > 0)//增加
			{
				curNumber = Math.Min(curNumber + speed, toNumber);
			}
			else if(speed < 0) //减少
			{
				curNumber = Math.Max(curNumber + speed, toNumber);
			}
			SetNumber(curNumber, true);

			if(curNumber == toNumber)
			{
				StopCoroutine("DoJumpNumber");
				isJumping = false;
				if(OnComplete != null)
					OnComplete();
				yield return null;
			}
			yield return new WaitForSeconds(_rollingDuration);
		}
	}

	/// <summary>
	/// 设置战力数字
	/// </summary>
	/// <param name="v"></param>
	/// <param name="isTween"></param>
	public void SetNumber(int _v, bool _isTween)
	{
		char[] c = _v.ToString().ToCharArray();
		Array.Reverse(c);
		string s = new string(c);

		if(!_isTween)
		{
			for(int i = 0; i < _numbers.Count; i++)
			{
				if(i < s.Count())
					_numbers[i].text = s[i] + "";
				else
					_numbers[i].text = "0";
			}
		}
		else
		{
			while(tweener.Count > 0)
			{
				tweener[0].Complete();
				tweener.RemoveAt(0);
			}

			for(int i = 0; i < _numbers.Count; i++)
			{
				if(i < s.Count())
				{
					_unactiveNumbers[i].text = s[i] + "";
				}
				else
				{
					_unactiveNumbers[i].text = "0";
				}

				_unactiveNumbers[i].rectTransform.anchoredPosition = new Vector2(_unactiveNumbers[i].rectTransform.anchoredPosition.x, (speed > 0 ? -1 : 1) * _numberSize.y);
				_numbers[i].rectTransform.anchoredPosition = new Vector2(_unactiveNumbers[i].rectTransform.anchoredPosition.x, 0);

				if(_unactiveNumbers[i].text != _numbers[i].text)
				{
					DoTween(_numbers[i], (speed > 0 ? 1 : -1) * _numberSize.y, _delay * i);
					DoTween(_unactiveNumbers[i], 0, _delay * i);

					Text tmp = _numbers[i];
					_numbers[i] = _unactiveNumbers[i];
					_unactiveNumbers[i] = tmp;
				}
			}
		}
	}

	public void DoTween(Text _text, float _endValue, float _delay)
	{
		Tweener t = DOTween.To(() => _text.rectTransform.anchoredPosition, (x) =>
		{
			_text.rectTransform.anchoredPosition = x;
		}, new Vector2(_text.rectTransform.anchoredPosition.x, _endValue), _rollingDuration - _delay).SetDelay(_delay);
		tweener.Add(t);
	}


	[ContextMenu("测试数字变化")]
	public void TestChange()
	{
		OnComplete += () =>
		{
			for(int i = 0; i < _numbers.Count; i++)
			{
				_numbers[i].transform.localPosition = new Vector2(_numbers[i].transform.localPosition.x, 0);
			}
		};
		Change(UnityEngine.Random.Range(1, 1), UnityEngine.Random.Range(1, 100000));
	}
}
