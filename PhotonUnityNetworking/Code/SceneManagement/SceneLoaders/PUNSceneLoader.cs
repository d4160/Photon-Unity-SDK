namespace d4160.SceneManagement
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class PunSceneLoader : DefaultEmptySceneLoader
    {
        public override bool NetworkingSyncLoad { get; } = true;
    }
}