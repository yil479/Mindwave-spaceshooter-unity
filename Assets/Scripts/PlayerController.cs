using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float fireRate = 0.5F;
	public float speed;
	public float tilt;
	public Boundary boundary;
	public GameObject shot;
	public Transform shotSpawn;
    private float AttentionLevel;
    private float MediLevel;
    private float BlinkLevel;
    private float myTime = 0.0F;
	private float nextFire = 0.5F;
	private Rigidbody rb;
	private AudioSource audioSource;
    

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
        MindwaveManager.Instance.Controller.OnUpdateMindwaveData += OnUpdateMindwaveData;
        MindwaveManager.Instance.Controller.OnUpdateBlink += OnUpdateBlink;
        // TESTING
        // You can use the "BrainwaveTester" component to test the MindblastTrigger behavior.
        // If you do so, comment the previous line, and uncomment this following one.
        //GetComponentInChildren<BrainwaveTester>().OnUpdateMindwaveData += OnUpdateMindwaveData;

    }
    public void OnUpdateMindwaveData(MindwaveDataModel _Data)
    {

        AttentionLevel = _Data.eSense.attention;
        MediLevel = _Data.eSense.meditation;


    }
    public void OnUpdateBlink(int _BlinkStrength)
    {

        BlinkLevel = _BlinkStrength;

    }

    private void Update()
    {
        myTime = myTime + Time.deltaTime;
        fireRate = MediLevel * 0.01f;
        Debug.Log("firerate：" + fireRate);
        Debug.Log("Meditation：" + MediLevel);
        Debug.Log("attention：" + AttentionLevel);
        Debug.Log("Blink：" + BlinkLevel);
        if (AttentionLevel >= 30.0f && myTime > nextFire)
        {
            nextFire = myTime + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

            audioSource.Play();

            nextFire = nextFire - myTime;
            myTime = 0.0F;

        }
    }
    void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
        speed = BlinkLevel * 0.1f;
		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		rb.velocity = movement * speed;

		rb.position = new Vector3(
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
		);

		rb.rotation = Quaternion.Euler(
			0.0f,
			0.0f,
			rb.velocity.x * -tilt
		);
	}
}
