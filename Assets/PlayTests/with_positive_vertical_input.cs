﻿using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace a_player
{
    public static class Helpers
    {
        public static IEnumerator LoadMovementTestsScene()
        {
            var operation = SceneManager.LoadSceneAsync("MovementTest");
            while (operation.isDone == false) 
                yield return null;
            
            
        }
        
        public static IEnumerator LoadItemTestScene()
        {
            var operation = SceneManager.LoadSceneAsync("ItemTests");
            while (operation.isDone == false) 
                yield return null;
            
            // LoadSceneMode.Additive will add the scene along with w/e else is there
            // Sort of like having multiple scenes in section 7
            operation = SceneManager.LoadSceneAsync("UI",LoadSceneMode.Additive);
            while (operation.isDone == false) 
                yield return null;
        }
        
        public static IEnumerator LoadEntityStateTestScene()
        {
            var operation = SceneManager.LoadSceneAsync("EntityStateMachineTests");
            while (operation.isDone == false) 
                yield return null;
        }
        
        public static IEnumerator LoadMenuScene()
        {
            var operation = SceneManager.LoadSceneAsync("Menu");
            while (operation.isDone == false) 
                yield return null;
        }
        
        public static Player GetPlayer()
        {
            Player player = GameObject.FindObjectOfType<Player>();
            
            var testPlayerInput = Substitute.For<IPlayerInput>();
            player.PlayerInput = testPlayerInput;
            return player;
        }

        public static float CalculateTurn(Quaternion originalRotation, Quaternion transformRotation)
        {
            var cross = Vector3.Cross(originalRotation * Vector3.forward, transformRotation * Vector3.forward);
            var dot = Vector3.Dot(cross, Vector3.up);
            return dot;
        }



    }
    
    public class with_positive_vertical_input
    {
        [UnityTest]
        public IEnumerator moves_forward()
        {
           yield return Helpers.LoadMovementTestsScene();

           var player = Helpers.GetPlayer();
            
            player.PlayerInput.Vertical.Returns(1f);
            //testPlayerInput.Horizontal = 1f;

            float startingZPosition = player.transform.position.z;
            
            yield return new WaitForSeconds(5f);

            float endingZPosition = player.transform.position.z;
            Assert.Greater(endingZPosition, startingZPosition);
        }
    }

    public class with_negative_vertical_input
    {
        [UnityTest]
        public IEnumerator moves_forward()
        {
            yield return Helpers.LoadMovementTestsScene();

            var player = Helpers.GetPlayer();
            
            player.PlayerInput.Vertical.Returns(-1f);
            //testPlayerInput.Horizontal = 1f;

            float startingZPosition = player.transform.position.z;
            
            yield return new WaitForSeconds(5f);

            float endingZPosition = player.transform.position.z;
            Assert.Less(endingZPosition, startingZPosition);
        }
    }
    
    public class with_negative_mouse_x
    {
        [UnityTest]
        public IEnumerator turns_left()
        {
            yield return Helpers.LoadMovementTestsScene();
            var player = Helpers.GetPlayer();

            player.PlayerInput.MouseX.Returns(-1f);

            var originalRotation = player.transform.rotation;
            yield return new WaitForSeconds(0.5f);

            float turnAmount = Helpers.CalculateTurn(originalRotation, player.transform.rotation);
            Assert.Less(turnAmount, 0);
        }
    }
    
    public class with_positive_mouse_x
    {
        [UnityTest]
        public IEnumerator turns_left()
        {
            yield return Helpers.LoadMovementTestsScene();
            var player = Helpers.GetPlayer();

            player.PlayerInput.MouseX.Returns(1f);

            var originalRotation = player.transform.rotation;
            yield return new WaitForSeconds(0.5f);

            float turnAmount = Helpers.CalculateTurn(originalRotation, player.transform.rotation);
            Assert.Greater(turnAmount, 0);
        }
    }
}