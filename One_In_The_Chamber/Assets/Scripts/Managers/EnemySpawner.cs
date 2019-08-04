using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

public class EnemySpawner : Singleton<EnemySpawner> {

    #region SpawnPoint_Data

    [System.Serializable]
    private class SpawnPoint {
        internal delegate void OnSpawnTimerUp(Transform spawnLocation);

        internal OnSpawnTimerUp onSpawnTimerUpEvent;

        [SerializeField, Tooltip("The locations for this spawn point"), MustBeAssigned]
        private Transform[] spawnPointLocations;

        [SerializeField, Tooltip("The spawn timing interval"), PositiveValueOnly]
        private float spawnIntervalTime;

        private float spawnTimer;

        internal bool IsActive { get; set; }

        internal void OnUpdate(float deltaTime) {
            if (!IsActive) { return; }

            spawnTimer += deltaTime;

            if (spawnTimer >= spawnIntervalTime) {

                var spawnLocation = spawnPointLocations[Random.Range(0, spawnPointLocations.Length - 1)];
                onSpawnTimerUpEvent?.Invoke(spawnLocation);
                spawnTimer = 0f;
            }
        }
    }

    #endregion

    public delegate void OnEnemySpawned(Enemy newEnemy);

    public OnEnemySpawned onEnemySpawnedEvent;

    [SerializeField, Tooltip("The prefab for the enemy"), MustBeAssigned]
    private Enemy enemyPrefab;

    [Separator("Spawn properties")]
    [SerializeField, Tooltip("The spawn points for the enemies"), MustBeAssigned]
    private SpawnPoint[] enemySpawnPoints;

    private void Awake() {
        SetupAndEnableSpawnPoints();

        #region Local_Function

        void SetupAndEnableSpawnPoints() {
            foreach (var spawnPoint in enemySpawnPoints) {
                spawnPoint.onSpawnTimerUpEvent += SpawnEnemyOnTransform;
                spawnPoint.IsActive = true;

            }

            #endregion
        }
    }

    private void SpawnEnemyOnTransform(Transform transform) {
        var positionToSpawn = transform.position;

        var newEnemy = Instantiate(enemyPrefab);
        newEnemy.transform.position = positionToSpawn;

        onEnemySpawnedEvent?.Invoke(newEnemy);
    }

    private void Update() {
        foreach (var spawnPoint in enemySpawnPoints) {
            spawnPoint.OnUpdate(Time.deltaTime);
        }
    }

    public void SetActiveSpawner(bool enable) {
        foreach (var spawnPoint in enemySpawnPoints) {
            spawnPoint.IsActive = enable;
        }
    }
}
