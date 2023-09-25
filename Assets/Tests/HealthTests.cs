using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthTests
{
    private GameObject testObject;
        
    [SetUp]
    public void SetUp()
    {
        testObject = GameObject.Instantiate(new GameObject());
        testObject.AddComponent<Health>();
    }
    
    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(testObject);
    }
    
    [Test]
    public void TestTakeDamage()
    {
        Health health = testObject.GetComponent<Health>();
        health.maxHealth = 10f;
        health.currentHealth = 10f;
        health.TakeDamage(2f);
        Assert.That(health.currentHealth == 8f);
    }
    
    [Test]
    public void TestHeal()
    {
        Health health = testObject.GetComponent<Health>();
        health.maxHealth = 10f;
        health.currentHealth = 2f;
        health.Heal(2f);
        Assert.That(health.currentHealth == 4f);
    }
    
    [Test]
    public void TestOverHeal()
    {
        Health health = testObject.GetComponent<Health>();
        health.maxHealth = 10f; 
        health.currentHealth = 2f;
        health.Heal(20f);
        Assert.That(health.currentHealth == 10f);
    }
}