using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenScale : Tweener
{

	[SerializeField]
	private Vector3 m_StartScale = Vector3.one;

	[SerializeField]
	private Vector3 m_EndScale = Vector3.one;


	protected override void UpdateTween ()
	{
		transform.localScale = Vector3.LerpUnclamped( m_StartScale, m_EndScale, Factor );
	}

}