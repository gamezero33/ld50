using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

	[SerializeField] private Vector3 m_Offset = Vector3.zero;

	private Camera m_Camera;

	[EditorButton]
	private void Update ()
	{
		if ( !m_Camera ) m_Camera = Camera.main;

		transform.LookAt( m_Camera.transform );
		transform.eulerAngles += m_Offset;
	}

}
