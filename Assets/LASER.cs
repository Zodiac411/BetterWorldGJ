using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LASER : MonoBehaviour
{
    [SerializeField] private LineRenderer _beam;
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private float _maxLength;
    [SerializeField] private ParticleSystem _muzzleParticles;
    [SerializeField] private ParticleSystem _hitParticles;
    [SerializeField] private float _damage; // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        _beam.enabled = false;
    }
    private void Activate()
    {
        _beam.enabled = true;

        _muzzleParticles.Play();
        _hitParticles.Play();
    }
    private void Deactivate()
    {
        _beam.enabled = false;
        _beam.SetPosition(0, _muzzlePoint.position);
        _beam.SetPosition(1,_muzzlePoint.position);
        _muzzleParticles.Stop();
        _hitParticles.Stop();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) Activate();
        else if (Input.GetMouseButtonUp(0)) Deactivate();
    }
    private void FixedUpdate()
    {
       // if(!_beam.enabled) return;

        Ray ray = new Ray(_muzzlePoint.position, _muzzlePoint.forward);
        bool cast = Physics.Raycast(ray, out RaycastHit hit, _maxLength);
        Vector3 hitPosition = cast ? hit.point : _muzzlePoint.position + _muzzlePoint.forward * _maxLength;

        _beam.SetPosition(0, _muzzlePoint.position);
        _beam.SetPosition(1, hitPosition);
        _hitParticles.transform.position = hitPosition;

        if (cast && hit.collider.TryGetComponent(out IDamageable damageable))
        {
            DamageContext context = new DamageContext(
                _damage * Time.deltaTime,
                gameObject,
                hit.point,
                hit.normal);
            damageable.ApplyDamage(in context);
        }
    }
}
