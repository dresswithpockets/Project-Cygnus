using UnityEngine;
using System.Collections;

public class ShortSword : AWeapon {

	public override string Name {
		get {
			return "Short Sword";
		}
	}

	public override WeaponHandiness Handiness {
		get {
			return WeaponHandiness.ONE_HANDED;
		}
	}

	GameObject RenderedModel;
	BoxCollider ModelCollider;

	public bool Animating = false;
	float TimeSinceAnimate = 0.0f;
	float AnimationLength = 0.25f;

	Vector3 StartingAnimPos = new Vector3(1.25f, 0f, 0f);
	Vector3 StartingRotation = new Vector3(0f, 90f, 0f);

	Vector3 InactivePosition = new Vector3(0.55f, -0.25f, 0f);
	Vector3 InactiveRotation = new Vector3(60f, 180f, 90f);

	public float Damage = 10f;

	public override void Start() {
		RenderedModel = transform.FindChild("Render").gameObject;
		ModelCollider = RenderedModel.GetComponent<BoxCollider>();
	}

	public override void Update() {
		if (Active) { // Weapon is equipped on hand

			if (Animating) {

				ModelCollider.enabled = true;

				if (TimeSinceAnimate > AnimationLength) {
					TimeSinceAnimate = 0f;
					Animating = false;
				}
				else {
					Vector3 newRot = transform.parent.eulerAngles;
					newRot.y += -145f;
                    transform.rotation = Quaternion.Lerp(transform.parent.rotation, Quaternion.Euler(newRot), TimeSinceAnimate / AnimationLength);

					TimeSinceAnimate += Time.deltaTime;
				}
			}
			else {

				ModelCollider.enabled = false;

				transform.rotation = transform.parent.rotation;
				RenderedModel.transform.localPosition = InactivePosition;
				RenderedModel.transform.localEulerAngles = InactiveRotation;

				if (vp_Input.GetButtonAny("Attack1")) {

					RenderedModel.transform.localPosition = StartingAnimPos;
					RenderedModel.transform.localEulerAngles = StartingRotation;

					/*StartCoroutine(
						AnimateNormalAttack(transform.parent.position, Vector3.up, -145f, 0.25f)
						);*/

					Animating = true;
					Using = true;
				}
			}
		}
	}

	public IEnumerator AnimateNormalAttack(Vector3 point, Vector3 axis,
					  float rotateAmount, float rotateTime) {

		float step = 0.0f; //non-smoothed
		float rate = 1.0f/rotateTime; //amount to increase non-smooth step by
		float smoothStep = 0.0f; //smooth step this time
		float lastStep = 0.0f; //smooth step last time
		while (step < 1.0) { // until we're done
			step += Time.deltaTime * rate; //increase the step
			smoothStep = Mathf.SmoothStep(0.0f, 1.0f, step); //get the smooth step
			transform.RotateAround(point, axis,
								   rotateAmount * (smoothStep - lastStep));
			lastStep = smoothStep; //store the smooth step
			yield return null;
		}
		//finish any left-over
		if (step > 1.0)
			transform.RotateAround(point, axis,
								   rotateAmount * (1.0f - lastStep));

		Animating = false;
		Using = false;
	}

	void OnTriggerEnter(Collider other) {
		//other.SendMessage("Damage", new DamageInfo(DamageType.PHYSICAL, DamageElement.NONE, Damage, Owner), SendMessageOptions.DontRequireReceiver);
		if (other.tag != "Player") {
			if (other.GetComponent<vp_DamageHandler>() != null) other.SendMessage("Damage", new vp_DamageInfo(Damage, Owner.transform), SendMessageOptions.DontRequireReceiver);
			else if (other.GetComponent<Pawn>() != null) other.SendMessage("Damage", new DamageInfo(DamageType.PHYSICAL, DamageElement.NONE, Damage, Owner));
		}
	}
}
