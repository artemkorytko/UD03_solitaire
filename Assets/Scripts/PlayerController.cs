using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerController : MonoBehaviour
    {
        private PlayingCard _holdCard;
        private Camera _camera;
        private LayerMask _layerMask;

        public event Action<CardType, int> OnAddToMain;
        public event Action<CardType, int> OnRemoveFromMain;
        public event Action OnExcludeCurrentCard;

        private Vector3 _offset;
        private void Awake()
        {
            _camera=Camera.main;
            _layerMask=LayerMask.GetMask("PlayingCard");
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryHoldCard();
            }

            if (Input.GetMouseButton(0) && _holdCard)
            {
                MoveCard();
            }

            if (Input.GetMouseButtonUp(0) && _holdCard)
            {
                ReleasedCard();
            }
        }
        

        private void TryHoldCard()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out RaycastHit hit, float.MaxValue,_layerMask))
            {
                var playingCard = hit.collider.gameObject.GetComponent<PlayingCard>();

                if (playingCard && playingCard.IsOpen)
                {
                    _holdCard = playingCard;
                    
                    var mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                    mouseWorldPosition.z = _holdCard.transform.position.z;

                    _offset = _holdCard.transform.position - mouseWorldPosition;
                    _holdCard.transform.Translate(Vector3.back*0.5f,Space.World);
                }
            }
        }
        
        private void MoveCard()
        {
            var mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = _holdCard.transform.position.z;
            _holdCard.transform.position = mouseWorldPosition + _offset;
        }
        
        private void ReleasedCard()
        {
            Ray ray = new Ray(_holdCard.transform.position, Vector3.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _layerMask))
            {
                var cardPlace = hit.collider.gameObject.GetComponent<CardPlace>();
                if (cardPlace && cardPlace.IsCanConnect(_holdCard))
                {

                    if (cardPlace.IsMain)
                    {
                        OnAddToMain?.Invoke(_holdCard.Type,_holdCard.Value);
                    }

                    if (_holdCard.IsMain && !cardPlace.IsMain)
                    {
                        OnRemoveFromMain?.Invoke(_holdCard.Type,_holdCard.Value);
                    }

                    _holdCard.SetParent(cardPlace);
                    
                    if (_holdCard.IsInDeck)
                    {
                        OnExcludeCurrentCard?.Invoke();   
                    }
                }
                else
                {
                    _holdCard.SetParent();
                }

            }
            else
            {
                _holdCard.SetParent();
            }
            _holdCard = null;
        }
    }
}