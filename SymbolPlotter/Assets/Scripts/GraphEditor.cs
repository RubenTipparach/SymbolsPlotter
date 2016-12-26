using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphEditor : MonoBehaviour {

	[SerializeField]
	GameObject sphereTemplate;

	[SerializeField]
	LineRenderer lineTemplate;

	[SerializeField]
	Text modeType;

	LineRenderer lineCreation;

	bool inAddLineProc = false;

	public Mode currentMode = Mode.AddLine;

	// Use this for initialization
	void Start () {
		modeType.text = currentMode.ToString();
	}
	
	public void SwitchMode(string mode)
	{
		if (mode == "AddLine")
		{
			currentMode = Mode.AddLine;
        }
		else if (mode == "AddLinePlus")
		{
			currentMode = Mode.AddLinePlus;
		}
		else
		{
			inAddLineProc = false;
		}

		if (mode == "AddSphere")
		{

			currentMode = Mode.AddSphere;
		}
		if (mode == "AddSpherePlus")
		{
			currentMode = Mode.AddSpherePlus;
		}

		if (mode =="Clear")
		{
			foreach (Transform t in gameObject.transform)
			{
				// destroy all child
				Destroy(t.gameObject);
			}
		}

		modeType.text = currentMode.ToString();
    }

	// Update is called once per frame
	void Update () {

		var camera = Camera.main;

        if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				Transform objectHit = hit.transform;

				// Do something with the object that was hit by the raycast.
				if (objectHit.gameObject.GetComponent<Clickable>() != null)
				{
					if (currentMode == Mode.AddLine)
					{
						Debug.Log("AddingLine");
						if (!inAddLineProc)
						{
							inAddLineProc = true;
							var line = GameObject.Instantiate(lineTemplate.gameObject);
							line.transform.SetParent(gameObject.transform);
                            line.SetActive(false);

							lineCreation = line.GetComponent<LineRenderer>();
							lineCreation.SetPosition(0, objectHit.position);
						}
						else
						{
							inAddLineProc = false;
							lineCreation.SetPosition(1, objectHit.position);
							lineCreation.gameObject.SetActive(true);
						}
					}
					if (currentMode == Mode.AddLinePlus)
					{
						Debug.Log("AddingLine");
						if (!inAddLineProc)
						{
							inAddLineProc = true;
							var line = GameObject.Instantiate(lineTemplate.gameObject);
							line.transform.SetParent(gameObject.transform);
							line.SetActive(false);

							lineCreation = line.GetComponent<LineRenderer>();
							lineCreation.SetPosition(0, objectHit.position + Vector3.up * .5f);
						}
						else
						{
							inAddLineProc = false;
							lineCreation.SetPosition(1, objectHit.position + Vector3.up * .5f);
							lineCreation.gameObject.SetActive(true);
						}
					}
					else if(currentMode == Mode.AddSphere)
					{
						var sphere = GameObject.Instantiate(sphereTemplate, objectHit.position, objectHit.rotation);
						sphere.transform.SetParent(gameObject.transform);
					}
					else if(currentMode == Mode.AddSpherePlus)
					{
						var sphere = GameObject.Instantiate(sphereTemplate, objectHit.position + Vector3.up * .5f, objectHit.rotation);
						sphere.transform.SetParent(gameObject.transform);
					}
				}
			}
		}
	}

	public enum Mode
	{
		AddLine,
		AddLinePlus,
		AddSphere,
		AddSpherePlus,
		Delete
	}
}
