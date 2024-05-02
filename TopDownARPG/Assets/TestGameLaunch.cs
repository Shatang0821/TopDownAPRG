using FrameWork.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SK
{
    public class TestGameLaunch : MonoBehaviour
    {
        private void Awake()
        {
            UIManager.Instance.ShowUI("UILogin");
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
