using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2020
{
    public class QuitButton : MonoBehaviour
    {
        public void Quit()
        {
            GameManager.Quit();
        }
    }
}
