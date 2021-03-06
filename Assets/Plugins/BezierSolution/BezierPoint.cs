﻿using UnityEngine;

namespace BezierSolution
{
	public class BezierPoint : MonoBehaviour
	{
		public enum HandleMode
		{
			Free,
			Aligned,		//对齐
			Mirrored		//镜像
		};

		public Vector3 localPosition        //自身localPosition
		{
			get
			{
				return transform.localPosition;
			}
			set
			{
				transform.localPosition = value;
			}
		}

		[SerializeField]
		[HideInInspector]
		private Vector3 m_position;
		public Vector3 position                     //自身position
		{
			get
			{
				if(transform.hasChanged)
					Revalidate();

				return m_position;
			}
			set
			{
				transform.position = value;
			}
		}

		public Quaternion localRotation                 //自身localRotation
		{
			get
			{
				return transform.localRotation;
			}
			set
			{
				transform.localRotation = value;
			}
		}

		public Quaternion rotation                      //自身rotation
		{
			get
			{
				return transform.rotation;
			}
			set
			{
				transform.rotation = value;
			}
		}

		public Vector3 localEulerAngles                 //相对于父对象的旋转角度
		{
			get
			{
				return transform.localEulerAngles;
			}
			set
			{
				transform.localEulerAngles = value;
			}
		}

		public Vector3 eulerAngles                      //自身旋转角度
		{
			get
			{
				return transform.eulerAngles;
			}
			set
			{
				transform.eulerAngles = value;
			}
		}

		public Vector3 localScale                       //自身缩放
		{
			get
			{
				return transform.localScale;
			}
			set
			{
				transform.localScale = value;
			}
		}

		//[SerializeField]
		[HideInInspector]
		private Vector3 m_precedingControlPointLocalPosition = Vector3.left;
		public Vector3 precedingControlPointLocalPosition                       //跟随点的本地坐标
		{
			get
			{
				return m_precedingControlPointLocalPosition;
			}
			set
			{
				m_precedingControlPointLocalPosition = value;
				m_precedingControlPointPosition = transform.TransformPoint(value);

				if(m_handleMode == HandleMode.Aligned)		//对齐
				{
					m_followingControlPointLocalPosition = -m_precedingControlPointLocalPosition.normalized * m_followingControlPointLocalPosition.magnitude;	//跟随点本地坐标=设定点坐标的反方向*自身长度
					m_followingControlPointPosition = transform.TransformPoint(m_followingControlPointLocalPosition);//将相对 “当前游戏对象” 的坐标转化为基于世界坐标系的坐标
				}
				else if(m_handleMode == HandleMode.Mirrored)	//镜像
				{
					m_followingControlPointLocalPosition = -m_precedingControlPointLocalPosition;			//跟随点本地坐标=设定点反向的本地坐标
					m_followingControlPointPosition = transform.TransformPoint(m_followingControlPointLocalPosition);//将相对 “当前游戏对象” 的坐标转化为基于世界坐标系的坐标
				}
			}
		}

		//[SerializeField]
		[HideInInspector]
		private Vector3 m_precedingControlPointPosition;
		public Vector3 precedingControlPointPosition                            //前点的世界坐标
		{
			get
			{
				if(transform.hasChanged)			//如果自身坐标有变化
					Revalidate();					//重新刷新

				return m_precedingControlPointPosition;
			}
			set
			{
				m_precedingControlPointPosition = value;
				m_precedingControlPointLocalPosition = transform.InverseTransformPoint(value);//将世界坐标转化为相对"当前游戏对象"的基于世界坐标系的坐标

				if(transform.hasChanged)
				{
					m_position = transform.position;
					transform.hasChanged = false;
				}

				if(m_handleMode == HandleMode.Aligned)
				{
					m_followingControlPointPosition = m_position - (m_precedingControlPointPosition - m_position).normalized *
																   (m_followingControlPointPosition - m_position).magnitude;
					m_followingControlPointLocalPosition = transform.InverseTransformPoint(m_followingControlPointPosition);//将世界坐标转化为相对"当前游戏对象"的基于世界坐标系的坐标
				}
				else if(m_handleMode == HandleMode.Mirrored)
				{
					m_followingControlPointPosition = 2f * m_position - m_precedingControlPointPosition;
					m_followingControlPointLocalPosition = transform.InverseTransformPoint(m_followingControlPointPosition);//将世界坐标转化为相对"当前游戏对象"的基于世界坐标系的坐标
				}
			}
		}

		//[SerializeField]
		[HideInInspector]
		private Vector3 m_followingControlPointLocalPosition = Vector3.right;
		public Vector3 followingControlPointLocalPosition				//跟随点的本地坐标
		{
			get
			{
				return m_followingControlPointLocalPosition;
			}
			set
			{
				m_followingControlPointLocalPosition = value;
				m_followingControlPointPosition = transform.TransformPoint(value);		//将目标坐标转化为世界坐标系的坐标

				if(m_handleMode == HandleMode.Aligned)
				{
					m_precedingControlPointLocalPosition = -m_followingControlPointLocalPosition.normalized * m_precedingControlPointLocalPosition.magnitude;//处理点的localPosition
					m_precedingControlPointPosition = transform.TransformPoint(m_precedingControlPointLocalPosition);
				}
				else if(m_handleMode == HandleMode.Mirrored)
				{
					m_precedingControlPointLocalPosition = -m_followingControlPointLocalPosition;
					m_precedingControlPointPosition = transform.TransformPoint(m_precedingControlPointLocalPosition);
				}
			}
		}

		//[SerializeField]
		[HideInInspector]
		private Vector3 m_followingControlPointPosition;
		public Vector3 followingControlPointPosition		//跟随点的世界坐标
		{
			get
			{
				if(transform.hasChanged)
					Revalidate();

				return m_followingControlPointPosition;
			}
			set
			{
				m_followingControlPointPosition = value;
				m_followingControlPointLocalPosition = transform.InverseTransformPoint(value);

				if(transform.hasChanged)
				{
					m_position = transform.position;
					transform.hasChanged = false;
				}

				if(m_handleMode == HandleMode.Aligned)
				{
					m_precedingControlPointPosition = m_position - (m_followingControlPointPosition - m_position).normalized *
																	(m_precedingControlPointPosition - m_position).magnitude;
					m_precedingControlPointLocalPosition = transform.InverseTransformPoint(m_precedingControlPointPosition);
				}
				else if(m_handleMode == HandleMode.Mirrored)
				{
					m_precedingControlPointPosition = 2f * m_position - m_followingControlPointPosition;
					m_precedingControlPointLocalPosition = transform.InverseTransformPoint(m_precedingControlPointPosition);
				}
			}
		}

		//[SerializeField]
		[HideInInspector]
		private HandleMode m_handleMode = HandleMode.Mirrored;
		public HandleMode handleMode
		{
			get
			{
				return m_handleMode;
			}
			set
			{
				m_handleMode = value;

				if(value == HandleMode.Aligned || value == HandleMode.Mirrored)
					precedingControlPointLocalPosition = m_precedingControlPointLocalPosition;
			}
		}

		private void Awake()
		{
			transform.hasChanged = true;
		}

		public void CopyTo(BezierPoint other)
		{
			other.transform.localPosition = transform.localPosition;
			other.transform.localRotation = transform.localRotation;
			other.transform.localScale = transform.localScale;

			other.m_handleMode = m_handleMode;

			other.m_precedingControlPointLocalPosition = m_precedingControlPointLocalPosition;
			other.m_followingControlPointLocalPosition = m_followingControlPointLocalPosition;
		}

		private void Revalidate()				//刷新
		{
			m_position = transform.position;
			m_precedingControlPointPosition = transform.TransformPoint(m_precedingControlPointLocalPosition);//定位点的位置为定位点的localPosition
			m_followingControlPointPosition = transform.TransformPoint(m_followingControlPointLocalPosition);//跟随点的位置为跟随点的localPosition

			transform.hasChanged = false;
		}

		public void Reset()
		{
			localPosition = Vector3.zero;
			localRotation = Quaternion.identity;
			localScale = Vector3.one;

			precedingControlPointLocalPosition = Vector3.left;
			followingControlPointLocalPosition = Vector3.right;

			transform.hasChanged = true;
		}
	}
}