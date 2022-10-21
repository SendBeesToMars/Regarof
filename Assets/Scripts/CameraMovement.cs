using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.U2D;

public class CameraMovement : MonoBehaviour {

    private byte ppu;

    public new PixelPerfectCamera camera;
    public Transform target;
    public float smoothing;


    // Start is called before the first frame update
    void Start() {
        ppu = (byte)camera.assetsPPU;
    }

    // Update is called once per frame
    void LateUpdate() {
        camera.assetsPPU = Input.GetKey(KeyCode.LeftShift) ? ppu/2 : (Input.GetKey(KeyCode.LeftControl) ? ppu*4 : ppu);

        if(transform.position != target.position) {
            Vector3 target_position = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, target_position, smoothing);
        }
    }
}
