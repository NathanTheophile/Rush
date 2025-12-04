using System;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    #region ______________________________/ VALUES
    [SerializeField] private GameObject _Explosion;
    [SerializeField] private Transform _Container;
    [SerializeField] private GameObject _Canvas;

    private float _Speed = 20f;
    private float _RotationSpeed = 90f;
    private float _TiltSpeed = 60f;

    private float _Up, _Lat;

    public event Action OnCollectibleHit;
    public event Action OnPlaneCrashed;

    #endregion


    #region ______________________________/ METHODS

    // Update is called once per frame
    void Update()
    {
        HandlingMovement();
    }

    private void HandlingMovement()
    {
        _Lat = -Input.GetAxis("Horizontal") * _RotationSpeed * Time.deltaTime;
        _Up = -Input.GetAxis("Vertical") * _RotationSpeed * Time.deltaTime;

        transform.Rotate(transform.forward, _TiltSpeed * _Lat * Time.deltaTime);
        
        _Container.transform.Rotate (_Up, 0, _Lat, Space.Self);


        _Container.transform.position += transform.forward * _Speed * Time.deltaTime; 
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Collectible")) OnCollectibleHit.Invoke();
        else AirplaneCrash();
    }

    private void AirplaneCrash()
    {
        Instantiate(_Explosion, transform.position, Quaternion.identity);
        OnPlaneCrashed.Invoke();
        Destroy(gameObject);
        _Canvas.SetActive(false);
    }

    #endregion
}