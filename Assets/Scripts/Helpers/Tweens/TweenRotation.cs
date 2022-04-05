using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenRotation : Tweener
{

	[SerializeField]
	private Vector3 m_StartRotation = Vector3.zero;

	[SerializeField]
	private Vector3 m_EndRotation = Vector3.zero;

	[SerializeField]
	private bool m_LocalSpace;


	protected override void UpdateTween ()
	{
		if ( m_LocalSpace )
			transform.localEulerAngles = Vector3.LerpUnclamped( m_StartRotation, m_EndRotation, Factor );
		else
			transform.eulerAngles = Vector3.LerpUnclamped( m_StartRotation, m_EndRotation, Factor );
	}

	[ContextMenu( "Set Start from Current" )]
	private void SetStartFromCurrent ()
	{
		if ( m_LocalSpace )
			m_StartRotation = transform.localEulerAngles;
		else
			m_StartRotation = transform.eulerAngles;
	}

	[ContextMenu( "Set End from Current" )]
	private void SetEndFromCurrent ()
	{
		if ( m_LocalSpace )
			m_EndRotation = transform.localEulerAngles;
		else
			m_EndRotation = transform.eulerAngles;
	}


}