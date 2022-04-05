using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenColor : Tweener
{

	[SerializeField]
	private Color m_StartColor = Color.white;

	[SerializeField]
	private Color m_EndColor = Color.black.ToAlpha( 0.5f );

	private Image m_Image;
	private Text m_Text;
	private TMPro.TextMeshProUGUI m_TextMeshProUGUI;
	private Light m_Light;

	private void Awake ()
	{
		m_Image = GetComponent<Image>();
		m_Text = GetComponent<Text>();
		m_TextMeshProUGUI = GetComponent<TMPro.TextMeshProUGUI>();
		m_Light = GetComponent<Light>();
	}

	protected override void UpdateTween ()
	{
		if ( !m_Image )
			m_Image = GetComponent<Image>();
		if ( !m_Text )
			m_Text = GetComponent<Text>();
		if (!m_TextMeshProUGUI)
			m_TextMeshProUGUI = GetComponent<TMPro.TextMeshProUGUI>();
		if ( !m_Light )
			m_Light = GetComponent<Light>();

		Color color = Color.LerpUnclamped( m_StartColor, m_EndColor, Factor );
		if ( m_Image )
			m_Image.color = color;
		if ( m_Text )
			m_Text.color = color;
		if (m_TextMeshProUGUI)
			m_TextMeshProUGUI.color = color;
		if ( m_Light )
			m_Light.color = color;
	}

}