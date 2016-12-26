using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGridRenderer : MonoBehaviour
{

	[SerializeField]
	private int _gridSize;

	[SerializeField]
	private float _gridSpacing;

	[SerializeField]
	public Transform _trackingObject;

	[SerializeField]
	public Transform _tacticalMarker;

	[SerializeField]
	private GameObject _lineRedererObj;

	private GameObject[] _lines;

	private GameObject[] _girdPlaneLines;

	//------------------ These are velocity trackers.
	private Vector3 _currentPosition;

	private Vector3 _lastPosition;

	private Vector3 _currentVector;

	private Vector3 _customVelocity;

	private Rigidbody _trackingRigidBody;

	private bool _useCustomVelcity = true;

	bool _useFixedValue = true;

	float _nextActionTime1 = 0;

	[SerializeField]
	float _vectorLineSizeFactor = 50;

	[SerializeField]
	int _vectorLineGridSize = 5;

	public enum DisplayMode
	{
		Empty,
		Grid,
		GridAndShipHeight
	}

	public DisplayMode displayMode;

	public int yOffset = 0;

	

	// Use this for initialization
	void Start()
	{

		if (displayMode == DisplayMode.Grid)
		{
			_lines = new GameObject[(_gridSize + 1) * 2];
			for (int i = 0; i < (_gridSize + 1) * 2; i++) // for the sides and stuff.
			{
				_lines[i] = GameObject.Instantiate(_lineRedererObj);
				_lines[i].transform.SetParent(this.transform);
			}
		}
		else
		{
			int counter = 0;
			// tripple trouble yall!
			_lines = new GameObject[_vectorLineGridSize * _vectorLineGridSize * _vectorLineGridSize];
			for (int i = 0; i < _vectorLineGridSize; i++)
				for (int j = 0; j < _vectorLineGridSize; j++)
					for (int k = 0; k < _vectorLineGridSize; k++)
					{
						_lines[counter] = GameObject.Instantiate(_lineRedererObj);
						_lines[counter].transform.SetParent(this.transform);
						counter++;
					}
		}

		DrawGrid();

	}

	// Update is called once per frame
	void FixedUpdate()
	{

		//if (_trackingObject == null) return;

		//_currentPosition = _trackingObject.position;


		// we'll use custom velocity.
		if(_useFixedValue)
		{
			_currentVector = Vector3.one;

			_lastPosition = Vector3.zero;
		}
		else if (_useCustomVelcity)
		{
			_currentVector = (_currentPosition - _lastPosition) / Time.deltaTime;
			// always last.
			_lastPosition = _trackingObject.transform.position;

		}
		else
		{
			_currentVector = _trackingRigidBody.velocity / 10;
			// always last.
			_lastPosition = _trackingObject.transform.position;
		}

		//DrawVectorGrid();
    }

	/// <summary>
	/// Draws the acceleration vector grid.
	/// </summary>
	private void DrawVectorGrid()
	{

		int counter = 0;
		for (int k = 0; k < _vectorLineGridSize; k++)
		{
			Vector3 upOffset = Vector3.up * (k * _vectorLineSizeFactor - _vectorLineSizeFactor * (_vectorLineGridSize / 2));
			for (int j = 0; j < _vectorLineGridSize; j++)
			{
				Vector3 sideOffset = Vector3.left * (j * _vectorLineSizeFactor - _vectorLineSizeFactor * (_vectorLineGridSize / 2)) + upOffset;

				for (int i = 0; i < _vectorLineGridSize; i++)
				{
					float localRound = 1f / _vectorLineSizeFactor;

					Vector3 localShipArea = new Vector3(
						Mathf.Round(_lastPosition.x * localRound),
						Mathf.Round(_lastPosition.y * localRound),
						Mathf.Round(_lastPosition.z * localRound));

					Vector3 forwardOffset = Vector3.back * (i * _vectorLineSizeFactor - _vectorLineSizeFactor * (_vectorLineGridSize / 2)) + localShipArea * _vectorLineSizeFactor + sideOffset;

					_lines[counter].GetComponent<LineRenderer>().SetPosition(0, forwardOffset);// DrawLine(forwardOffset, -_currentVector + forwardOffset);
					_lines[counter].GetComponent<LineRenderer>().SetPosition(1, forwardOffset - _currentVector);

					//Debug.Log(counter + " " + forwardOffset);
					counter++;
				}
			}
		}
	}

	/// <summary>
	/// Draws the grid. This method is used for grid only!
	/// </summary>
	private void DrawGrid()
	{
		//if (_tacticalMarker == null)
		//{
		//	return;
		//}

			Vector3 currentPos = _tacticalMarker.position - Vector3.up * _tacticalMarker.position.y;
		//Vector3 currentPos = Vector3.forward;
		int counter = 0;
		// Removed Y axis since it got too cluttered
		for (int i = 0; i <= _gridSize; i++)
		{
			float localRound = 1f / _gridSpacing;
			//Debug.Log(localRound);
			Vector3 localShipArea = new Vector3(
				Mathf.Round(currentPos.x * localRound),
				Mathf.Round(currentPos.y * localRound),
				Mathf.Round(currentPos.z * localRound));

			Vector3 forwardOffset = Vector3.left * (i * _gridSpacing - _gridSpacing * (_gridSize / 2)) + localShipArea * _gridSpacing;

			//GLLines.LineDrawing(
			//	_lineMaterial,
			//	Color.green,
			//	forwardOffset + Vector3.forward * _gridSpacing * _gridSize / 2,
			//	-Vector3.forward * _gridSpacing * _gridSize / 2 + forwardOffset + Vector3.forward);
			_lines[counter].GetComponent<LineRenderer>().SetPosition(
				0,
				forwardOffset + Vector3.forward * _gridSpacing * _gridSize / 2  + Vector3.up * yOffset);
			_lines[counter].GetComponent<LineRenderer>().SetPosition(
				1,
				-Vector3.forward * _gridSpacing * _gridSize / 2 + forwardOffset + Vector3.up * yOffset);

			counter++;
		}

	

		for (int i = 0; i <= _gridSize; i++)
		{
			float localRound = 1f / _gridSpacing;

			Vector3 localShipArea = new Vector3(
				Mathf.Round(currentPos.x * localRound),
				Mathf.Round(currentPos.y * localRound),
				Mathf.Round(currentPos.z * localRound));

			Vector3 forwardOffset = Vector3.back * (i * _gridSpacing - _gridSpacing * (_gridSize / 2)) + localShipArea * _gridSpacing; // + sideOffset;

			//		GLLines.LineDrawing(
			// _lineMaterial,
			// Color.green,
			// forwardOffset + Vector3.right * _gridSpacing * _gridSize / 2,
			// (-Vector3.right * _gridSpacing * _gridSize / 2 + forwardOffset));
			_lines[counter].GetComponent<LineRenderer>().SetPosition(
				0,
				forwardOffset + Vector3.right * _gridSpacing * _gridSize / 2 + Vector3.up * yOffset);
			_lines[counter].GetComponent<LineRenderer>().SetPosition(
				1,
				-Vector3.right * _gridSpacing * _gridSize / 2 + forwardOffset + Vector3.up * yOffset);

			counter++;
		}

	}
}
