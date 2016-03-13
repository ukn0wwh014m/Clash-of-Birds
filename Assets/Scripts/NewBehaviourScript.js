﻿var normalCollisionCount = 1;
var spring = 50.0;
var damper = 5.0;
var drag = 10.0;
var angularDrag = 5.0;
var distance = 0.2;
var throwForce = 500;
var throwRange = 1000;
var attachToCenterOfMass = false;
 
private var springJoint : SpringJoint;
 
function Update ()
{
	// Make sure the user pressed the mouse down
	if (!Input.GetMouseButtonDown (0))
		return;
 
	var mainCamera = FindCamera();
 
	// We need to actually hit an object
	var hit : RaycastHit;
	if (!Physics.Raycast(mainCamera.gameObject.GetComponent(Camera).ScreenPointToRay(Input.mousePosition),  hit, 100))
		return;
	// We need to hit a rigidbody that is not kinematic
	if (!hit.rigidbody || hit.rigidbody.isKinematic)
		return;
 
	if (!springJoint)
	{
		var go = new GameObject("Rigidbody dragger");
		var body : Rigidbody = go.AddComponent.<Rigidbody>() as Rigidbody;
		springJoint = go.AddComponent.<SpringJoint>();
		body.isKinematic = true;
	}
 
	springJoint.transform.position = hit.point;
	if (attachToCenterOfMass)
	{
		var anchor = transform.TransformDirection(hit.rigidbody.centerOfMass) + hit.rigidbody.transform.position;
		anchor = springJoint.transform.InverseTransformPoint(anchor);
		springJoint.anchor = anchor;
	}
	else
	{
		springJoint.anchor = Vector3.zero;
	}
 
	springJoint.spring = spring;
	springJoint.damper = damper;
	springJoint.maxDistance = distance;
	springJoint.connectedBody = hit.rigidbody;
 
	StartCoroutine ("DragObject", hit.distance);
}
 
function DragObject (distance : float)
{
	var oldDrag = springJoint.connectedBody.drag;
	var oldAngularDrag = springJoint.connectedBody.angularDrag;
	springJoint.connectedBody.drag = drag;
	springJoint.connectedBody.angularDrag = angularDrag;
	var mainCamera = FindCamera();
	while (Input.GetMouseButton (0))
	{
		var ray = mainCamera.gameObject.GetComponent(Camera).ScreenPointToRay (Input.mousePosition);
		springJoint.transform.position = ray.GetPoint(distance);
		yield;
 
		if (Input.GetMouseButton (1)){
		    springJoint.connectedBody.AddExplosionForce(throwForce,mainCamera.transform.position,throwRange);
		    springJoint.connectedBody.drag = oldDrag;
		    springJoint.connectedBody.angularDrag = oldAngularDrag;
		    springJoint.connectedBody = null;
		}
	}
	if (springJoint.connectedBody)
	{
		springJoint.connectedBody.drag = oldDrag;
		springJoint.connectedBody.angularDrag = oldAngularDrag;
		springJoint.connectedBody = null;
	}
}
 
function FindCamera ()
{
	if (GetComponent.<Camera>())
		return GetComponent.<Camera>();
	else
		return Camera.main;
}
