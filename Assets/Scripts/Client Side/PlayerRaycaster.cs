using UnityEngine;

namespace Client_Side
{
    public class PlayerRaycaster : MonoBehaviour
    {
        private const float SightLimit = 500f;

        [SerializeField] private Transform gun;
        [SerializeField] private Rigidbody bulletPrefab;

        private void Start()
        {
            InvokeRepeating(nameof(ShootBullet), 0f, 0.1f);
        }

        private void ShootBullet()
        {
            var newBullet = Instantiate(bulletPrefab, gun.transform.position, Quaternion.identity);
            newBullet.velocity = gun.forward * 50f;
            Destroy(newBullet.gameObject, 10f);
        }

        private void OnDrawGizmos()
        {
            var hit = Physics.Raycast(transform.position, transform.forward, out RaycastHit info, SightLimit);

            Gizmos.color = hit ? Color.green : Color.red;

            if (hit)
            {
                Gizmos.DrawLine(transform.position, info.point);
                Gizmos.DrawSphere(info.point, 0.1f);
                gun.LookAt(info.point);
            }
            else
            {
                Gizmos.DrawRay(transform.position, transform.forward);
                gun.LookAt(transform.position + transform.forward * SightLimit);
            }
        }
    }
}