using UnityEngine;
using System.Collections;

public class Explode : MonoBehaviour {
  ParticleSystem particles;
	// Use this for initialization
  void Start() {
    particles = GetComponent<ParticleSystem>();
  }
	public void Boom() {
    particles.Play();
    Invoke("destroySelf", 5);
  }

  void destroySelf() {
    Destroy(transform.parent.gameObject);
  }
}
