using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

using Studious.Pooling;

namespace StudiousTests.Pooling
{

    public class ObjectPoolTest
    {

        Bullet _prefabObject;
        private ObjectPool<Bullet> _objectPool;
        private Vector3 _spawnPosition = Vector3.zero;

        [SetUp]
        public void Setup()
        {
            _prefabObject = new GameObject("BulletTest").AddComponent<Bullet>();
            _objectPool = new ObjectPool<Bullet>(_prefabObject, 10);
        }

        [Test]
        public void ConstructorTest()
        {
            Assert.AreEqual(10, _objectPool.pooledCount, "Constructor Failed.");
        }

        [Test]
        public void PullTest()
        {
            Bullet bullet = _objectPool.Pull();
            Assert.AreEqual(9, _objectPool.pooledCount, "Failed to pull object from ObjectPool.");

            bullet.ReturnToPool();
        }

        [Test]
        [TestCaseSource("ListOfPositions")]
        public void PullPositonTest(Vector3 position)
        {
            Bullet bullet = _objectPool.Pull(position);

            Assert.AreEqual(position, bullet.transform.position, "Failed to set the objects position.");
            _objectPool.Push(bullet);
        }

        [Test]
        [TestCaseSource("ListOfPositionsRotations")]
        public void PullPostionRotationTest(Vector3 position, Quaternion rotation)
        {
            Bullet bullet = _objectPool.Pull(position, rotation);

            Assert.AreEqual(rotation.eulerAngles, bullet.transform.rotation.eulerAngles, "Failed to set the objects position.");
            _objectPool.Push(bullet);
        }

        [Test]
        public void PushTest()
        {
            Bullet bullet = _objectPool.Pull();
            Assert.AreEqual(9, _objectPool.pooledCount, "Failed to pull object from ObjectPool.");
            _objectPool.Push(bullet);
            Assert.AreEqual(10, _objectPool.pooledCount, "Failed to push object back to ObjectPool.");
        }

        private static IEnumerable<Vector3> ListOfPositions
        {
            get
            {
                yield return new Vector3(0,0,0);
                yield return new Vector3(10, 10, 10);
                yield return new Vector3(20, 20, 20);
            }
        }

        private static IEnumerable<TestCaseData> ListOfPositionsRotations
        {
            get
            {
                yield return new TestCaseData(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                yield return new TestCaseData(new Vector3(10, 0, 0), Quaternion.Euler(30, 0, 0));
                yield return new TestCaseData(new Vector3(20, 0, 0), Quaternion.Euler(60, 0, 0));
            }
        }
    }
}
