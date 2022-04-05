using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenSpacing : Tweener
{

	[SerializeField]
	private float m_StartSpacing = 0;

	[SerializeField]
	private float m_EndSpacing = 10;

	private HorizontalLayoutGroup m_HorizontalLayoutGroup;

	protected override void UpdateTween ()
	{
		if ( !m_HorizontalLayoutGroup )
			m_HorizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();

		m_HorizontalLayoutGroup.spacing = Mathf.Lerp( m_StartSpacing, m_EndSpacing, Factor );
	}

}