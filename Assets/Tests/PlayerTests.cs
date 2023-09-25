using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.TestTools;

public class PlayerTests
{
    private GameObject playerPrefab;

    [OneTimeSetUp]
    public void Setup()
    {
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
    }
    
    [Test]
    public void TestPlayerInstantiation()
    {
        GameObject playerObject = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        Assert.That(playerObject, !Is.Null);
    }
    
    [UnityTest]
    public IEnumerator MoveLeft()
    {
        Vector3 playerPos = Vector3.zero;
        Quaternion playerDir = Quaternion.identity; // the default direction the player is facing is enough
        GameObject player = GameObject.Instantiate(playerPrefab, playerPos, playerDir);
        
        player.GetComponent<PlayerController>().DoMovement(Vector3.left);
        yield return new WaitForSeconds(1);
        Assert.IsTrue(player.transform.position.x < playerPos.x);
    }
    
    [UnityTest]
    public IEnumerator MoveRight()
    {
        Vector3 playerPos = Vector3.zero;
        Quaternion playerDir = Quaternion.identity; // the default direction the player is facing is enough
        GameObject player = GameObject.Instantiate(playerPrefab, playerPos, playerDir);
        
        player.GetComponent<PlayerController>().DoMovement(Vector3.right);
        yield return new WaitForSeconds(1);
        Assert.IsTrue(player.transform.position.x > playerPos.x);
    }
    
    [UnityTest]
    public IEnumerator MoveUp()
    {
        Vector3 playerPos = Vector3.zero;
        Quaternion playerDir = Quaternion.identity; // the default direction the player is facing is enough
        GameObject player = GameObject.Instantiate(playerPrefab, playerPos, playerDir);
        
        player.GetComponent<PlayerController>().DoMovement(Vector3.forward);
        yield return new WaitForSeconds(1);
        Assert.IsTrue(player.transform.position.z > playerPos.z);
    }
    
    [UnityTest]
    public IEnumerator MoveDown()
    {
        Vector3 playerPos = Vector3.zero;
        Quaternion playerDir = Quaternion.identity; // the default direction the player is facing is enough
        GameObject player = GameObject.Instantiate(playerPrefab, playerPos, playerDir);
        
        player.GetComponent<PlayerController>().DoMovement(Vector3.back);
        yield return new WaitForSeconds(1);
        Assert.IsTrue(player.transform.position.z < playerPos.z);
    }
}