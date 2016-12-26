using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPillar : MonoBehaviour {

	private GameObject[] _lines;

	[SerializeField]
	private int GridSize = 0;

	[SerializeField]
	private float spacing;

	[SerializeField]
	private float height;

	[SerializeField]
	GameObject _lineTemplate;

	// Use this for initialization
	void Start ()
	{
		_lines = new GameObject[GridSize * GridSize];

		int k = 0;

		for(int i = 0; i < GridSize * GridSize; i++)
		{
			if (i % GridSize == 0)
			{
				k++;
			}

			_lines[i] = GameObject.Instantiate(_lineTemplate);
			_lines[i].transform.SetParent(this.transform);

			_lines[i].GetComponent<LineRenderer>().SetPosition(0, new Vector3(k - GridSize / 2 - 1, 0, i% GridSize - GridSize / 2 ));
			_lines[i].GetComponent<LineRenderer>().SetPosition(1, new Vector3(k - GridSize / 2 - 1, height, i % GridSize - GridSize / 2));
		}
    }
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
