// using Game.Collectable.Impl;
using Services.Particles;
using Services.Particles.Service;
using Services.Pool;
using UnityEngine;
using Zenject;

namespace Services.Money.Impl
{
    public class MoneyDrop : MonoBehaviour, IMoneyDrop
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;
        // [SerializeField] private CollectableEventSender _collectable;
        [SerializeField] private Animator _animator;
        [Inject] private IMoneyService _moneyService;
        [Inject] private IParticlesService _particlesService;
        
        private bool _isActive;
        private IPool<IMoneyDrop> _pool;

        public int MoneyCount { get; set; }
        
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                gameObject.SetActive(_isActive);
            } }
        
        public void InitPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void Drop(Vector3 dropForce)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(dropForce, ForceMode.Impulse);
        }

        public Vector3 Position => transform.position;


        public void Init(IPool<IMoneyDrop> pool)
        {
            _pool = pool;
        }

        public void CollectBack()
        {
            gameObject.SetActive(false);
            _pool.Return(this);
        }
        
        public void OnCollectAnimEvent()
        {
            _moneyService.AddMoney(MoneyCount);
            _pool.Return(this);
            gameObject.SetActive(false);
           var particles = _particlesService.PlayParticles(new PlayParticlesArg()
            {
                name = ParticlesNames.DollarBlast, 
                position = transform.position, 
                duration = 1.5f, 
                timed =  true
                
            });
           particles.Play();
        }
        
        private void OnEnable()
        {
            // _collectable.OnCollect += OnCollect;
        }

        private void OnDisable()
        {
            // _collectable.OnCollect -= OnCollect;
        }

        private void OnCollect()
        {
            if (_isActive == false)
                return;
            _isActive = false;
            _animator.Play("Collect");
        }
        
        
    }
}