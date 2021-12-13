using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_SnapNotZero
{
    // A Test behaves as an ordinary method
    [Test]
    public void Test_SnapNotZeroSimplePasses()
    {
        // Use the Assert class to test conditions

        // Tests to ensure all instances of InputMgr in the scene has a 'snapDist' value greater than zero.
        foreach(InputMgr tester in GameObject.FindObjectsOfType<InputMgr>())
        {
            Assert.Greater(tester.snapDist, 0f);
            Debug.Log("tested " + tester.gameObject.transform.root.name);
        }
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    //[UnityTest]
    //public IEnumerator Test_SnapNotZeroWithEnumeratorPasses()
    //{
    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}
}
