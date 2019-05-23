using UnityEngine;
using System.Collections;

public class ReactorCore : MonoBehaviour {
	public GameObject ControlRod;
	public Transform UpPosition;
	public Transform DownPosition;
	public AudioSource SelectSound;
		public Light CoreGlow;
	public Color NominalColor;
	public Color StabilizingColor;
	public Color DestabilizingColor;
	public Color VentingColor;
	public Color RodFailureColor;
	
	public enum RodStatus
	{
		Up,
		MovingUp,
		Selected,
		MovingDown,
		Down
	}
	
	public RodStatus ControlState = RodStatus.Up;
	float VentingTime = 0;
	bool venting = false;
	
	public int TimesFixed = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		ProcessCoreState();
	}
	
	void OnEnable() {
		
	}
	
	void OnDisable() {
		
	}
	
	void OnMouseUp() {
		if (null != SelectSound)
		{
			SelectSound.PlayOneShot(SelectSound.clip);
		}
		
		switch (ControlState)
		{
		case RodStatus.Up:
			ControlState = RodStatus.MovingDown;
			if (venting)
			{
				venting = false;
				VentingTime = 0;
			}
			else
				TimesFixed += 1;
			break;
			case RodStatus.MovingUp:
			ControlState = RodStatus.MovingDown;
			if (venting)
			{
				venting = false;
				VentingTime = 0;
			}
			else
				TimesFixed += 1;
			break;
		default:
			break;
		}
	}
	
	void ProcessCoreState()
	{
			switch(ControlState)
		{
		case RodStatus.Up:
			if (null != CoreGlow)
			{
				CoreGlow.color = Color.Lerp (CoreGlow.color, VentingColor, Time.deltaTime);
			}
			if (venting)
			{
				VentingTime += Time.deltaTime;
			
				if (VentingTime >= 2.0f)
				{
					venting = false;
					// The code to do something when it's been venting for 2 seconds should go here.
				
				}
			}
			break;
			
		case RodStatus.MovingDown:
			if (null != CoreGlow)
			{
				CoreGlow.color = Color.Lerp (CoreGlow.color, StabilizingColor, Time.deltaTime);
			}
			if (!ControlRod.transform.localPosition.Equals(DownPosition.localPosition))
			{
				ControlRod.transform.localPosition = Vector3.Lerp (ControlRod.transform.localPosition, DownPosition.localPosition, Time.deltaTime);
				float distance = Vector3.Distance(ControlRod.transform.localPosition, DownPosition.localPosition);
				if (distance <= 0.025f)
				{
					ControlRod.transform.localPosition = DownPosition.localPosition;
					ControlRod.transform.localPosition = new Vector3(DownPosition.localPosition.x, DownPosition.localPosition.y, DownPosition.localPosition.z);
				}
				
			}
			else
			{
				ControlState = RodStatus.Down;
			}
			break;
			
		case RodStatus.Selected:
			if (null != CoreGlow)
			{
				CoreGlow.color = Color.Lerp (CoreGlow.color, DestabilizingColor, Time.deltaTime);
			}
			ControlState = RodStatus.MovingUp;
			ControlRod.transform.localPosition = Vector3.Lerp (ControlRod.transform.localPosition, UpPosition.localPosition, Time.deltaTime);
			break;
			
		case RodStatus.MovingUp:
			if (null != CoreGlow)
			{
				CoreGlow.color = Color.Lerp (CoreGlow.color, RodFailureColor, Time.deltaTime);
			}
			
			if (!ControlRod.transform.localPosition.Equals(UpPosition.localPosition))
			{
				ControlRod.transform.localPosition = Vector3.Lerp (ControlRod.transform.localPosition, UpPosition.localPosition, Time.deltaTime);
				float distance = Vector3.Distance(ControlRod.transform.localPosition, UpPosition.localPosition);
				if (distance <= 0.025f)
				{
					ControlRod.transform.localPosition = UpPosition.localPosition;
					ControlRod.transform.localPosition = new Vector3(UpPosition.localPosition.x, UpPosition.localPosition.y, UpPosition.localPosition.z);
				}
			}
			else
			{
				ControlState = RodStatus.Up;
				Debug.Log ("Rod is up");
				venting = true;
			}
			break;
			
		default:
			if (null != CoreGlow)
			{
				CoreGlow.color = Color.Lerp (CoreGlow.color, NominalColor, Time.deltaTime);
			}
			break;
		}
	
	}


}
