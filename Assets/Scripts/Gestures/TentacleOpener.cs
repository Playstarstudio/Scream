using Services;
using System;
using UnityEngine;

namespace UI
{


    public class TentacleOpener : MonoBehaviour
    {
        public GameObject panel;
        public DragGesture Tentacle_1_Gesture;
        public DragGesture Tentacle_2_Gesture;
        public DragGesture Tentacle_3_Gesture;
        public DragGesture Tentacle_4_Gesture;
        public DragGesture Tentacle_5_Gesture;
        public DragGesture Tentacle_6_Gesture;
        public DragGesture Tentacle_7_Gesture;
        public DragGesture Tentacle_8_Gesture;
        public GameObject[] Closed_Tentacles;
        public GameObject[] Open_Tentacles;
        public int currentKey = 1;
        
        private new readonly AudioManager audio;

        [Header("Game State")]
        public GameStateKey amuletRetrievedStateKey;

        // 1 4 5 8 3
        // 1 4 5 8 3 - or 0 3 4 7 2
        private void OnEnable()
        {
            Tentacle_1_Gesture.OnGestureEnd += OnTentacle_1_Gesture;
            Tentacle_2_Gesture.OnGestureEnd += OnTentacle_2_Gesture;
            Tentacle_3_Gesture.OnGestureEnd += OnTentacle_3_Gesture;
            Tentacle_4_Gesture.OnGestureEnd += OnTentacle_4_Gesture;
            Tentacle_5_Gesture.OnGestureEnd += OnTentacle_5_Gesture;
            Tentacle_6_Gesture.OnGestureEnd += OnTentacle_6_Gesture;
            Tentacle_7_Gesture.OnGestureEnd += OnTentacle_7_Gesture;
            Tentacle_8_Gesture.OnGestureEnd += OnTentacle_8_Gesture;


        }
        private void OnDisable()
        {
            Tentacle_1_Gesture.OnGestureEnd -= OnTentacle_1_Gesture;
            Tentacle_2_Gesture.OnGestureEnd -= OnTentacle_2_Gesture;
            Tentacle_3_Gesture.OnGestureEnd -= OnTentacle_3_Gesture;
            Tentacle_4_Gesture.OnGestureEnd -= OnTentacle_4_Gesture;
            Tentacle_5_Gesture.OnGestureEnd -= OnTentacle_5_Gesture;
            Tentacle_6_Gesture.OnGestureEnd -= OnTentacle_6_Gesture;
            Tentacle_7_Gesture.OnGestureEnd -= OnTentacle_7_Gesture;
            Tentacle_8_Gesture.OnGestureEnd -= OnTentacle_8_Gesture;
        }
        private void OnTentacle_1_Gesture(DragDirection dragDirection)
        {
            int index = 0;
            if (currentKey == 1 && dragDirection == DragDirection.Up)
            {
                Closed_Tentacles[0].SetActive(false);
                Open_Tentacles[0].SetActive(true);
            }
            
            // TODO: Add tentacle sounds once this is implemented
            // audio.PlayOneShot(AudioID.SFX.Player.Interact.Tentacle.pull, GameObject.Find("Character"));
            CheckTentacles(index, dragDirection);
        }

        private void OnTentacle_2_Gesture(DragDirection dragDirection)
        {
            int index = 1;
            CheckTentacles(index, dragDirection);
        }
        private void OnTentacle_3_Gesture(DragDirection dragDirection)
        {
            int index = 2;
            CheckTentacles(index, dragDirection);
        }

        private void OnTentacle_4_Gesture(DragDirection dragDirection)
        {
            throw new NotImplementedException();
            int index = 3;
            CheckTentacles(index, dragDirection);
        }

        private void OnTentacle_5_Gesture(DragDirection dragDirection)
        {
            int index = 4;
            CheckTentacles(index, dragDirection);
        }

        private void OnTentacle_6_Gesture(DragDirection dragDirection)
        {
            int index = 5;
            CheckTentacles(index, dragDirection);
        }

        private void OnTentacle_7_Gesture(DragDirection dragDirection)
        {
            int index = 6;
            CheckTentacles(index, dragDirection);
        }

        private void OnTentacle_8_Gesture(DragDirection dragDirection)
        {
            int index = 7;
            CheckTentacles(index, dragDirection);
        }
        private void CheckTentacles(int index, DragDirection dragDirection)
        {
            switch (index)
            {
                case 0:
                    if (currentKey == 0 && dragDirection == DragDirection.Up)
                        TentacleSuccess(index);
                        currentKey = 3;
                    }
                    else
                    {
                        TentacleFail();
                    }
                    break;
                case 1:
                    TentacleFail();
                    break;
                case 2:
                    if (currentKey == 0 && dragDirection == DragDirection.Up)
                    {
                        TentacleSuccess(index);
                        currentKey = 3;
                    }
                    else
                    {
                        TentacleFail();
                    }
                    break;
                case 3:
                    if (currentKey == 3 && dragDirection == DragDirection.Up)
                    {
                        TentacleSuccess(index);
                        currentKey = 4;
                    }
                    else
                    {
                        TentacleFail();
                    }
                    break;
                case 4:
                    if (currentKey == 4 && dragDirection == DragDirection.Up)
                    {
                        TentacleSuccess(index);
                        currentKey = 7;
                    }
                    else
                    {
                        TentacleFail();
                    }
                    break;
                case 5:
                    TentacleFail();
                    break;
                case 6:
                    TentacleFail();
                    break;
                case 7:
                    if (currentKey == 7 && dragDirection == DragDirection.Up)
                    {
                        TentacleSuccess(index);
                        currentKey = 2;
                    }
                    else
                    {
                        TentacleFail();
                    }
                    break;

            }
        }

        private void TentacleSuccess(int index)
        {
            Closed_Tentacles[index].SetActive(false);
            Open_Tentacles[index].SetActive(true);
        }
        private void TentacleFail()
            int index = 0;
            foreach(var tindex in Closed_Tentacles)
                index++;
            }
        }


        private void setState()
        {
            Debug.Log("success!");
            panel.SetActive(false);

            if (amuletRetrievedStateKey != null)
            {
                ServiceLocator.Instance.Get<GameStateManager>().SetState(amuletRetrievedStateKey, true);
                audio.PlayOneShot(AudioID.SFX.Player.Interact.Amulet.pick_up, GameObject.Find("Character"));
            }
        }
    }
}
