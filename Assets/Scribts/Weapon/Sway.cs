using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{

    #region Variables

    public float intensity;
    public float smooth;

    private Quaternion originRotation;

    #endregion

    #region Monobehaviour CallBacks

    private void Start()
    {
        originRotation = transform.localRotation;
    }

    private void Update()
    {
        if(!PlayerManager.instance.deadPlayer || !PlayerManager.instance.gamePaused)
        UpdateSway();
    }

    #endregion

    #region Private Methods

    private void UpdateSway()
    {
        float xMouse = Input.GetAxisRaw("Mouse X");
        float yMouse = Input.GetAxisRaw("Mouse Y");

        Quaternion xAdj = Quaternion.AngleAxis(-intensity * xMouse, Vector3.up);
        Quaternion yAdj = Quaternion.AngleAxis(intensity * yMouse, Vector3.up);

        Quaternion targetRotattion = originRotation * xAdj *yAdj;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotattion, Time.deltaTime * smooth);
    }

    #endregion
}
