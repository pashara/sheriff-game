using System;
using NaughtyCharacter;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace Sheriff.ClientServer.Players
{
    public class DummyPlayer : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private GameObject playerView;
        [SerializeField] private GameObject view;
        private GameObject _realView = null;
        
        [Inject]
        private void Construct(DiContainer container)
        {
            // _realView = container.InstantiatePrefab(playerView, transform.position, transform.rotation, transform.parent);
            //

            container.InjectGameObject(view);
            view.transform.parent = transform.parent;
            _realView = view;
            
            if (photonView.IsMine)
            {
                _realView.GetComponent<PlayerController>().enabled = true;
                _realView.GetComponent<CharacterController>().enabled = true;
            }
            //
        }


        private void Update()
        {
            if (_realView == null)
                return;
            
            if (photonView.IsMine)
            {
                transform.position = _realView.transform.position;
                transform.rotation = _realView.transform.rotation;
            }
            else
            {
                _realView.transform.position = transform.position;
                _realView.transform.rotation = transform.rotation;
            }
        }
    }
}