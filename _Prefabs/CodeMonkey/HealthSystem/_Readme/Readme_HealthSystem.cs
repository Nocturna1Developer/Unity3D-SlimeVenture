/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Thanks!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using UnityEngine;

namespace CodeMonkey.HealthSystemCM {

    //[CreateAssetMenu(fileName = "Readme", menuName = "CodeMonkey/HealthSystem/ReadMe", order = 1)]
    public class Readme_HealthSystem : ScriptableObject {

        public Texture2D codeMonkeyHeader;
        public string title;
        public string titlesub;
        public bool loadedLayout;
        public Section[] sections;

        [Serializable]
        public class Section {
            public string heading, linkText, url;
            public string[] textLines;
        }

        /*
           * Hi there!
           * Here's a simple Health System, easy to use and fully featured.
           * Apply to the Player, Enemies, Objects or anything that you want to have Health.
           * 
           * 
           * Video Tutorial
           * - How To + Code Walkthrough
           * - https://www.youtube.com/watch?v=BHoqVb7psno
           * 
           * How To Use
           * - There are two ways to use it
           * - 1. Create a HealthSystem through code using the C# Constructor
           * - 2. Add HealthSystemComponent to any GameObject
           * - Then interact with it through the simple functions or the built-in scripts
           * - Also you can make your own HealthBar or use the included ones
           * 
           * 
           * I hope this Asset helps you in your projects!
           * If you find it useful please write a review on the Asset Store page, I'd love to hear your thoughts!
           * Best of luck with your games!
           * - Code Monkey
           * https://youtube.com/c/CodeMonkeyUnity

         * */
    }

}